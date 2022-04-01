using VisaD.Data.Common.Enums;

namespace VisaD.Application.Common.Dtos
{
	public abstract class CommitDto
	{
		public int Id { get; set; }
		public int LotId { get; set; }
		public CommitState State { get; set; }
	}
}
