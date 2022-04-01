using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Candidates.Commands
{
	public class DeleteCandidateLotCommand : IRequest<Unit>
	{
		public int LotId { get; set; }

		public class Handler : IRequestHandler<DeleteCandidateLotCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(DeleteCandidateLotCommand request, CancellationToken cancellationToken)
			{
				var lot = await context.Set<CandidateLot>()
					.Include(e => e.Commits)
						.ThenInclude(e => e.CandidatePart)
							.ThenInclude(c => c.Entity)
					.SingleAsync(e => e.Id == request.LotId, cancellationToken);

				if (lot.Commits.Any(c => c.State != CommitState.InitialDraft))
				{
					// TODO: Add more specific error
					throw new ArgumentException("Cannot delete lot with commits not in initial draft!");
				}

				context.Set<CandidateCommit>().RemoveRange(lot.Commits);
				context.Set<Candidate>().RemoveRange(lot.Commits.Select(c => c.CandidatePart.Entity));
				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}

	}
}
