using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Dtos
{
	public class CandidateSearchResultItemDto
	{
		public int LotId { get; set; }
		public int CommitId { get; set; }
		public CommitState State { get; set; }

		public string Name { get; set; }
		public string Nationality { get; set; }
		public string BirthPlace { get; set; }
		public DateTime BirthDate { get; set; }
		public string CyrillicName { get; set; }
		public List<Country> OtherNationalities { get; set; } = new List<Country>();
		public string Mail { get; set; }
		public string Phone { get; set; }
		public int ApplicationsCount { get; set; }
		public string Country { get; set; }

		//ForPdfExport
		public string ConvertedBirthDate { get; set; }

		//ForExcelExport
		public string ConvertedBirthInfo { get; set; }
		public string ConvertedContactInfo { get; set; }

		public static Expression<Func<CandidateCommit, CandidateSearchResultItemDto>> SelectExpression
			=> commit => new CandidateSearchResultItemDto {
				LotId = commit.LotId,
				CommitId = commit.Id,
				State = commit.State,
				Name = $"{commit.CandidatePart.Entity.FirstName} {commit.CandidatePart.Entity.LastName}",
				Nationality = commit.CandidatePart.Entity.Nationality.Name,
				BirthDate = commit.CandidatePart.Entity.BirthDate,
				BirthPlace = commit.CandidatePart.Entity.BirthPlace,
				CyrillicName = $"{commit.CandidatePart.Entity.FirstNameCyrillic} {commit.CandidatePart.Entity.LastNameCyrillic}",
				OtherNationalities = commit.CandidatePart.Entity.OtherNationalities != null
											? commit.CandidatePart.Entity.OtherNationalities.Select(x => new Country { Id = x.NationalityId, Name = x.Nationality.Name }).ToList()
											: null,
				Mail = commit.CandidatePart.Entity.Mail,
				Phone = commit.CandidatePart.Entity.Phone,
				Country = commit.CandidatePart.Entity.Country.Name,
				ConvertedBirthDate = commit.CandidatePart.Entity.BirthDate.ToString("dd.MM.yyyy"),
				ConvertedBirthInfo = commit.CandidatePart.Entity.Country.Name + ", " + commit.CandidatePart.Entity.BirthPlace + ", " + commit.CandidatePart.Entity.BirthDate.ToString("dd.MM.yyyy"),
				ConvertedContactInfo = commit.CandidatePart.Entity.Mail + ", " + commit.CandidatePart.Entity.Phone
			};
	}
}
