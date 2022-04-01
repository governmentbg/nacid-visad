using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class TaxAccount : IEntity, IAuditable, IConcurrency
	{
		public int Id { get; set; }

		private HashSet<Tax> _taxes = new HashSet<Tax>();
		public IReadOnlyCollection<Tax> Taxes => _taxes.ToList().AsReadOnly();

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		public int Version { get; set; }

	

		public TaxAccount()
		{

		}

		public TaxAccount(TaxAccount taxAccount)
			: this()
		{
			foreach(var item in taxAccount.Taxes)
			{
				this.AddTax(item.Iban, item.AccountHolder, item.Amount, item.CurrencyTypeId, item.AdditionalInfo, item.Type, item.Bank, item.Bic);
			}
		}

		public Tax AddTax(string iban, string accountHolder, decimal? amount, int? currencyTypeId, string additionalInfo, TaxType type, string bank, string bic)
		{
			var tax = new Tax(iban, accountHolder, amount, currencyTypeId, additionalInfo, type, bank, bic);
			this._taxes.Add(tax);

			return tax;
		}

		public Tax UpdateTax(string iban, string accountHolder, decimal? amount, int? currencyTypeId, string additionalInfo, TaxType type, int id, string bank, string bic)
		{
			var tax = this._taxes.Single(e => e.Id == id);
			tax.Update(iban, accountHolder, amount, currencyTypeId, additionalInfo, type, bank, bic);

			return tax;
		}

		public void RemoveTax(int id)
		{
			var tax = this._taxes.Single(e => e.Id == id);
			this._taxes.Remove(tax);
		}

	}
}
