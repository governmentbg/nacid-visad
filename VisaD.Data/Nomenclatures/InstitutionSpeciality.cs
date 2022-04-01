using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Nomenclatures
{
	public class InstitutionSpeciality : IEntity
	{
		public int Id { get; set; }
		public int? ViewOrder { get; set; }
		public bool IsActive { get; set; }
		public int ExternalId { get; set; }
		public int InstitutionId { get; set; }
		public Institution Institution { get; set; }
		public int SpecialityId { get; set; }
		public Speciality Speciality { get; set; }
		public int? EducationalFormId { get; set; }
		public EducationFormType EducationalForm { get; set; }
		public double? Duration { get; set; }
		public virtual List<InstitutionSpecialityLanguage> InstitutionSpecialityLanguages { get; set; } = new List<InstitutionSpecialityLanguage>();

		public InstitutionSpeciality()
		{

		}

		public InstitutionSpeciality(int externalId, int institutionId, int specialityId, int? educationalFormId, double duration, bool isActive, List<InstitutionSpecialityLanguage> institutionSpecialityLanguages)
		{
			this.ExternalId = externalId;
			this.InstitutionId = institutionId;
			this.SpecialityId = specialityId;
			this.EducationalFormId = educationalFormId;
			this.Duration = duration;
			this.IsActive = isActive;

			foreach (var language in institutionSpecialityLanguages)
			{
				var institutionSpecialityLanguage = new InstitutionSpecialityLanguage {
					LanguageId = language.LanguageId
				};

				this.InstitutionSpecialityLanguages.Add(institutionSpecialityLanguage);
			}

		}

		public InstitutionSpeciality(InstitutionSpeciality institutionSpeciality)
			: this(institutionSpeciality.Id, institutionSpeciality.InstitutionId, institutionSpeciality.SpecialityId,
				  institutionSpeciality.EducationalFormId.Value, institutionSpeciality.Duration.Value, institutionSpeciality.IsActive, institutionSpeciality.InstitutionSpecialityLanguages)
		{

		}

		public void Update(int externalId, int institutionId, int specialityId, int? educationalFormId, double duration, bool isActive)
		{
			this.ExternalId = externalId;
			this.InstitutionId = institutionId;
			this.SpecialityId = specialityId;
			this.EducationalFormId = educationalFormId;
			this.Duration = duration;
			this.IsActive = isActive;
		}

		public void RemoveLanguage(int id)
		{
			var language = this.InstitutionSpecialityLanguages.Single(e => e.Id == id);
			this.InstitutionSpecialityLanguages.Remove(language);
		}
	}
}
