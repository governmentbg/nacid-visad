using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
    public class UpdateRepresentativeCommand : IRequest<Unit>
    {
        public RepresentativeDto Model { get; set; }
        public int PartId { get; set; }

        public class Handler : IRequestHandler<UpdateRepresentativeCommand, Unit>
        {
            private readonly IAppDbContext context;

            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(UpdateRepresentativeCommand request, CancellationToken cancellationToken)
            {
                var part = await context.Set<RepresentativePart>()
                    .Include(e => e.Entity)
                        .ThenInclude(e => e.RepresentativeDocumentFiles)
                    .SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

                if (request.Model.LetterOfAttorney != null && !part.Entity.RepresentativeDocumentFiles.Any(x => x.Type == RepresentativeDocumentType.LetterOfAttorney))
				{
                    request.Model.LetterOfAttorney.Type = RepresentativeDocumentType.LetterOfAttorney;
                    part.Entity.AddFile(request.Model.LetterOfAttorney);
				} 
                else if (request.Model.LetterOfAttorney != null && part.Entity.RepresentativeDocumentFiles.Any(x => x.Type == RepresentativeDocumentType.LetterOfAttorney))
				{
                    part.Entity.UpdateFile(request.Model.LetterOfAttorney);
                } 
                else if (request.Model.LetterOfAttorney == null && part.Entity.RepresentativeDocumentFiles.Any(x => x.Type == RepresentativeDocumentType.LetterOfAttorney))
				{
                    var file = part.Entity.RepresentativeDocumentFiles.SingleOrDefault(x => x.Type == RepresentativeDocumentType.LetterOfAttorney);
                    part.Entity.RepresentativeDocumentFiles.Remove(file);
				}

                part.Entity.Update(request.Model.HasRepresentative, request.Model.Type, request.Model.FirstName, request.Model.LastName,
                    request.Model.IdentificationCode, request.Model.Mail, request.Model.Phone, request.Model.Note, request.Model.SubmissionDate);

                part.Entity.UpdateFile(request.Model.ApplicationForCertificate);

                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
