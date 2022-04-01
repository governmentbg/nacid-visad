using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
    public class StartRepresentativePartModificationCommandHandler : StartPartModificationCommandHandler<RepresentativePart, Representative>
    {
        public StartRepresentativePartModificationCommandHandler(IAppDbContext context)
            : base(context)
        {
        }

        protected override IQueryable<RepresentativePart> LoadPart()
        {
            return this.context.Set<RepresentativePart>()
                .Include(e => e.Entity)
                    .ThenInclude(e => e.RepresentativeDocumentFiles);
        }
    }
}
