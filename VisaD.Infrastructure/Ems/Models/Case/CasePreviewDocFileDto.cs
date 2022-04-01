using System;
using VisaD.Infrastructure.Ems.Enums;

namespace VisaD.Infrastructure.Ems.Models.Case
{
	public class CasePreviewDocFileDto
	{
		public Guid Key { get; set; }
		public int DbId { get; set; }
		public string Name { get; set; }
		public EmsDocFileType DocFileType { get; set; }

		public int? ElectronicDocumentId { get; set; }
	}
}
