using System.Collections.Generic;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicationLotHistoryDto
	{
		public List<ApplicationCommitHistoryItemDto> Commits { get; set; } = new List<ApplicationCommitHistoryItemDto>();

		public int ActualCommitId { get; set; }
	}
}
