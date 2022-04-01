using System;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Common.Models
{
	public abstract class Commit : IEntity, IAuditable, IConcurrency
	{
		public int Id { get; set; }

		public int LotId { get; set; }

		public CommitState State { get; set; }

		public int? Number { get; set; }

		public DateTime CreateDate { get; set; }	
		public int CreatorUserId { get; set; }

		public int Version { get; set; }

		public string ChangeStateDescription { get; set; }

		public Commit()
		{
			this.Number = 1;
			this.State = CommitState.InitialDraft;
		}

		public Commit(Commit commit)
		{
			this.LotId = commit.LotId;
			this.State = commit.State;
			this.Number = commit.Number;
		}
	}
}
