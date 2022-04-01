using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdatePreviousApplicationCommand : IRequest<Unit>
	{
		public PreviousApplicationDto Model { get; set; }
		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdatePreviousApplicationCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdatePreviousApplicationCommand request, CancellationToken cancellationToken)
			{
				var part = await this.context.Set<PreviousApplicationPart>()
					.Include(e => e.Entity)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				part.Entity.UpdateFile(request.Model.PreviousApplicationRegisterNumber, request.Model.PreviousApplicationYear, request.Model.PreviousApplicationLotId, request.Model.PreviousApplicationCommitId);
				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
