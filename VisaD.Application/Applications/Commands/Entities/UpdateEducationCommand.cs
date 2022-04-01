using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateEducationCommand : IRequest<Unit>
	{
		public EducationDto Model { get; set; }
		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdateEducationCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateEducationCommand request, CancellationToken cancellationToken)
			{
				var part = await context.Set<EducationPart>()
					.Include(e => e.Entity)
						.ThenInclude(e => e.EducationSpecialityLanguages)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				part.Entity.Update(request.Model.Speciality?.Id, request.Model.SchoolYear.Id, request.Model.EducationalQualification.Id, request.Model.Form?.Id, request.Model.Duration,
					 request.Model.Faculty?.Id, request.Model.Specialization, request.Model.TraineeDuration);

				if (request.Model.EducationSpecialityLanguages != null)
				{
					var languagesToAdd = request.Model.EducationSpecialityLanguages.Where(e => !part.Entity.EducationSpecialityLanguages.Select(t => t.LanguageId).Contains(e.Id));
					foreach (var educationSpecialityLanguage in languagesToAdd)
					{
						var language = new EducationSpecialityLanguage {
							LanguageId = educationSpecialityLanguage.Id,
						};

						part.Entity.EducationSpecialityLanguages.Add(language);
					}

					var languagesToRemove = part.Entity.EducationSpecialityLanguages.Where(e => !request.Model.EducationSpecialityLanguages.Select(t => t.Id).Contains(e.LanguageId));
					foreach (var language in languagesToRemove.ToList())
					{
						part.Entity.RemoveLanguage(language.Id);
					}
				}
				else
				{
					foreach (var language in part.Entity.EducationSpecialityLanguages.ToList())
					{
						part.Entity.RemoveLanguage(language.Id);
					}
				}

				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
