using System;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications.Register
{
	public class ApplicationStatusHistory : IEntity
	{
		public int Id { get; set; }

		public string CreatorUser { get; set; }

		public DateTime CreateDate { get; set; }

		public CommitState CommitState { get; set; }

		public string ChangeStateDescription { get; set; }

		public int LotId { get; set; }

		public int? CommitId { get; set; }

		public string CandidateName { get; set; }

		public string RegisterNumber { get; set; }

		public DateTime CandidateBirthDate { get; set; }

		public string CandidateCountry { get; set; }

		public ApplicationLotResultType? ApplicationLotResultType { get; set; }

		public ApplicationStatusHistory(int lotId, int? commitId, string creatorUser, DateTime createDate, CommitState commitState, string changeStateDescription,
			string candidateName, string registerNumber, DateTime candidateBirthDate, string candidateCountry, ApplicationLotResultType? applicationLotResultType)
		{
			this.LotId = lotId;
			this.CommitId = commitId;
			this.CreatorUser = creatorUser;
			this.CreateDate = createDate;
			this.CommitState = commitState;
			this.ChangeStateDescription = changeStateDescription;
			this.CandidateName = candidateName;
			this.RegisterNumber = registerNumber;
			this.CandidateBirthDate = candidateBirthDate;
			this.CandidateCountry = candidateCountry;
			this.ApplicationLotResultType = applicationLotResultType;
		}
	}
}
