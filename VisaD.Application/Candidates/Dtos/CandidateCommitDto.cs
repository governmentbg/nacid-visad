using FileStorageNetCore.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Candidates.Dtos
{
	public class CandidateCommitDto : CommitDto
	{
		public PartDto<CandidateDto> CandidatePart { get; set; }

		public string ChangeStateDescription { get; set; }

		public static Expression<Func<CandidateCommit, CandidateCommitDto>> SelectExpression => e => new CandidateCommitDto {
			Id = e.Id,
			LotId = e.LotId,
			State = e.State,
			CandidatePart = new PartDto<CandidateDto> {
				Id = e.CandidatePart.Id,
				Entity = new CandidateDto {
					FirstName = e.CandidatePart.Entity.FirstName,
					LastName = e.CandidatePart.Entity.LastName,
					OtherNames = e.CandidatePart.Entity.OtherNames,
					BirthDate = e.CandidatePart.Entity.BirthDate,
					BirthPlace = e.CandidatePart.Entity.BirthPlace,
					Nationality = e.CandidatePart.Entity.Nationality != null
											? new NomenclatureDto<Country> {
												Id = e.CandidatePart.Entity.Nationality.Id,
												Name = e.CandidatePart.Entity.Nationality.Name
											}
											: null,
					Country = e.CandidatePart.Entity.Country != null
											? new NomenclatureDto<Country> {
												Id = e.CandidatePart.Entity.Country.Id,
												Name = e.CandidatePart.Entity.Country.Name
											}
											: null,
					PassportNumber = e.CandidatePart.Entity.PassportNumber,
					PassportValidUntil = e.CandidatePart.Entity.PassportValidUntil,
					Phone = e.CandidatePart.Entity.Phone,
					Mail = e.CandidatePart.Entity.Mail,
					ImgFile = new AttachedFile {
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
											? e.CandidatePart.Entity.OtherNationalities.Select(x => new Country { Id = x.NationalityId, Name = x.Nationality.Name }).ToList()
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
		};
	}
}
