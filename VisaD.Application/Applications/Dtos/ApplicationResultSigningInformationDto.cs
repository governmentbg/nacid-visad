using System.Collections.Generic;

namespace VisaD.Application.Applications.Dtos
{
    public class ApplicationResultSigningInformationDto
    {
        public string Content { get; set; }
        public IEnumerable<string> SignatureLineIds { get; set; } = new List<string>();
        public string Filename { get; set; }
    }
}
