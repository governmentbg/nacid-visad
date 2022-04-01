using System.Collections.Generic;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Common.Models
{
	public abstract class Lot<TCommit> : IEntity
		where TCommit : Commit
	{
		public int Id { get; set; }
		public int LotNumber { get; set; }

		public string RegisterNumber { get; set; }

		public ICollection<TCommit> Commits { get; set; } = new List<TCommit>();
	}
}
