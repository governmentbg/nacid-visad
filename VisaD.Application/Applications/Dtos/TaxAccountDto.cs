using System.Collections.Generic;
using VisaD.Data.Applications;

namespace VisaD.Application.Applications.Dtos
{
	public class TaxAccountDto
	{
		public IEnumerable<TaxDto> Taxes { get; set; } = new List<TaxDto>();

		public TaxAccount ToModel()
		{
			var taxAccount = new TaxAccount();
			foreach (var item in this.Taxes)
			{
				taxAccount.AddTax(item.Iban, item.AccountHolder, item.Amount, item.CurrencyType?.Id, item.AdditionalInfo, item.Type, item.Bank, item.Bic);
			}

			return taxAccount;
		}
	}
}
