using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates.Register;

namespace VisaD.Application.Candidates.Queries
{
    public class GetCandidateCommitCountQuery : IRequest<bool>
    {
        public int LotId { get; set; }

        public class Handler : IRequestHandler<GetCandidateCommitCountQuery, bool>
        {
            private readonly IAppDbContext context;

            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(GetCandidateCommitCountQuery request, CancellationToken cancellationToken)
            {
                var commitsCount = await context.Set<CandidateCommit>()
                    .AsNoTracking()
                    .CountAsync(e => e.LotId == request.LotId);

                return commitsCount > 1;
            }
        }
    }
}
