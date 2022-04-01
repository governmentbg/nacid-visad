using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
	public class StartDiplomaPartModificationCommandHandler : StartPartModificationCommandHandler<DiplomaPart, Diploma>
	{
		public StartDiplomaPartModificationCommandHandler(IAppDbContext context)
			: base(context)
		{
		}

		protected override IQueryable<DiplomaPart> LoadPart()
		{
			return this.context.Set<DiplomaPart>()
				.Include(e => e.Entity.DiplomaFiles)
					.ThenInclude(e => e.DiplomaDocumentFiles)
				.Include(e => e.Entity.AttachedFiles);
		}
	}
}
