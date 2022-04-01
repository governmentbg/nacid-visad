using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Candidates.Commands
{
	public class CreateCandidateCommand : IRequest<CommitInfoDto>
	{
		public CandidateDto Candidate { get; set; }

		public class Handler : IRequestHandler<CreateCandidateCommand, CommitInfoDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<CommitInfoDto> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
			{
				int? lastLotNumber = await context.Set<CandidateLot>()
					.MaxAsync(e => (int?)e.LotNumber, cancellationToken);
				var lot = new CandidateLot {
					LotNumber = (lastLotNumber ?? 0) + 1
				};
				context.Set<CandidateLot>().Add(lot);
				await context.SaveChangesAsync(cancellationToken);

				var commit = new CandidateCommit {
					LotId = lot.Id,
					State = CommitState.Actual,
					Number = 1,
					CandidatePart = new CandidatePart {
						Entity = request.Candidate?.ToModel()
					}
				};

				context.Set<CandidateCommit>().Add(commit);
				await context.SaveChangesAsync(cancellationToken);

				return new CommitInfoDto {
					LotId = lot.Id,
					CommitId = commit.Id
				};
			}
		}
	}
}
