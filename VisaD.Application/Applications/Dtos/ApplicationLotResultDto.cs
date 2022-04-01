using VisaD.Data.Applications.Enums;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicationLotResultDto
	{
		public int Id { get; set; }
		public ApplicationLotResultType Type { get; set; }
		public string AttachedFilePath { get; set; }
		public string Note { get; set; }
		public string CertificateNumber { get; set; }
		public string AccessCode { get; set; }
		public bool IsSigned { get; set; }
	}
}
