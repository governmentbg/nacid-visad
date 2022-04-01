using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries
{
    public class GetApplicationCommitCountQuery : IRequest<bool>
    {
        public int LotId { get; set; }

        public class Handler : IRequestHandler<GetApplicationCommitCountQuery, bool>
        {
            private readonly IAppDbContext context;

            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(GetApplicationCommitCountQuery request, CancellationToken cancellationToken)
            {
                var commitsCount = await context.Set<ApplicationCommit>()
                    .AsNoTracking()
                    .CountAsync(e => e.LotId == request.LotId);

                return commitsCount > 1;
            }
        }
    }
}
