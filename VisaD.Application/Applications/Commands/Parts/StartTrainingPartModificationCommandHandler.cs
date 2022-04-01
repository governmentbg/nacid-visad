using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
	public class StartTrainingPartModificationCommandHandler : StartPartModificationCommandHandler<TrainingPart, Training>
	{
		public StartTrainingPartModificationCommandHandler(IAppDbContext context)
			: base(context)
		{
		}

		protected override IQueryable<TrainingPart> LoadPart()
		{
			return context.Set<TrainingPart>()
				.Include(e => e.Entity)
					.ThenInclude(e => e.Proficiencies)
				.Include(e => e.Entity.TrainingLanguageDocument);
		}
	}
}
