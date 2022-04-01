using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Dtos
{
	public class TaxDto
	{
		public int Id { get; set; }
		
		public TaxType Type { get; set; }
		public string Iban { get; set; }
		public string AccountHolder { get; set; }
		
		public decimal? Amount { get; set; }
		public NomenclatureDto<CurrencyType> CurrencyType { get; set; }

		public string AdditionalInfo { get; set; }
		
		public string Bank { get; set; }
		public string Bic { get; set; }
	}
}
