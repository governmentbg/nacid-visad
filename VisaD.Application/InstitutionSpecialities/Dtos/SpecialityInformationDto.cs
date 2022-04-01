using System.Collections.Generic;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.InstitutionSpecialities.Dtos
{
    public class SpecialityInformationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Duration { get; set; }
        public EducationFormType Form { get; set; }
        public EducationalQualification Qualification { get; set; }
        public HashSet<Language> SpecialityLanguage { get; set; } = new HashSet<Language>();
        public Institution Institution { get; set; }

        public SpecialityInformationDto(int id, string name, double duration, EducationFormType form, EducationalQualification qualification, List<InstitutionSpecialityLanguage> institutionSpecialityLanguages, Institution institution)
        {
            this.Id = id;
            this.Name = name;
            this.Duration = duration;
            this.Form = form;
            this.Qualification = qualification;
            this.Institution = institution;
			foreach (var institutionSpecialityLanguage in institutionSpecialityLanguages)
			{
                var specialityLanguage = new Language {
                    Id = institutionSpecialityLanguage.LanguageId.Value,
                    Name = institutionSpecialityLanguage.Language.Name
                };

                this.SpecialityLanguage.Add(specialityLanguage);
			}
        }
    }
}
