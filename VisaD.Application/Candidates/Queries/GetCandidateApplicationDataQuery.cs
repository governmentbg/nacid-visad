using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Candidates.Queries
{
	public class GetCandidateApplicationDataQuery : IRequest<CandidateApplicationDataDto>
	{
		public int CandidateCommitId { get; set; }

		public class Handler : IRequestHandler<GetCandidateApplicationDataQuery, CandidateApplicationDataDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<CandidateApplicationDataDto> Handle(GetCandidateApplicationDataQuery request, CancellationToken cancellationToken)
			{
				var candidateData = new CandidateApplicationDataDto();

				var candidateLotId = await this.context.Set<CandidateCommit>()
					.Where(x => x.Id == request.CandidateCommitId)
					.Select(x => x.LotId)
					.SingleOrDefaultAsync(cancellationToken);

				var applicationCommit = await this.context.Set<ApplicationCommit>()
					.Include(e => e.DiplomaPart.Entity.DiplomaFiles)
						.ThenInclude(e => e.Country)
					.Include(e => e.DiplomaPart.Entity.DiplomaFiles)
						.ThenInclude(e => e.DiplomaDocumentFiles)
					.Include(e => e.DiplomaPart.Entity.DiplomaFiles)
						.ThenInclude(e => e.DiplomaType)
					.Include(e => e.Lot.Result)
					.Where(e => (e.Lot.Result.Type == ApplicationLotResultType.Certificate || e.Lot.Result.Type == ApplicationLotResultType.Rejection) && e.CandidateCommit.LotId == candidateLotId)
					.OrderBy(e => e.LotId)
					.LastOrDefaultAsync(cancellationToken);

				if (applicationCommit != null)
				{
					candidateData.PreviousApplication = new PreviousApplicationDto {
						HasPreviousApplication = true,
						PreviousApplicationRegisterNumber = applicationCommit.Lot.Result.CertificateNumber,
						PreviousApplicationYear = applicationCommit.Lot.Result.CreateDate.Year,
						PreviousApplicationLotId = applicationCommit.LotId,
						PreviousApplicationCommitId = applicationCommit.Id
					};

					candidateData.Diploma = new DiplomaDto();
					candidateData.Diploma.Description = applicationCommit.DiplomaPart.Entity.Description;

					var diplomas = new List<DiplomaFileDto>();

					foreach (var diploma in applicationCommit.DiplomaPart.Entity.DiplomaFiles)
					{
						var diplomaDto = new DiplomaFileDto {
							Id = diploma.Id,
							IssuedDate = diploma.IssuedDate,
							City = diploma.City,
							Country = new NomenclatureDto<Country> {
								Id = diploma.Country.Id,
								Name = diploma.Country.Name
							},
							DiplomaNumber = diploma.DiplomaNumber,
							OrganizationName = diploma.OrganizationName,
							Type = new DiplomaTypeNomenclatureDto {
								Id = diploma.DiplomaType.Id,
								Name = diploma.DiplomaType.Name,
								IsNacidVerificationRequired = diploma.DiplomaType.IsNacidVerificationRequired,
								Alias = diploma.DiplomaType.Alias
							},
							DiplomaDocumentFile = diploma.DiplomaDocumentFiles.SingleOrDefault(x => x.Type == DiplomaDocumentType.Diploma),
							AttachedFiles = diploma.DiplomaDocumentFiles.Where(x => x.Type != DiplomaDocumentType.Diploma).ToList()
						};

						diplomas.Add(diplomaDto);
					}

					candidateData.Diploma.DiplomaFiles = diplomas;
				}

				return candidateData;
			}
		}
	}
}
