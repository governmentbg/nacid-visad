using System.Collections.Generic;

namespace VisaD.Infrastructure.Ems.Models.Case
{
	public class CasePreviewDto
	{
		public CasePreviewDocDto Case { get; set; }

		public IEnumerable<CasePreviewStageDto> Stages { get; set; } = new List<CasePreviewStageDto>();
		public IEnumerable<CasePreviewDocDto> Docs { get; set; } = new List<CasePreviewDocDto>();
	}
}
