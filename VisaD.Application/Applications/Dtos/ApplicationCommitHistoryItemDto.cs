using System;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicationCommitHistoryItemDto
	{
		public int? CommitId { get; set; }
		public int LotId { get; set; }
		
		public CommitState State { get; set; }

		public string CandidateName { get; set; }

		public DateTime CreateDate { get; set; }

		public string ApplicantName { get; set; }

		public string ChangeStateDescription { get; set; }

		public string RegisterNumber { get; set; }
		
		public DateTime CandidateBirthDate { get; set; }

		public string CandidateCountry { get; set; }

		public ApplicationLotResultType? ApplicationLotResultType { get; set; }
	}
}
