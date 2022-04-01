using System;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications
{
	public class Tax : IEntity, IAuditable
	{
		public int Id { get; private set; }

		public TaxType Type { get; private set; }
		public string Iban { get; private set; }
		public string AccountHolder { get; private set; }
		public decimal? Amount { get; private set; }
		public int? CurrencyTypeId { get; private set; }
		public CurrencyType CurrencyType { get; private set; }
		public string AdditionalInfo { get; private set; }

		public int? TaxAccountId { get; private set; }
		public TaxAccount TaxAccount { get; private set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		public string Bank { get; private set; }
		public string Bic { get; private set; }

		private Tax()
		{

		}

		public Tax(string iban, string accountHolder, decimal? amount, int? currencyTypeId, string additionalInfo, TaxType type, string bank, string bic)
		{
			this.Type = type;
			this.Iban = iban;
			this.AccountHolder = accountHolder;
			this.Amount = amount;
			this.CurrencyTypeId = currencyTypeId;
			this.AdditionalInfo = additionalInfo;
			this.Bank = bank;
			this.Bic = bic;
		}

		public Tax(Tax tax)
			: this(tax.Iban, tax.AccountHolder, tax.Amount.Value, tax.CurrencyTypeId, tax.AdditionalInfo, tax.Type, tax.Bank, tax.Bic)
        {

        }

		public void Update(string iban, string accountHolder, decimal? amount, int? currencyTypeId, string additionalInfo, TaxType type, string bank, string bic)
		{
			this.Type = type;
			this.Iban = iban;
			this.AccountHolder = accountHolder;
			this.Amount = amount;
			this.CurrencyTypeId = currencyTypeId;
			this.AdditionalInfo = additionalInfo;
			this.Bank = bank;
			this.Bic = bic;
		}
	}
}
