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
    public class GetEducationPartQuery : IRequest<PartDto<EducationDto>>
    {
        public int PartId { get; set; }

        public class Handler : IRequestHandler<GetEducationPartQuery, PartDto<EducationDto>>
        {
            private readonly IAppDbContext context;

            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<PartDto<EducationDto>> Handle(GetEducationPartQuery request, CancellationToken cancellationToken)
            {
                var result = await context.Set<EducationPart>()
                    .AsNoTracking()
                    .Select(e => new PartDto<EducationDto> {
                        Id = e.Id,
                        Entity = new EducationDto {
                            Faculty = e.Entity.Institution != null
                            ? new NomenclatureDto<Institution> {
                                Id = e.Entity.Institution.Id,
                                Name = e.Entity.Institution.Name
                            }
                            : null,
                            Speciality = e.Entity.Speciality != null
                            ? new NomenclatureDto<Speciality> {
                                Id = e.Entity.Speciality.Id,
                                Name = e.Entity.Speciality.Name
                            }
                            : null,
                            EducationalQualification = e.Entity.EducationalQualification != null
                            ? new NomenclatureDto<EducationalQualification> {
                                Id = e.Entity.EducationalQualification.Id,
                                Name = e.Entity.EducationalQualification.Name
                            }
                            : null,
                            Form = e.Entity.Form != null
                            ? new NomenclatureDto<EducationFormType> {
                                Id = e.Entity.Form.Id,
                                Name = e.Entity.Form.Name
                            }
                            : null,
                            Duration = e.Entity.Duration,
                            SchoolYear = e.Entity.SchoolYear != null
                            ? new NomenclatureDto<SchoolYear> {
                                Id = e.Entity.SchoolYear.Id,
                                Name = e.Entity.SchoolYear.Name
							}
                            : null,
                            EducationSpecialityLanguages = e.Entity.EducationSpecialityLanguages.Select(x => new NomenclatureDto<Language> {
                                Id = x.LanguageId,
                                Name = x.Language.Name
                            }).ToList(),
                            Specialization = e.Entity.Specialization,
                            TraineeDuration = e.Entity.TraineeDuration
                        },
                        State = e.State
                    })
                    .SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

                return result;
            }
        }
    }
}
