using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Register.Commands
{
	public abstract class StartCommitModificationCommandHandler<TCommit> : IRequestHandler<StartCommitModificationCommand<TCommit>, CommitInfoDto>
		where TCommit : Commit
	{
		protected readonly IAppDbContext context;

		public StartCommitModificationCommandHandler(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<CommitInfoDto> Handle(StartCommitModificationCommand<TCommit> request, CancellationToken cancellationToken)
		{
			var actualCommit = await LoadRelatedData(context.Set<TCommit>())
				.SingleAsync(e => e.LotId == request.LotId && (e.State == CommitState.Actual || e.State == CommitState.RefusedSign), cancellationToken);
			actualCommit.State = CommitState.ActualWithModification;

			var modificationCommit = (TCommit)Activator.CreateInstance(typeof(TCommit), actualCommit);
			modificationCommit.State = CommitState.Modification;
			modificationCommit.ChangeStateDescription = request.ChangeStateDescription;
			context.Set<TCommit>().Add(modificationCommit);

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = modificationCommit.LotId,
				CommitId = modificationCommit.Id
			};
		}

		protected abstract IQueryable<TCommit> LoadRelatedData(IQueryable<TCommit> query);
	}
}
