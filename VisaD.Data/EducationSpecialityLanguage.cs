using VisaD.Data.Applications;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data
{
	public class EducationSpecialityLanguage : IEntity
	{
		public int Id { get; set; }

		public int EducationId { get; set; }
		public Education Education { get; set; }

		public int LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
