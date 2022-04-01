using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Dtos
{
	public class LanguageProficiencyDto
	{
		public int Id { get; set; }
		public NomenclatureDto<Language> Language { get; set; }
		public NomenclatureDto<LanguageDegree> Reading { get; set; }
		public NomenclatureDto<LanguageDegree> Writing { get; set; }
		public NomenclatureDto<LanguageDegree> Speaking { get; set; }
	}
}
