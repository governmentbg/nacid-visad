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
	public class StartPartModificationCommandHandler<TPart, TEntity> : IRequestHandler<StartPartModificationCommand<TPart, TEntity>, int>
			where TPart : Part<TEntity>
			where TEntity : class, IEntity
	{
		protected readonly IAppDbContext context;

		public StartPartModificationCommandHandler(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<int> Handle(StartPartModificationCommand<TPart, TEntity> request, CancellationToken cancellationToken)
		{
			var part = await LoadPart()
				.SingleAsync(e => e.Id == request.Id, cancellationToken);

			var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), part.Entity);
			context.Set<TEntity>().Add(entity);
			await context.SaveChangesAsync(cancellationToken);

			part.Entity = entity;
			part.EntityId = entity.Id;
			part.State = PartState.Modified;
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
