using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Register.Commands
{
	public class RevertErasedCommitCommandHandler<TCommit> : IRequestHandler<RevertErasedCommitCommand<TCommit>, CommitInfoDto>
			where TCommit : Commit
	{
		private readonly IAppDbContext context;

		public RevertErasedCommitCommandHandler(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<CommitInfoDto> Handle(RevertErasedCommitCommand<TCommit> request, CancellationToken cancellationToken)
		{
			var actualCommit = await context.Set<TCommit>()
				.SingleAsync(e => e.LotId == request.LotId && e.State == CommitState.Deleted, cancellationToken);
			actualCommit.State = CommitState.Actual;
			actualCommit.ChangeStateDescription = null;

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = actualCommit.LotId,
				CommitId = actualCommit.Id
			};
		}
	}
}
