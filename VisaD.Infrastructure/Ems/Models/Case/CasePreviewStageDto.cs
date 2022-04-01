using System;
using VisaD.Infrastructure.Ems.Common;

namespace VisaD.Infrastructure.Ems.Models.Case
{
	public class CasePreviewStageDto
	{
		public NameObject ElectronicServiceStage { get; set; }
		public NameObject ExecutorUnit { get; set; }
		public DateTime? EndingDate { get; set; }
	}
}
