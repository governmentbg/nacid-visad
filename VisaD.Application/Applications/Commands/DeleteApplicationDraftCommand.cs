using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands
{
	public class DeleteApplicationDraftCommand : IRequest<Unit>
	{
		public int DraftId { get; set; }

		public class Handler : IRequestHandler<DeleteApplicationDraftCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}
			public async Task<Unit> Handle(DeleteApplicationDraftCommand request, CancellationToken cancellationToken)
			{
				var draft = await this.context.Set<ApplicationDraft>()
					.SingleOrDefaultAsync(e => e.Id == request.DraftId, cancellationToken);

				this.context.Set<ApplicationDraft>().Remove(draft);

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
