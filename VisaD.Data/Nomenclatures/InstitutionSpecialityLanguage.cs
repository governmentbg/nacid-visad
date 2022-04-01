using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Nomenclatures
{
	public class InstitutionSpecialityLanguage : IEntity
	{
		public int Id { get; set; }

		public int InstitutionSpecialityId { get; set; }
		public virtual InstitutionSpeciality InstitutionSpeciality { get; set; }

		public int? LanguageId { get; set; }
		public virtual Language Language { get; set; }

		public int Version { get; set; }
	}
}
