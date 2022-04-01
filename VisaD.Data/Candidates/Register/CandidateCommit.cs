using VisaD.Data.Common.Models;

namespace VisaD.Data.Candidates.Register
{
	public class CandidateCommit : Commit
	{
		public CandidateLot Lot { get; set; }

		public CandidatePart CandidatePart { get; set; }

		public CandidateCommit()
			: base()
		{

		}

		public CandidateCommit(CandidateCommit commit)
			: base(commit)
		{
			this.CandidatePart = new CandidatePart(commit.CandidatePart);
		}
	}
}
