using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries
{
	public class GetApplicationLotHistoryQuery : IRequest<ApplicationLotHistoryDto>
	{
		public int LotId { get; set; }

		public class Handler : IRequestHandler<GetApplicationLotHistoryQuery, ApplicationLotHistoryDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<ApplicationLotHistoryDto> Handle(GetApplicationLotHistoryQuery request, CancellationToken cancellationToken)
			{
				var actualCommitId = await this.context.Set<ApplicationCommit>()
					.Where(x => x.LotId == request.LotId)
					.OrderByDescending(x => x.Id)
					.Select(x => x.Id)
					.FirstAsync(cancellationToken);

				var commits = await this.context.Set<ApplicationStatusHistory>()
					.Where(e => e.LotId == request.LotId)
					.ToListAsync(cancellationToken);

				var lotHistory = new ApplicationLotHistoryDto();

				foreach (var commit in commits)
				{
					var historyItem = new ApplicationCommitHistoryItemDto {
						CommitId = commit.CommitId,
						LotId = commit.LotId,
						State = commit.CommitState,
						CandidateBirthDate = commit.CandidateBirthDate,
						CandidateCountry = commit.CandidateCountry,
						CandidateName = commit.CandidateName,
						ChangeStateDescription = commit.ChangeStateDescription,
						CreateDate = commit.CreateDate,
						ApplicantName = commit.CreatorUser,
						RegisterNumber = commit.RegisterNumber,
						ApplicationLotResultType = commit.ApplicationLotResultType
					};

					lotHistory.Commits.Add(historyItem);
				}

				lotHistory.Commits = lotHistory.Commits.OrderByDescending(x => x.CreateDate).ToList();
				lotHistory.ActualCommitId = actualCommitId;

				return lotHistory;
			}
		}
	}
}
