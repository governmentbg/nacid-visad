using System;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class PreviousApplication : IEntity, IAuditable
	{
		public int Id { get; set; }

		public bool HasPreviousApplication { get; set; }

		public string PreviousApplicationRegisterNumber { get; private set; }

		public int? PreviousApplicationYear { get; private set; }

		public int? PreviousApplicationLotId { get; private set; }

		public int? PreviousApplicationCommitId { get; private set; }

		public DateTime CreateDate { get; set; }

		public int CreatorUserId { get; set; }

		public PreviousApplication(
			bool hasPreviousApplication, 
			string previousApplicationRegisterNumber, 
			int? previousApplicationYear, 
			int? previousApplicationLotId, 
			int? previousApplicationCommitId
			)
		{
			this.HasPreviousApplication = hasPreviousApplication;
			this.PreviousApplicationRegisterNumber = previousApplicationRegisterNumber;
			this.PreviousApplicationYear = previousApplicationYear;
			this.PreviousApplicationLotId = previousApplicationLotId;
			this.PreviousApplicationCommitId = previousApplicationCommitId;
		}

		public PreviousApplication(PreviousApplication previousApplication)
			: this(previousApplication.HasPreviousApplication, previousApplication.PreviousApplicationRegisterNumber, previousApplication.PreviousApplicationYear, previousApplication.PreviousApplicationLotId, previousApplication.PreviousApplicationCommitId)
		{
		}

		public void UpdateFile(string previousApplicationRegisterNumber, int? previousApplicationYear, int? previousApplicationLotId, int? previousApplicationCommitId)
		{
			this.PreviousApplicationRegisterNumber = previousApplicationRegisterNumber;
			this.PreviousApplicationYear = previousApplicationYear;
			this.PreviousApplicationLotId = previousApplicationLotId;
			this.PreviousApplicationCommitId = previousApplicationCommitId;
		}
	}
}
