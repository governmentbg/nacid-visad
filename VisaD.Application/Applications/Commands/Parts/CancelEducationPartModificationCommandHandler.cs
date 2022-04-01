using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
	public class CancelEducationPartModificationCommandHandler : CancelPartModificationCommandHandler<ApplicationCommit, EducationPart, Education>
	{
		public CancelEducationPartModificationCommandHandler(IAppDbContext context)
			: base(context)
		{
		}

		protected override IQueryable<EducationPart> LoadPart()
		{
			return context.Set<EducationPart>()
				.Include(e => e.Entity)
					.ThenInclude(e => e.EducationSpecialityLanguages);
		}
	}
}
