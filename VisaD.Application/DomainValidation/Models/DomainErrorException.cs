using System;
using System.Collections.Generic;

namespace VisaD.Application.DomainValidation.Models
{
    public class DomainErrorException : Exception
    {
		private List<DomainErrorMessage> errorMessages = new List<DomainErrorMessage>();
		public List<DomainErrorMessage> ErrorMessages => errorMessages;

		public DomainErrorException(DomainErrorMessage errorMessage)
		{
			errorMessages.Add(errorMessage);
		}

		public DomainErrorException(List<DomainErrorMessage> errorMessages)
		{
			this.errorMessages = errorMessages;
		}

		public DomainErrorException(DomainErrorMessage errorMessage, string exceptionMessage)
			: base(exceptionMessage)
		{
			errorMessages.Add(errorMessage);
		}
	}
}
