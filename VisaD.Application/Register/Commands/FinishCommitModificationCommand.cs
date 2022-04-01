using MediatR;
using VisaD.Application.Common.Dtos;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Register.Commands
{
	public class FinishCommitModificationCommand<TLot, TCommit> : IRequest<CommitInfoDto>
		where TLot: Lot<TCommit>
		where TCommit : Commit
	{
		public int LotId { get; set; }
		public bool ShouldRegisterLot { get; set; }
		public string RegisterIndexAlias { get; set; }
	}
}
