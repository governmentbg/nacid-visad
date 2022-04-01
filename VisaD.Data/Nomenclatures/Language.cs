using System.Collections.Generic;
using VisaD.Data.Common.Models;

namespace VisaD.Data.Nomenclatures
{
    public class Language : Nomenclature
    {
        public bool? IsMandatory { get; set; }

        public string NameAlt { get; set; }

        public string Code { get; set; }

        public int Version { get; set; }

        public virtual ICollection<InstitutionSpecialityLanguage> InstitutionSpecialityLanguage { get; set; } = new HashSet<InstitutionSpecialityLanguage>();
    }
}
