using System;
using System.Linq;
using System.Linq.Expressions;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicationPdfDto
	{
		private const string doctor = "доктор";
		private const string trainee = "специализант";
		private const string doctorLabel = "образователна и научна степен";
		private const string notDoctorLabel = "образователно-квалификационна степен";

		public string RegUri { get; set; }
		public string EmbeddedImage { get; set; }
		public string Note { get; set; }
		public string NoteDisplay { get; set; }
		public string AccessCode { get; set; }


		public string CandidateFirstName { get; set; }
		public string CandidateLastName { get; set; }
		public string CandidateBirthDate { get; set; }
		public string CandidateNationality { get; set; }
		public string CandidatePassportNumber { get; set; }
		public string CandidatePassportValidUntil { get; set; }

		public string Institution { get; set; }
		public string Speciality { get; set; }
		public string SpecialityLanguage { get; set; }
		public string EducationalQualification { get; set; }
		public string QualificationLabel { get; set; }
		public string EducationFormType { get; set; }
		public double? Duration { get; set; }
		public string SchoolYear { get; set; }
		public string Semester { get; set; }
		public string StudentLabel { get; set; }
		public string ShowTrainee { get; set; }
		public string HideTrainee { get; set; }
		public string Specialization { get; set; }
		public string TraineeDuration { get; set; }

		public string LanguageDepartment { get; set; }
		public int? LanguageTrainingDuration { get; set; }
		public string TrainingDisplay { get; set; }

		public string EducationTaxDisplay { get; set; }
		public decimal? EducationAmount { get; set; }
		public string EducationCurrency { get; set; }
		public string EducationBank { get; set; }
		public string EducationBic { get; set; }
		public string EducationIban { get; set; }

		public string LanguageTaxDisplay { get; set; }
		public decimal? LanguageAmount { get; set; }
		public string LanguageCurrency { get; set; }
		public string LanguageBank { get; set; }
		public string LanguageBic { get; set; }
		public string LanguageIban { get; set; }

		public string BothTaxDisplay { get; set; }

		public string Regulation { get; set; }

		public string Signer { get; set; }
		public string SignerPosition { get; set; }
		public string ShowSignerInfo { get; set; }

		public static Expression<Func<ApplicationCommit, ApplicationPdfDto>> SelectExpression => e => new ApplicationPdfDto {
			RegUri = e.Lot.RegisterNumber,

			CandidateFirstName = e.CandidateCommit.CandidatePart.Entity.FirstName,
			CandidateLastName = e.CandidateCommit.CandidatePart.Entity.LastName,
			CandidateBirthDate = e.CandidateCommit.CandidatePart.Entity.BirthDate.ToString("dd.MM.yyyy"),
			CandidateNationality = e.CandidateCommit.CandidatePart.Entity.Nationality.Name,
			CandidatePassportNumber = e.CandidateCommit.CandidatePart.Entity.PassportNumber,
			CandidatePassportValidUntil = e.CandidateCommit.CandidatePart.Entity.PassportValidUntil.ToString("dd.MM.yyyy"),

			Institution = e.ApplicantPart.Entity.Institution.Name,
			Speciality = e.EducationPart.Entity.Speciality.Name,
            SpecialityLanguage = e.EducationPart.Entity.EducationSpecialityLanguages.FirstOrDefault().Language.Name,
            EducationalQualification = e.EducationPart.Entity.EducationalQualification.Name,
			QualificationLabel = e.EducationPart.Entity.EducationalQualification.Name.Trim().ToLower().Contains(doctor.Trim().ToLower()) ? doctorLabel : notDoctorLabel,
			EducationFormType = e.EducationPart.Entity.Form.Name,
			Duration = e.EducationPart.Entity.Duration.Value,
			SchoolYear = e.EducationPart.Entity.SchoolYear.Name.Substring(0, 9),
			Semester = e.EducationPart.Entity.SchoolYear.Name.Substring(9),
			StudentLabel = e.EducationPart.Entity.EducationalQualification.Name.Trim().ToLower().Contains(doctor.Trim().ToLower()) ? "докторант" : "студент",
			ShowTrainee = e.EducationPart.Entity.EducationalQualification.Name.Trim().ToLower().Contains(trainee.Trim().ToLower()) ? "inline" : "none",
			HideTrainee = !e.EducationPart.Entity.EducationalQualification.Name.Trim().ToLower().Contains(trainee.Trim().ToLower()) ? "inline" : "none",
			Specialization = e.EducationPart.Entity.Specialization,
			TraineeDuration = e.EducationPart.Entity.TraineeDuration,

			LanguageDepartment = !string.IsNullOrWhiteSpace(e.TrainingPart.Entity.LanguageDepartment) ? e.TrainingPart.Entity.LanguageDepartment : null,
			LanguageTrainingDuration = e.TrainingPart.Entity.LanguageTrainingDuration ?? null,
            TrainingDisplay = string.IsNullOrWhiteSpace(e.TrainingPart.Entity.LanguageDepartment) ? "none" : "block",

            EducationTaxDisplay = string.IsNullOrWhiteSpace(e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Iban) ? "block" : "none",
            EducationAmount = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.EducationTax).Amount.Value,
            EducationCurrency = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.EducationTax).CurrencyType.Name,
            EducationBank = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.EducationTax).Bank,
            EducationBic = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.EducationTax).Bic,
            EducationIban = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.EducationTax).Iban,

            LanguageTaxDisplay = !string.IsNullOrWhiteSpace(e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Iban) &&
                !e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.EducationTax).Iban.Trim().ToLower().Contains(e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Iban.Trim().ToLower()) ? "block" : "none",
            LanguageAmount = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Amount.Value,
            LanguageCurrency = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).CurrencyType.Name,
            LanguageBank = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Bank,
            LanguageBic = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Bic,
            LanguageIban = e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Iban,

            BothTaxDisplay = !string.IsNullOrWhiteSpace(e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Iban) &&
                e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.EducationTax).Iban.Trim().ToLower().Contains(e.TaxAccountPart.Entity.Taxes.SingleOrDefault(e => e.Type == TaxType.TrainingTax).Iban.Trim().ToLower()) ? "initial" : "none"
		};
	}
}
