using VisaD.Application.Applications.Commands;

namespace VisaD.Application.Ems.Models
{
    public class EmsApplicationDto
    {
        public CreateApplicationCommand StructuredData { get; set; }
        public byte[] Signature { get; set; }
    }
}
