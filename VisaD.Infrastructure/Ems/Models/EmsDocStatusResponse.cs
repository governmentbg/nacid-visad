using Newtonsoft.Json;
using VisaD.Infrastructure.Ems.Enums;

namespace VisaD.Infrastructure.Ems.Models
{
	public class EmsDocStatusResponse
	{
		public EmsIncomingDocStatus Status { get; set; }
		public string ReceiptElectronicDocument { get; set; }

		public EmsReceiptAcknowledgedDoc ReceiptAcknowledgedDoc =>
			JsonConvert.DeserializeObject<EmsReceiptAcknowledgedDoc>(this.ReceiptElectronicDocument);
	}
}
