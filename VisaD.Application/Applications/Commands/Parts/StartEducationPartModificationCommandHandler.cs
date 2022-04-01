using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
	public class StartEducationPartModificationCommand : StartPartModificationCommandHandler<EducationPart, Education>
	{
		public StartEducationPartModificationCommand(IAppDbContext context)
			: base(context)
		{
		}

		protected override IQueryable<EducationPart> LoadPart()
		{
			return this.context.Set<EducationPart>()
				.Include(e => e.Entity)
					.ThenInclude(x => x.EducationSpecialityLanguages);
		}
	}
}
