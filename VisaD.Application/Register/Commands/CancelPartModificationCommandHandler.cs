using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Register.Commands
{
	public class CancelPartModificationCommandHandler<TCommit, TPart, TEntity> : IRequestHandler<CancelPartModificationCommand<TCommit, TPart, TEntity>, int>
			where TCommit : Commit
			where TPart : Part<TEntity>
			where TEntity : class, IEntity
	{
		protected readonly IAppDbContext context;

		public CancelPartModificationCommandHandler(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<int> Handle(CancelPartModificationCommand<TCommit, TPart, TEntity> request, CancellationToken cancellationToken)
		{
			var part = await LoadPart()
				.SingleAsync(e => e.Id == request.Id, cancellationToken);

			var currentLotIdQuery = context.Set<TCommit>()
				.Where(e => e.Id == part.Id)
				.Select(e => e.LotId);

			var previousCommitIdQuery = context.Set<TCommit>()
				.Where(e => currentLotIdQuery.Contains(e.LotId) && e.State == CommitState.ActualWithModification)
				.Select(e => e.Id);

			var previousPart = await context.Set<TPart>()
				.Include(e => e.Entity)
				.AsNoTracking()
				.Where(e => previousCommitIdQuery.Contains(e.Id))
				.SingleAsync(cancellationToken);

			var currentEntity = part.Entity;

			part.Entity = previousPart.Entity;
			part.EntityId = previousPart.EntityId;
			part.State = PartState.Unchanged;

			context.Set<TEntity>().Remove(currentEntity);

			await context.SaveChangesAsync(cancellationToken);

			return part.Id;
		}

		protected virtual IQueryable<TPart> LoadPart()
		{
			return context.Set<TPart>()
				.Include(e => e.Entity);
		}
	}
}
