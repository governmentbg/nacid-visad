using MediatR;
using VisaD.Application.Common.Dtos;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Register.Commands
{
	public class CommitReadyModificationCommand<TCommit> : IRequest<CommitInfoDto>
		where TCommit : Commit
	{
		public int LotId { get; set; }
	}
}
