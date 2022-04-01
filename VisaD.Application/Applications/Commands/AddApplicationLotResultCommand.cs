using FileStorageNetCore.Api;
using FileStorageNetCore.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Common.Constants;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Common.Models;
using VisaD.Application.Common.Services;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Application.Utils;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;

namespace VisaD.Application.Applications.Commands
{
	public class AddApplicationLotResultCommand : IRequest<ApplicationLotResultDto>
	{
		public int LotId { get; set; }
		public ApplicationLotResultType Type { get; set; }
		public string Note { get; set; }
		public NomenclatureDto<Regulation> Regulation { get; set; }

		public class Handler : IRequestHandler<AddApplicationLotResultCommand, ApplicationLotResultDto>
		{
			private readonly BlobStorageService blobStorageService;
			private readonly QrCodeService qrCodeService;
			private readonly IAppDbContext context;
			private readonly IPdfFileService applicationFileService;
			private readonly ITemplateService templateService;
			private readonly IUserContext userContext;
			private readonly ApplicationFileConfiguration applicationFileConfiguration;

			public Handler(
				BlobStorageService blobStorageService,
				QrCodeService qrCodeService,
				IAppDbContext context,
				IPdfFileService applicationFileService,
				ITemplateService templateService,
				IOptions<ApplicationFileConfiguration> applicationFileOptions,
				IUserContext userContext
				)
			{
				this.blobStorageService = blobStorageService;
				this.qrCodeService = qrCodeService;
				this.context = context;
				this.applicationFileService = applicationFileService;
				this.templateService = templateService;
				this.userContext = userContext;
				this.applicationFileConfiguration = applicationFileOptions.Value;
			}

			public async Task<ApplicationLotResultDto> Handle(AddApplicationLotResultCommand request, CancellationToken cancellationToken)
			{
				var lot = await this.context.Set<ApplicationLot>()
					.Include(e => e.Result)
						.ThenInclude(r => r.File)
					.SingleAsync(e => e.Id == request.LotId, cancellationToken);
				
				var generator = new RandomStringGenerator(8);
				string accessCode = string.Empty;
				bool hasSameCode = false;
				do
				{
					accessCode = generator.Generate();
					hasSameCode = await this.context.Set<ApplicationLotResult>()
						.AnyAsync(e => e.AccessCode == accessCode, cancellationToken);
				} while (hasSameCode);

				var regulation = await this.context.Set<Regulation>()
					.SingleOrDefaultAsync(e => e.Id == request.Regulation.Id);

				lot.UpdateResult(request.Type, request.Note, lot.RegisterNumber, accessCode, request.Regulation.Id);

				var certificateFile = await this.GetResultPdfFile(request.LotId, accessCode, 
					request.Type == ApplicationLotResultType.Certificate ? FileTemplateAliases.CERTIFICATE_TEMPLATE : FileTemplateAliases.REJECTION_TEMPLATE, 
					regulation.FullName, request.Note, cancellationToken);

				lot.Result.AddFile(certificateFile.Key, certificateFile.Hash, certificateFile.Size, certificateFile.Name, certificateFile.MimeType, certificateFile.DbId);
				
				await this.context.SaveChangesAsync(cancellationToken);

				return new ApplicationLotResultDto {
					Id = lot.Result.Id,
					Type = lot.Result.Type,
					Note = lot.Result.Note,
					CertificateNumber = lot.Result.CertificateNumber,
					AttachedFilePath = $"api/FilesStorage?key={lot.Result.File.Key}&fileName={lot.Result.File.Name}&dbId={lot.Result.File.DbId}",
					AccessCode = lot.Result.AccessCode,
					IsSigned = lot.Result.IsSigned
				};
			}

			private async Task<AttachedFile> GetResultPdfFile(int lotId, string accessCode, string templateAlias, string regulation, string note, CancellationToken cancellationToken)
			{
				var applicationData = await context.Set<ApplicationCommit>()
					.Where(e => e.LotId == lotId && e.State == CommitState.Approved)
					.Include(e => e.Lot.Result)
					.Select(ApplicationPdfDto.SelectExpression)
					.SingleAsync(cancellationToken);

				applicationData.AccessCode = accessCode;
				applicationData.EmbeddedImage = this.qrCodeService.Create(applicationFileConfiguration.PreviewUrlTemplate + $"{accessCode}");
				applicationData.Regulation = regulation;
				applicationData.Note = note;
				applicationData.NoteDisplay = string.IsNullOrWhiteSpace(note) ? "none" : "block";
				applicationData.ShowSignerInfo = this.userContext.Role == UserRoleAliases.RESULT_SIGNER_USER ? "block" : "none";

				if (this.userContext.Role == UserRoleAliases.RESULT_SIGNER_USER)
				{
					var user = await this.context.Set<User>()
						.SingleOrDefaultAsync(x => x.Id == this.userContext.UserId);

					applicationData.SignerPosition = user.Position;
					applicationData.Signer = user.FirstName + " " + user.LastName;
				}

				var template = await this.templateService.GetTemplateAsync(templateAlias);

				var signFieldSettings = new PdfSignFieldSettings {
					FieldName = "Signature",
					Width = 150,
					Height = 50,
					Margin = 5
				};
				var bytes = await this.applicationFileService.GenerateSignedPdfFile(applicationData, template, signFieldSettings);
				var file = await this.blobStorageService.Post(bytes, "ApplicationLotResultFile", "application/pdf");

				return file;
			}
		}
	}
}
