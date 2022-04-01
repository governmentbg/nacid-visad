using EnumsNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicationSearchResultItemDto
	{
		public int LotId { get; set; }
		public int CommitId { get; set; }
		public CommitState State { get; set; }
		public string RegisterNumber { get; set; }

		public bool HasResult { get; set; }
		public ApplicationLotResultType? ResultType { get; set; }
		public bool IsSigned { get; set; }
		public string FilePdfUrl { get; set; }

		public string CandidateName { get; set; }
		public string CandidateNationality { get; set; }
		public string CandidateCyrillicName { get; set; }
		public string CandidateCountry { get; set; }
		public string CandidateBirthPlace { get; set; }
		public DateTime BirthDate { get; set; }
		public List<Country> OtherNationalities { get; set; } = new List<Country>();

		public string OrganizationName { get; set; }
		public string Mail { get; set; }
		public string Speciality { get; set; }
		public string EducationalQualification { get; set; }
		public string Form { get; set; }
		public string Representative { get; set; }
		public string Specialization { get; set; }

		public string ApplicantName { get; set; }
		public string ApplicantMail { get; set; }
		public string ApplicantPhone { get; set; }

		//ForPdfExport
		public string ConvertedBirthDate { get; set; }
		public string ResultTypeDescription { get; set; }

		//ForExcelExport
		public string CandidateInfo { get; set; }
		public string SpecialityInfo { get; set; }
		public string FormEducationInfo { get; set; }

		public static Expression<Func<ApplicationCommit, ApplicationSearchResultItemDto>> SelectExpression
			=> commit => new ApplicationSearchResultItemDto {
				LotId = commit.LotId,
				CommitId = commit.Id,
				State = commit.State,
				RegisterNumber = commit.Lot.RegisterNumber,

				HasResult = commit.Lot.Result != null,
				ResultType = commit.Lot.Result != null ? commit.Lot.Result.Type : (ApplicationLotResultType?)null,
				IsSigned = commit.Lot.Result.IsSigned,
				FilePdfUrl = commit.Lot.Result.File != null
							? $"/api/FilesStorage?key={commit.Lot.Result.File.Key}&fileName={commit.Lot.Result.File.Name}&dbId={commit.Lot.Result.File.DbId}"
							: null,

				CandidateName = $"{commit.CandidateCommit.CandidatePart.Entity.FirstName} {commit.CandidateCommit.CandidatePart.Entity.LastName}",
				CandidateNationality = commit.CandidateCommit.CandidatePart.Entity.Nationality.Name,
				CandidateCyrillicName = $"{commit.CandidateCommit.CandidatePart.Entity.FirstNameCyrillic} {commit.CandidateCommit.CandidatePart.Entity.LastNameCyrillic}",
				CandidateCountry = commit.CandidateCommit.CandidatePart.Entity.Country.Name,
				BirthDate = commit.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateBirthPlace = commit.CandidateCommit.CandidatePart.Entity.BirthPlace,
				OtherNationalities = commit.CandidateCommit.CandidatePart.Entity.OtherNationalities != null
											? commit.CandidateCommit.CandidatePart.Entity.OtherNationalities.Select(x => new Country { Id = x.NationalityId, Name = x.Nationality.Name }).ToList()
											: null,

				OrganizationName = commit.ApplicantPart.Entity.Institution.Name,
				Mail = commit.CandidateCommit.CandidatePart.Entity.Mail,
				Speciality = commit.EducationPart.Entity.Speciality.Name,
				EducationalQualification = commit.EducationPart.Entity.EducationalQualification.Name,
				Form = commit.EducationPart.Entity.Form.Name,
				Representative = commit.RepresentativePart.Entity.HasRepresentative ? commit.RepresentativePart.Entity.Fullname : null,
				Specialization = commit.EducationPart.Entity.Specialization,

				ApplicantName = $"{commit.ApplicantPart.Entity.FirstName} {commit.ApplicantPart.Entity.LastName}",
				ApplicantMail = commit.ApplicantPart.Entity.Mail,
				ApplicantPhone = commit.ApplicantPart.Entity.Phone,

				ConvertedBirthDate = commit.CandidateCommit.CandidatePart.Entity.BirthDate.ToString("dd.MM.yyyy"),
				ResultTypeDescription = commit.Lot.Result.Type.AsString(EnumFormat.Description),

				CandidateInfo = commit.CandidateCommit.CandidatePart.Entity.Country.Name + ", "  + commit.CandidateCommit.CandidatePart.Entity.BirthDate.ToString("dd.MM.yyyy"),
				SpecialityInfo = commit.EducationPart.Entity.Speciality.Name != null  ? commit.EducationPart.Entity.Speciality.Name : commit.EducationPart.Entity.Specialization,
				FormEducationInfo = commit.EducationPart.Entity.EducationalQualification.Name + ", " + commit.EducationPart.Entity.Form.Name,
			};
	}
}
