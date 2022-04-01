using System.Collections.Generic;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.InstitutionSpecialities.Dtos
{
    public class InstitutionSpecialityDto
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public int SpecialityId { get; set; }
        public SpecialityDto Speciality { get; set; }
        public int? EducationalFormId { get; set; }
        public EducationFormType EducationalForm { get; set; }

        public double Duration { get; set; }
        public bool IsActive { get; set; }

        public virtual List<InstitutionSpecialityLanguage> OrganizationSpecialityLanguages { get; set; } = new List<InstitutionSpecialityLanguage>();
    }
}
