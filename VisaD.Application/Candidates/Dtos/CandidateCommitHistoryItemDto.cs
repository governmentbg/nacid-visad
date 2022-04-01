using System;
using VisaD.Application.Candidates.Dtos;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Candidates.Dtos
{
	public class CandidateCommitHistoryItemDto
	{
		public int Id { get; set; }
		public int LotId { get; set; }

		public CommitState State { get; set; }

		public string CandidateName { get; set; }

		public int? Number { get; set; }
		public DateTime CreateDate { get; set; }
		public string Country { get; set; }
		public DateTime BirthDate { get; set; }
		public string CandidateCyrillicName { get; set; }
		public string Institution { get; set; }
		public string Mail { get; set; }
		public CreatorUserDto CreatorUser { get; set; }
		public string ChangeStateDescription { get; set; }
	}
}
