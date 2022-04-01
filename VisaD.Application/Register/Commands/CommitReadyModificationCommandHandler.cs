using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Register.Commands
{
	public class CommitReadyModificationCommandHandler<TCommit> : IRequestHandler<CommitReadyModificationCommand<TCommit>, CommitInfoDto>
		where TCommit : Commit
	{
		protected readonly IAppDbContext context;

		public CommitReadyModificationCommandHandler(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<CommitInfoDto> Handle(CommitReadyModificationCommand<TCommit> request, CancellationToken cancellationToken)
		{
			var states = new List<CommitState> { CommitState.Modification, CommitState.InitialDraft, CommitState.CommitReady };
			var modificationCommit = await context.Set<TCommit>()
				.SingleAsync(e => e.LotId == request.LotId && states.Contains(e.State), cancellationToken);
			modificationCommit.State = CommitState.CommitReady;
			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = modificationCommit.LotId,
				CommitId = modificationCommit.Id
			};
		}
	}
}
