using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.AttachedFiles;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries.Parts
{
    public class GetRepresentativePartQuery : IRequest<PartDto<RepresentativeDto>>
    {
        public int PartId { get; set; }
        
        public class Handler : IRequestHandler<GetRepresentativePartQuery, PartDto<RepresentativeDto>>
        {
            private readonly IAppDbContext context;

            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<PartDto<RepresentativeDto>> Handle(GetRepresentativePartQuery request, CancellationToken cancellationToken)
            {
                var result = await context.Set<RepresentativePart>()
                    .AsNoTracking()
                    .Select(e => new PartDto<RepresentativeDto> {
                        Id = e.Id,
                        Entity = new RepresentativeDto {
                            HasRepresentative = e.Entity.HasRepresentative,
                            Type = e.Entity.Type.Value,
                            FirstName = e.Entity.FirstName,
                            LastName = e.Entity.LastName,
                            IdentificationCode = e.Entity.IdentificationCode,
                            Mail = e.Entity.Mail,
                            Phone = e.Entity.Phone,
                            Note = e.Entity.Note,
                            SubmissionDate = e.Entity.SubmissionDate,
                            ApplicationForCertificate = e.Entity.RepresentativeDocumentFiles.SingleOrDefault(x => x.Type == RepresentativeDocumentType.ApplicationForCertificate),
                            LetterOfAttorney = e.Entity.RepresentativeDocumentFiles.SingleOrDefault(x => x.Type == RepresentativeDocumentType.LetterOfAttorney)
                        },
                        State = e.State
                    })
                    .SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

                return result;
            }
        }
    }
}
