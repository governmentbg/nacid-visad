using System;
using System.Collections.Generic;
using VisaD.Infrastructure.Ems.Common;

namespace VisaD.Infrastructure.Ems.Models.Case
{
	public class CasePreviewDocDto
	{
		public string RegUri { get; set; }
		public DateTime RegDate { get; set; }
		public NameObject DocType { get; set; }

		public IEnumerable<CasePreviewDocFileDto> DocFiles { get; set; } = new List<CasePreviewDocFileDto>();
	}
}
