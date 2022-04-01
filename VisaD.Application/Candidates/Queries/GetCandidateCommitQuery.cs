using FileStorageNetCore.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Candidates.Queries
{
	public class GetCandidateCommitQuery : IRequest<CandidateCommitDto>
	{
		public int LotId { get; set; }
		public int CommitId { get; set; }

		public class Handler : IRequestHandler<GetCandidateCommitQuery, CandidateCommitDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<CandidateCommitDto> Handle(GetCandidateCommitQuery request, CancellationToken cancellationToken)
			{
				var commit = await context.Set<CandidateCommit>()
					.AsNoTracking()
					.Where(e => e.LotId == request.LotId && e.Id == request.CommitId)
					.Select(e => new CandidateCommitDto {
						Id = e.Id,
						LotId = e.LotId,
						State = e.State,
						ChangeStateDescription = e.ChangeStateDescription,
						CandidatePart = new PartDto<CandidateDto> {
							Id = e.CandidatePart.Id,
							Entity = new CandidateDto {
								FirstName = e.CandidatePart.Entity.FirstName,
								LastName = e.CandidatePart.Entity.LastName,
								OtherNames = e.CandidatePart.Entity.OtherNames,
								BirthDate = e.CandidatePart.Entity.BirthDate,
								BirthPlace = e.CandidatePart.Entity.BirthPlace,
								Country = e.CandidatePart.Entity.Country != null
											? new NomenclatureDto<Country> {
												Id = e.CandidatePart.Entity.Country.Id,
												Name = e.CandidatePart.Entity.Country.Name
											}
											: null,
								Nationality = e.CandidatePart.Entity.Nationality != null
											? new NomenclatureDto<Country> {
												Id = e.CandidatePart.Entity.Nationality.Id,
												Name = e.CandidatePart.Entity.Nationality.Name
											}
											: null,
								PassportNumber = e.CandidatePart.Entity.PassportNumber,
								PassportValidUntil = e.CandidatePart.Entity.PassportValidUntil,
								Phone = e.CandidatePart.Entity.Phone,
								Mail = e.CandidatePart.Entity.Mail,
								ImgFile =  new AttachedFile { 
									Key = e.CandidatePart.Entity.Key,
									Hash = e.CandidatePart.Entity.Hash,
									Size = e.CandidatePart.Entity.Size,
									Name = e.CandidatePart.Entity.Name,
									MimeType = e.CandidatePart.Entity.MimeType,
									DbId = e.CandidatePart.Entity.DbId
								},
								FirstNameCyrillic = e.CandidatePart.Entity.FirstNameCyrillic,
								LastNameCyrillic = e.CandidatePart.Entity.LastNameCyrillic,
								OtherNamesCyrillic = e.CandidatePart.Entity.OtherNamesCyrillic,
								OtherNationalities = e.CandidatePart.Entity.OtherNationalities != null
											? e.CandidatePart.Entity.OtherNationalities.Select(x => new Country { Id = x.NationalityId, Name = x.Nationality.Name}).ToList()
											: null,
								Document = new CandidatePassportDocumentDto {
									Id = e.CandidatePart.Entity.CandidatePassportDocument.Id,
									AttachedFile = new AttachedFile {
										Key = e.CandidatePart.Entity.CandidatePassportDocument.Key,
										Hash = e.CandidatePart.Entity.CandidatePassportDocument.Hash,
										Size = e.CandidatePart.Entity.CandidatePassportDocument.Size,
										Name = e.CandidatePart.Entity.CandidatePassportDocument.Name,
										MimeType = e.CandidatePart.Entity.CandidatePassportDocument.MimeType,
										DbId = e.CandidatePart.Entity.CandidatePassportDocument.DbId,
									}
								} 
							},
							State = e.CandidatePart.State
						}
					})
					.SingleOrDefaultAsync(cancellationToken);

				return commit;
			}
		}
	}
}
