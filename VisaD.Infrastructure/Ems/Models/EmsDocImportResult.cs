using System.Collections.Generic;
using VisaD.Infrastructure.Ems.Enums;

namespace VisaD.Infrastructure.Ems.Models
{
	public class EmsDocImportResult
    {
        public EmsDocImportStatus Status { get; set; } = EmsDocImportStatus.HasTechnicalIssue;
        public IList<string> Errors { get; set; } = new List<string>();
    }
}
