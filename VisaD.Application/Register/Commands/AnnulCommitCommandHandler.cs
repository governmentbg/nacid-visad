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
	public class AnnulCommitCommandHandler<TCommit> : IRequestHandler<AnnulCommitCommand<TCommit>, CommitInfoDto>
		where TCommit : Commit
	{
		protected readonly IAppDbContext context;

		public AnnulCommitCommandHandler(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<CommitInfoDto> Handle(AnnulCommitCommand<TCommit> request, CancellationToken cancellationToken)
		{
			var actualCommit = await context.Set<TCommit>()
				.SingleAsync(e => e.LotId == request.LotId && e.State == CommitState.Approved, cancellationToken);
			actualCommit.State = CommitState.Annulled;
			actualCommit.ChangeStateDescription = request.ChangeStateDescription;

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = actualCommit.LotId,
				CommitId = actualCommit.Id
			};
		}
	}
}
