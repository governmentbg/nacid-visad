using FileStorageNetCore.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Ems.Converters;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Nomenclatures.Constants;
using VisaD.Infrastructure.Ems;
using VisaD.Infrastructure.Ems.Enums;
using VisaD.Infrastructure.Ems.Models;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateApplicationLotResultFileCommand : IRequest<ApplicationLotResultDto>
	{
		const string ElectornicServiceUri = "VisaApplication";

		public int ResultId { get; set; }
		public string Content { get; set; }

		public class Handler : IRequestHandler<UpdateApplicationLotResultFileCommand, ApplicationLotResultDto>
		{
			private readonly IAppDbContext context;
			private readonly BlobStorageService blobStorageService;
			private readonly IUserContext userContext;
			private readonly IMediator mediator;
			private readonly AuthConfiguration authConfig;
			private readonly IEmsApplicationConverter converter;
			private readonly EmsService emsService;

			public Handler(
				IAppDbContext context,
				BlobStorageService blobStorageService,
				IUserContext userContext,
				IOptions<AuthConfiguration> options,
				IMediator mediator,
				IEmsApplicationConverter converter,
				EmsService emsService
			)
			{
				this.context = context;
				this.blobStorageService = blobStorageService;
				this.userContext = userContext;
				this.mediator = mediator;
				this.authConfig = options.Value;
				this.converter = converter;
				this.emsService = emsService;
			}

			public async Task<ApplicationLotResultDto> Handle(UpdateApplicationLotResultFileCommand request, CancellationToken cancellationToken)
			{
				var result = await context.Set<ApplicationLotResult>()
					.Include(e => e.File)
					.Include(e => e.Lot)
						.ThenInclude(l => l.Commits)
							.ThenInclude(c => c.ApplicantPart)
								.ThenInclude(ap => ap.Entity)
					.SingleAsync(e => e.Id == request.ResultId, cancellationToken);

				var newFile = await blobStorageService.Post(Convert.FromBase64String(request.Content), result.File.Name, result.File.MimeType);

				result.File.Key = newFile.Key;
				result.File.Hash = newFile.Hash;
				result.File.Size = newFile.Size;

				result.Sign(this.userContext.UserId);

				var commit = result.Lot.Commits
					.Where(c => c.LotId == result.LotId && c.State == CommitState.Approved)
					.SingleOrDefault();

				EmsApplication emsApplication = this.converter.ToEmsApplication(ElectornicServiceUri, commit.ApplicantPart, result.Lot.RegisterNumber, result.File, true);
				var pendingEmsApplication = new {
					Application = JsonConvert.SerializeObject(emsApplication),
					//Signature = application.Signature,
					Status = EmsIncomingDocStatus.Pending
				};

				var docGuid = await emsService.SubmitApplicationAsync(JsonConvert.SerializeObject(pendingEmsApplication));
				//var response = await Policy
				//    .HandleResult<EmsDocStatusResponse>(e => e.Status == EmsIncomingDocStatus.Pending)
				//    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
				//    .ExecuteAsync(async () => await emsService.GetEmsApplicationStatus(docGuid))
				//;

				var templateData = new {
					VisaDAddress = this.authConfig.Issuer,
					CertificateNumber = result.CertificateNumber,
					AccessCode = result.AccessCode
				};

				await this.mediator.Send(new SendApplicationEmailCommand {
					Alias = EmailTypeAlias.NEW_CERTIFICATE,
					CreatorUserId = result.CreatorUserId,
					TemplateData = templateData
				});

				await context.SaveChangesAsync(cancellationToken);

				return new ApplicationLotResultDto {
					Id = result.Id,
					Type = result.Type,
					Note = result.Note,
					AttachedFilePath = $"api/FilesStorage?key={result.File.Key}&fileName={result.File.Name}&dbId={result.File.DbId}",
					CertificateNumber = result.CertificateNumber,
					AccessCode = result.AccessCode,
					IsSigned = result.IsSigned
				};
			}
		}
	}
}
