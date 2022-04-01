using System.Collections.Generic;
using System.Numerics;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;

namespace VisaD.Application.Nomenclatures.Services
{
	public class BankService : IBankService
	{
		private readonly DomainValidationService validation;
		private readonly Dictionary<char, int> englishLetters = new Dictionary<char, int> 
		{
			{'A', 10 },
			{'B', 11 },
			{'C', 12 },
			{'D', 13 },
			{'E', 14 },
			{'F', 15 },
			{'G', 16 },
			{'H', 17 },
			{'I', 18 },
			{'J', 19 },
			{'K', 20 },
			{'L', 21 },
			{'M', 22 },
			{'N', 23 },
			{'O', 24 },
			{'P', 25 },
			{'Q', 26 },
			{'R', 27 },
			{'S', 28 },
			{'T', 29 },
			{'U', 30 },
			{'V', 31 },
			{'W', 32 },
			{'X', 33 },
			{'Y', 34 },
			{'Z', 35 },
		};

		public BankService(DomainValidationService validation)
		{
			this.validation = validation;
		}

		public void ValidateIban(string iban)
		{
			var countryCode = iban.Substring(0, 4);
			
			iban = iban.Remove(0, 4);
			iban += countryCode;

			for (int i = 0; i < iban.Length; i++)
			{
				if (char.IsLetter(iban[i]))
				{
					var letterNumber = this.englishLetters[iban[i]];
					var letterIndex = iban.IndexOf(iban[i]);
					iban = iban.Remove(letterIndex, 1);
					iban = iban.Insert(letterIndex, letterNumber.ToString());
				}
			}

			BigInteger ibanNumber = BigInteger.Parse(iban);

			if (ibanNumber % 97 != 1)
			{
				this.validation.ThrowErrorMessage(ApplicationErrorCode.Application_InvalidIBAN);
			}
		}
	}
}
