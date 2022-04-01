using FileStorageNetCore.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Commands;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Queries
{
    public class GetApplicationResultSigningInformationQuery : IRequest<ApplicationResultSigningInformationDto>
	{
		public int ResultId { get; set; }

		public class Handler : IRequestHandler<GetApplicationResultSigningInformationQuery, ApplicationResultSigningInformationDto>
		{
			private readonly IAppDbContext context;
			private readonly BlobStorageService blobStorageService;
			private readonly IMediator mediator;

			public Handler(
				IAppDbContext context,
				BlobStorageService blobStorageService,
				IMediator mediator
			)
			{
				this.context = context;
				this.blobStorageService = blobStorageService;
				this.mediator = mediator;
			}

			public async Task<ApplicationResultSigningInformationDto> Handle(GetApplicationResultSigningInformationQuery request, CancellationToken cancellationToken)
			{
				var result = await this.context.Set<ApplicationLotResult>()
					.Where(e => e.Id == request.ResultId)
					.Include(e => e.File)
					.SingleAsync(cancellationToken);

				await this.mediator.Send(new AddApplicationLotResultCommand {
					LotId = result.LotId,
					Note = result.Note,
					Regulation = new NomenclatureDto<Regulation> { Id = result.RegulationId.Value, },
					Type = result.Type
				});

				var content = await this.blobStorageService.GetBytes(result.File.Key, result.File.DbId);
				return new ApplicationResultSigningInformationDto {
					Content = Convert.ToBase64String(content),
					SignatureLineIds = new List<string> { "Signature" },
					//Filename = applicationLotResultInformation.Filename
					Filename = "test.pdf"
				};
			}
		}
	}
}
