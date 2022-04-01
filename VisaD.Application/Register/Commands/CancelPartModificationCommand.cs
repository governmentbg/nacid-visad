using MediatR;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Register.Commands
{
	public class CancelPartModificationCommand<TCommit, TPart, TEntity> : IRequest<int>
			where TPart : Part<TEntity>
			where TEntity : IEntity
	{
		public int Id { get; set; }
	}
}
