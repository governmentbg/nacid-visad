using System;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Common.Models;

namespace VisaD.Data.Candidates.Register
{
	public class CandidateLot : Lot<CandidateCommit>, IAuditable
	{
		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }
	}
}
