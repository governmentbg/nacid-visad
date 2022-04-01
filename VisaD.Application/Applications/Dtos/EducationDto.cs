using System.Collections.Generic;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data;
using VisaD.Data.Applications;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Dtos
{
	public class EducationDto
	{
		public NomenclatureDto<Speciality> Speciality { get; set; }
		public NomenclatureDto<SchoolYear> SchoolYear { get; set; }
		public NomenclatureDto<EducationalQualification> EducationalQualification { get; set; }
		public NomenclatureDto<EducationFormType> Form { get; set; }
		public NomenclatureDto<Institution> Faculty { get; set; }

		public double? Duration { get; set; }
		public List<NomenclatureDto<Language>> EducationSpecialityLanguages { get; set; } = new List<NomenclatureDto<Language>>();
		public string Specialization { get; set; }
		public string TraineeDuration { get; set; }

		public Education ToModel()
		{
			var educationSpecialityLanguages = new List<EducationSpecialityLanguage>();
			if (this.EducationSpecialityLanguages != null)
			{
				foreach (var specialityLanguage in this.EducationSpecialityLanguages)
				{
					var educationSpecialityLanguage = new EducationSpecialityLanguage { LanguageId = specialityLanguage.Id };
					educationSpecialityLanguages.Add(educationSpecialityLanguage);
				}
			}

			var education = new Education(this.Speciality?.Id, this.SchoolYear?.Id, this.EducationalQualification?.Id, this.Form?.Id, this.Duration, this.Faculty?.Id, educationSpecialityLanguages, this.Specialization, this.TraineeDuration);

			return education;
		}
	}
}
