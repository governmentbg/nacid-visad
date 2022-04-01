using System.Collections.Generic;
using VisaD.Data.Applications.Enums;

namespace VisaD.Application.Applications.Dtos
{
	public class ReportDto
	{
		public List<ReportItemDto> Reports = new List<ReportItemDto>();

		public ReportType ReportType { get; set; }
	}
}
