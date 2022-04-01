using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries.Parts
{
    public class GetPreviousApplicationPartQuery : IRequest<PartDto<PreviousApplicationDto>>
    {
        public int PartId { get; set; }

        public class Handler : IRequestHandler<GetPreviousApplicationPartQuery, PartDto<PreviousApplicationDto>>
        {
            private readonly IAppDbContext context;

            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<PartDto<PreviousApplicationDto>> Handle(GetPreviousApplicationPartQuery request, CancellationToken cancellationToken)
            {
                var result = await this.context.Set<PreviousApplicationPart>()
                    .AsNoTracking()
                    .Select(e => new PartDto<PreviousApplicationDto> {
                        Id = e.Id,
                        Entity = new PreviousApplicationDto {
                            HasPreviousApplication = e.Entity.HasPreviousApplication,
                            PreviousApplicationRegisterNumber = e.Entity.PreviousApplicationRegisterNumber,
                            PreviousApplicationYear = e.Entity.PreviousApplicationYear,
                            PreviousApplicationLotId = e.Entity.PreviousApplicationLotId,
                            PreviousApplicationCommitId = e.Entity.PreviousApplicationCommitId
                        },
                        State = e.State
                    })
                    .SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

                return result;
            }
        }
    }
}
