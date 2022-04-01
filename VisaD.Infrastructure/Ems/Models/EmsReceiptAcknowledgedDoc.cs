using System.Collections.Generic;

namespace VisaD.Infrastructure.Ems.Models
{
	public class EmsReceiptAcknowledgedDoc
	{
		public string CaseNumber { get; set; }
		public string CaseAccessCode { get; set; }

		public string RegisteredDocNumber { get; set; }

		public string ElectronicServiceProviderName { get; set; }

		public IList<string> Discrepancies { get; set; }
	}
}
