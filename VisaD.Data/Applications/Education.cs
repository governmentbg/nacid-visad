using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications
{
	public class Education : IEntity, IAuditable
	{
		public int Id { get; private set; }

		public int? SpecialityId { get; private set; }
		public Speciality Speciality { get; private set; }
		public int? EducationalQualificationId { get; private set; }
		public EducationalQualification EducationalQualification { get; private set; }
		public int? FormId { get; private set; }
		public EducationFormType Form { get; private set; }
		public int? InstitutionId { get; private set; }
		public Institution Institution { get; private set; }

		public int? SchoolYearId { get; private set; }
		public SchoolYear SchoolYear { get; private set; }

		public double? Duration { get; private set; }
		public string TraineeDuration { get; private set; }

		public List<EducationSpecialityLanguage> EducationSpecialityLanguages { get; private set; } = new List<EducationSpecialityLanguage>();

		public string Specialization { get; set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		private Education()
		{

		}

		public Education(int? specialityId, int? schoolYearId, int? qualificationId, int? formId, double? duration, int? institutionId, List<EducationSpecialityLanguage> educationSpecialityLanguages, string specialization, string traineeDuration)
		{
			this.InstitutionId = institutionId;
			this.SpecialityId = specialityId;
			this.SchoolYearId = schoolYearId;
			this.EducationalQualificationId = qualificationId;
			this.FormId = formId;
			this.Duration = duration;
			this.Specialization = specialization;
			this.TraineeDuration = traineeDuration;

			if (educationSpecialityLanguages != null)
			{
				foreach (var language in educationSpecialityLanguages)
				{
					var newLanguage = new EducationSpecialityLanguage { LanguageId = language.LanguageId };

					this.EducationSpecialityLanguages.Add(newLanguage);
				}
			}
		}

		public Education(Education education)
			: this(education.SpecialityId, education.SchoolYearId, education.EducationalQualificationId, education.FormId, education.Duration, education.InstitutionId, education.EducationSpecialityLanguages, education.Specialization, education.TraineeDuration)
		{

		}

		public void Update(int? specialityId, int? schoolYearId, int? qualificationId, int? formId, double? duration, int? institutionId, string specialization, string traineeDuration)
		{
			this.InstitutionId = institutionId;
			this.SpecialityId = specialityId;
			this.SchoolYearId = schoolYearId;
			this.EducationalQualificationId = qualificationId;
			this.FormId = formId;
			this.Duration = duration;
			this.Specialization = specialization;
			this.TraineeDuration = traineeDuration;
		}

		public void RemoveLanguage(int id)
		{
			var language = this.EducationSpecialityLanguages.Single(e => e.Id == id);
			this.EducationSpecialityLanguages.Remove(language);
		}
	}
}
