using Newtonsoft.Json.Linq;

namespace VisaD.Infrastructure.Ems.Models
{
	public class EmsStructuredDataWithInformation
	{
		public string DocRegisterNumber { get; set; }
		public JObject StructuredData { get; set; }
	}
}
