using System.Collections.Generic;

namespace VisaD.Application.Candidates.Dtos
{
	public class CandidateLotHistoryDto
	{
		public IEnumerable<CandidateCommitHistoryItemDto> Commits { get; set; } = new List<CandidateCommitHistoryItemDto>();

		public int ActualCommitId { get; set; }
	}
}
