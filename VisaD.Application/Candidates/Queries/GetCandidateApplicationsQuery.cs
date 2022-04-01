using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Candidates.Queries
{
	public class GetCandidateApplicationsQuery : IRequest<IEnumerable<ApplicationSearchResultItemDto>>
	{
		public int CandidateLotId { get; set; }
		public int CandidateCommitId { get; set; }
		public CommitState CandidateCommitState { get; set; }

		public class Handler : IRequestHandler<GetCandidateApplicationsQuery, IEnumerable<ApplicationSearchResultItemDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<IEnumerable<ApplicationSearchResultItemDto>> Handle(GetCandidateApplicationsQuery request, CancellationToken cancellationToken)
			{
				return await context.Set<ApplicationCommit>()
					   .Where(e => (request.CandidateCommitState == CommitState.Actual ? e.CandidateCommit.LotId == request.CandidateLotId : e.CandidateCommitId == request.CandidateCommitId) &&
					   (e.State == CommitState.Actual
					   || e.State == CommitState.Modification
					   || e.State == CommitState.Deleted
					   || e.State == CommitState.Approved
					   || e.State == CommitState.Annulled
					   || e.State == CommitState.RefusedSign))
					   .Select(ApplicationSearchResultItemDto.SelectExpression)
					   .OrderByDescending(e => e.LotId)
						   .ThenBy(e => e.CommitId)
					   .ToListAsync(cancellationToken);
			}
		}
	}
}
