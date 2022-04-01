using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Candidates.Commands
{
	public class CancelCandidateModificationCommand : IRequest<CommitInfoDto>
	{
		public int LotId { get; set; }

		public class Handler : IRequestHandler<CancelCandidateModificationCommand, CommitInfoDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<CommitInfoDto> Handle(CancelCandidateModificationCommand request, CancellationToken cancellationToken)
			{
				var modificationCommit = await context.Set<CandidateCommit>()
					.Include(e => e.CandidatePart)
							.ThenInclude(a => a.Entity)
					.SingleAsync(e => e.LotId == request.LotId && e.State == CommitState.Modification, cancellationToken);

				context.Set<CandidateCommit>().Remove(modificationCommit);

				if (modificationCommit.CandidatePart.State == PartState.Modified)
				{
					context.Set<Candidate>().Remove(modificationCommit.CandidatePart.Entity);
				}
				context.Set<CandidatePart>().Remove(modificationCommit.CandidatePart);

				var actualWithModificationCommit = await context.Set<CandidateCommit>()
					.SingleAsync(e => e.LotId == request.LotId && e.State == CommitState.ActualWithModification, cancellationToken);
				actualWithModificationCommit.State = CommitState.Actual;

				await context.SaveChangesAsync(cancellationToken);

				return new CommitInfoDto {
					LotId = actualWithModificationCommit.LotId,
					CommitId = actualWithModificationCommit.Id
				};
			}
		}
	}
}
