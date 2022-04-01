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
	public class GetCandidatePartQuery : IRequest<PartDto<CandidateDto>>
	{
		public int PartId { get; set; }

		public class Handler : IRequestHandler<GetCandidatePartQuery, PartDto<CandidateDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<PartDto<CandidateDto>> Handle(GetCandidatePartQuery request, CancellationToken cancellationToken)
			{
				var result = await context.Set<CandidatePart>()
					.AsNoTracking()
					.Select(e => new PartDto<CandidateDto> {
						Id = e.Id,
						Entity = new CandidateDto {
							FirstName = e.Entity.FirstName,
							LastName = e.Entity.LastName,
							OtherNames = e.Entity.OtherNames,
							BirthDate = e.Entity.BirthDate,
							BirthPlace = e.Entity.BirthPlace,
							Nationality = e.Entity.Nationality != null
											? new NomenclatureDto<Country> {
												Id = e.Entity.Nationality.Id,
												Name = e.Entity.Nationality.Name
											}
											: null,
							Country = e.Entity.Country != null
											? new NomenclatureDto<Country> {
												Id = e.Entity.Country.Id,
												Name = e.Entity.Country.Name
											}
											: null,
							PassportNumber = e.Entity.PassportNumber,
							PassportValidUntil = e.Entity.PassportValidUntil,
							Phone = e.Entity.Phone,
							Mail = e.Entity.Mail,
							ImgFile = new AttachedFile {
								Key = e.Entity.Key,
								Hash = e.Entity.Hash,
								Size = e.Entity.Size,
								Name = e.Entity.Name,
								MimeType = e.Entity.MimeType,
								DbId = e.Entity.DbId
							},
							FirstNameCyrillic = e.Entity.FirstNameCyrillic,
							LastNameCyrillic = e.Entity.LastNameCyrillic,
							OtherNamesCyrillic = e.Entity.OtherNamesCyrillic,
							OtherNationalities = e.Entity.OtherNationalities != null
											? e.Entity.OtherNationalities.Select(x => new Country { Id = x.NationalityId, Name = x.Nationality.Name }).ToList()
											: null,
							Document = new CandidatePassportDocumentDto {
								Id = e.Entity.CandidatePassportDocument.Id,
								AttachedFile = new AttachedFile {
									Key = e.Entity.CandidatePassportDocument.Key,
									Hash = e.Entity.CandidatePassportDocument.Hash,
									Size = e.Entity.CandidatePassportDocument.Size,
									Name = e.Entity.CandidatePassportDocument.Name,
									MimeType = e.Entity.CandidatePassportDocument.MimeType,
									DbId = e.Entity.CandidatePassportDocument.DbId,
								}
							}
						},
						State = e.State
					})
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				return result;
			}
		}
	}
}
