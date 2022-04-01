using System;
using System.Collections.Generic;
using VisaD.Application.DomainValidation.Models;

namespace VisaD.Application.DomainValidation
{
    public class DomainValidationService
    {
		private List<DomainErrorMessage> errorMessages = new List<DomainErrorMessage>();

		public DomainValidationService()
		{
		}

		public void AddErrorMessage(Enum errorCode)
		{
			errorMessages.Add(new DomainErrorMessage(errorCode));
		}

		public void ThrowErrorMessage(Enum errorCode)
		{
			throw new DomainErrorException(new DomainErrorMessage(errorCode));
		}

		public void ThrowErrorMessage(DomainErrorMessage errorMessage)
		{
			throw new DomainErrorException(errorMessage);
		}

		public void ThrowErrorMessage(Enum errorCode, string exceptionMessage)
		{
			throw new DomainErrorException(new DomainErrorMessage(errorCode), exceptionMessage);
		}

		public void AddErrorMessage(Enum errorCode, params object[] errorParameters)
		{
			throw new NotImplementedException();
		}

		public void Validate()
		{
			if (errorMessages.Count > 0)
			{
				throw new DomainErrorException(errorMessages);
			}
		}
	}
}
