using VisaD.Data.Common.Models;

namespace VisaD.Data.Nomenclatures
{
	public class Bank : Nomenclature
	{
		public string IBAN_CODE { get; set; }

		public string BIC { get; set; }
	}
}
