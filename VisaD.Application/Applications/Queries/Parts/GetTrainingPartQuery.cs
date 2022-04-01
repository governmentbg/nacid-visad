using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Queries.Parts
{
	public class GetTrainingPartQuery : IRequest<PartDto<TrainingDto>>
	{
		public int PartId { get; set; }

		public class Handler : IRequestHandler<GetTrainingPartQuery, PartDto<TrainingDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<PartDto<TrainingDto>> Handle(GetTrainingPartQuery request, CancellationToken cancellationToken)
			{
				var result = await context.Set<TrainingPart>()
					.AsNoTracking()
					.Select(e => new PartDto<TrainingDto> {
						Id = e.Id,
						Entity = new TrainingDto {
							LanguageDepartment = e.Entity.LanguageDepartment,
							LanguageTrainingDuration = e.Entity.LanguageTrainingDuration,
							TrainingLanguageDocumentFile = e.Entity.TrainingLanguageDocument,
							LanguageProficiencies = e.Entity.Proficiencies
									.Select(lpe => new LanguageProficiencyDto {
										Id = lpe.Id,
										Language = lpe.Language != null 
										? new NomenclatureDto<Language> {
											Id = lpe.Language.Id,
											Name = lpe.Language.Name
										}
										: null,
										Reading = lpe.Reading != null
										? new NomenclatureDto<LanguageDegree> {
											Id = lpe.Reading.Id,
											Name = lpe.Reading.Name
										}
										: null,
										Writing = lpe.Writing != null
										? new NomenclatureDto<LanguageDegree> {
											Id = lpe.Writing.Id,
											Name = lpe.Writing.Name
										}
										: null,
										Speaking = lpe.Speaking != null
										? new NomenclatureDto<LanguageDegree> {
											Id = lpe.Speaking.Id,
											Name = lpe.Speaking.Name
										}
										: null
									})
						},
						State = e.State
					})
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				return result;
			}
		}
	}
}
