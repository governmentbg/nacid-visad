using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Queries.Parts
{
	public class GetDiplomaPartQuery : IRequest<PartDto<DiplomaDto>>
	{
		public int PartId { get; set; }

		public class Handler : IRequestHandler<GetDiplomaPartQuery, PartDto<DiplomaDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<PartDto<DiplomaDto>> Handle(GetDiplomaPartQuery request, CancellationToken cancellationToken)
			{
				var result2 = await this.context.Set<DiplomaPart>().SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				var result = await this.context.Set<DiplomaPart>()
					.AsNoTracking()
					.Select(e => new PartDto<DiplomaDto> {
						Id = e.Id,
						Entity = new DiplomaDto {
							Id = e.Entity.Id,
							Description = e.Entity.Description,
							RectorDecisionDocumentFile = e.Entity.AttachedFiles.SingleOrDefault(x => x.Type == DiplomaDocumentType.RectorDecision),
							NacidRecommendation = e.Entity.AttachedFiles.SingleOrDefault(x => x.Type == DiplomaDocumentType.NacidRecommendation),
							DiplomaFiles = e.Entity.DiplomaFiles
											.Select(df => new DiplomaFileDto {
												Id = df.Id,
												DiplomaNumber = df.DiplomaNumber,
												IssuedDate = df.IssuedDate,
												Country = df.Country != null
											? new NomenclatureDto<Country> {
												Id = df.Country.Id,
												Name = df.Country.Name
											}
											: null,
												City = df.City,
												OrganizationName = df.OrganizationName,
												Type = df.DiplomaType != null
												? new DiplomaTypeNomenclatureDto {
													Id = df.DiplomaType.Id,
													Name = df.DiplomaType.Name,
													IsNacidVerificationRequired = df.DiplomaType.IsNacidVerificationRequired,
													Alias = df.DiplomaType.Alias
												}
												: null,
												DiplomaDocumentFile = df.DiplomaDocumentFiles.SingleOrDefault(x => x.Type == DiplomaDocumentType.Diploma),
												AttachedFiles = df.DiplomaDocumentFiles.Where(x => x.Type == DiplomaDocumentType.OtherDocument).ToList()
											})
						},
						State = e.State
					}).SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				return result;
			}
		}
	}
}
