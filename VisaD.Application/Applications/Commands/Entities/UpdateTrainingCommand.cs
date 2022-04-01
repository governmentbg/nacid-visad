using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateTrainingCommand : IRequest<Unit>
	{
		public TrainingDto Model { get; set; }
		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdateTrainingCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateTrainingCommand request, CancellationToken cancellationToken)
			{
				var part = await context.Set<TrainingPart>()
					.Include(e => e.Entity)
						.ThenInclude(e => e.Proficiencies)
					.Include(e => e.Entity.TrainingLanguageDocument)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				part.Entity.Update(request.Model.LanguageDepartment, request.Model.LanguageTrainingDuration);

				if (request.Model.TrainingLanguageDocumentFile != null)
				{
					if (request.Model.TrainingLanguageDocumentFile.Id == 0)
					{
						part.Entity.AddFile(request.Model.TrainingLanguageDocumentFile.Key, request.Model.TrainingLanguageDocumentFile.Hash, request.Model.TrainingLanguageDocumentFile.Size,
						request.Model.TrainingLanguageDocumentFile.Name, request.Model.TrainingLanguageDocumentFile.MimeType, request.Model.TrainingLanguageDocumentFile.DbId);
					}
					else
					{
						part.Entity.UpdateFile(request.Model.TrainingLanguageDocumentFile.Key, request.Model.TrainingLanguageDocumentFile.Hash, request.Model.TrainingLanguageDocumentFile.Size,
						request.Model.TrainingLanguageDocumentFile.Name, request.Model.TrainingLanguageDocumentFile.MimeType, request.Model.TrainingLanguageDocumentFile.DbId);
					}
				}

				var proficienciesForAdd = request.Model.LanguageProficiencies.Where(e => !part.Entity.Proficiencies.Select(t => t.Id).Contains(e.Id));
				foreach (var proficiencyForAdd in proficienciesForAdd)
				{
					part.Entity.AddProficiency(proficiencyForAdd.Language.Id, proficiencyForAdd.Reading.Id, proficiencyForAdd.Writing.Id, proficiencyForAdd.Speaking.Id);
				}

				var proficienciesForUpdate = request.Model.LanguageProficiencies.Where(e => part.Entity.Proficiencies.Select(t => t.Id).Contains(e.Id));
				foreach (var proficiencyForUpdate in proficienciesForUpdate)
				{
					part.Entity.UpdateProficiency(proficiencyForUpdate.Id, proficiencyForUpdate.Language.Id, proficiencyForUpdate.Reading.Id, proficiencyForUpdate.Writing.Id, proficiencyForUpdate.Speaking.Id);
				}

				var proficienciesForRemove = part.Entity.Proficiencies.Where(e => !request.Model.LanguageProficiencies.Select(t => t.Id).Contains(e.Id));
				foreach (var proficiencyForRemove in proficienciesForRemove)
				{
					part.Entity.RemoveProficiency(proficiencyForRemove.Id);
				}

				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
