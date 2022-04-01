using FileStorageNetCore.Models;
using System;
using System.Collections.Generic;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Candidates;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Candidates.Dtos
{
	public class CandidateDto
	{
		public string PassportNumber { get; set; }
		public DateTime PassportValidUntil { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string OtherNames { get; set; }

		public DateTime BirthDate { get; set; }
		public string BirthPlace { get; set; }

		public NomenclatureDto<Country> Country { get; set; }
		public NomenclatureDto<Country> Nationality { get; set; }

		public string Phone { get; set; }
		public string Mail { get; set; }

		public AttachedFile ImgFile { get; set; }

		public string FirstNameCyrillic { get; set; }
		public string LastNameCyrillic { get; set; }
		public string OtherNamesCyrillic { get; set; }

		public List<Country> OtherNationalities { get; set; } = new List<Country>();

		public CandidatePassportDocumentDto Document { get; set; }

		public Candidate ToModel()
		{
			var candidate = new Candidate(this.FirstName, this.LastName, this.BirthDate, this.BirthPlace, this.Nationality.Id, this.PassportNumber.Trim(), this.PassportValidUntil, 
				this.Country.Id, this.Phone, this.Mail, this.ImgFile.Key, this.ImgFile.Hash, this.ImgFile.Size, this.ImgFile.Name, this.ImgFile.MimeType, this.ImgFile.DbId,
				this.OtherNames, this.FirstNameCyrillic, this.LastNameCyrillic, this.OtherNamesCyrillic);

			foreach (var nationality in this.OtherNationalities)
			{
				candidate.AddNationality(nationality.Id);
			}

			candidate.AddFile(
				this.Document.AttachedFile.Key,
				this.Document.AttachedFile.Hash,
				this.Document.AttachedFile.Size,
				this.Document.AttachedFile.Name,
				this.Document.AttachedFile.MimeType,
				this.Document.AttachedFile.DbId
				);

			return candidate;
		}
	}
}
