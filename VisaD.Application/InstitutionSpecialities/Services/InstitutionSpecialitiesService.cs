using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.InstitutionSpecialities.Dtos;
using VisaD.Application.InstitutionSpecialities.Extensions;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.InstitutionSpecialities.Services
{
	public class InstitutionSpecialitiesService
	{
		const string RegularEdicationalForm = "Редовна";

		private readonly IAppDbContext context;

		public InstitutionSpecialitiesService(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task SaveInstitution(InstitutionDto dto)
		{
			var model = await context.Set<Institution>()
				.Include(e => e.InstitutionSpecialities)
					.ThenInclude(s => s.Speciality)
				.Include(e => e.InstitutionSpecialities)
					.ThenInclude(s => s.InstitutionSpecialityLanguages)
				.SingleOrDefaultAsync(e => e.ExternalId == dto.Id);
			if (model == null)
			{
				model = new Institution(dto.Name, dto.IsActive, dto.InstitutionType, dto.Id, dto.RootId, dto.ParentId, dto.Level);
				context.Entry(model).State = EntityState.Added;
			}
			else
			{
				model.Update(dto.Name, dto.IsActive, dto.InstitutionType, dto.Id, dto.RootId, dto.ParentId, dto.Level);
			}

			var specialities = await context.Set<Speciality>()
				.AsNoTracking()
				.ToListAsync();

			var forRemove = model.InstitutionSpecialities
				.Where(e => !dto.InstitutionSpecialities.Any(d => d.Id == e.ExternalId))
				.ToList();
			if (forRemove.Any())
			{
				forRemove.ForEach(fr => model.InstitutionSpecialities.Remove(fr));
				context.Set<InstitutionSpeciality>().RemoveRange(forRemove);
			}

			foreach (var dtoInstitutionSpeciality in dto.InstitutionSpecialities)
			{
				var speciality = model.InstitutionSpecialities
					.Select(e => e.Speciality)
					.Distinct()
					.SingleOrDefault(e => e.ExternalId == dtoInstitutionSpeciality.SpecialityId);
				if (speciality == null)
				{
					speciality = specialities.SingleOrDefault(e => e.ExternalId == dtoInstitutionSpeciality.SpecialityId);
				}

				if (speciality == null)
				{
					speciality = new Speciality(
						dtoInstitutionSpeciality.Speciality.Name,
						dtoInstitutionSpeciality.SpecialityId,
						dtoInstitutionSpeciality.Speciality.EducationalQualificationId,
						dtoInstitutionSpeciality.Speciality.IsActive
					);
				}
				else
				{
					speciality.Update(
						dtoInstitutionSpeciality.Speciality.Name,
						dtoInstitutionSpeciality.SpecialityId,
						dtoInstitutionSpeciality.Speciality.EducationalQualificationId,
						dtoInstitutionSpeciality.Speciality.IsActive
					);
				}

				var institutionSpeciality = model.InstitutionSpecialities
					.SingleOrDefault(e => e.ExternalId == dtoInstitutionSpeciality.Id);
				if (institutionSpeciality != null)
				{
					institutionSpeciality.SpecialityId = speciality.Id;
					institutionSpeciality.Speciality = speciality;
					institutionSpeciality.EducationalFormId = dtoInstitutionSpeciality.EducationalFormId;
					institutionSpeciality.Duration = dtoInstitutionSpeciality.Duration;
					institutionSpeciality.IsActive = dtoInstitutionSpeciality.IsActive;

					var languagesToAdd = dtoInstitutionSpeciality.OrganizationSpecialityLanguages.Where(e => !institutionSpeciality.InstitutionSpecialityLanguages.Select(t => t.LanguageId).Contains(e.LanguageId));
					foreach (var language in languagesToAdd)
					{
						var institutionLanguage = new InstitutionSpecialityLanguage {
							LanguageId = language.LanguageId,
						};

						institutionSpeciality.InstitutionSpecialityLanguages.Add(institutionLanguage);
					}
					var languagesToRemove = institutionSpeciality.InstitutionSpecialityLanguages.Where(e => !dtoInstitutionSpeciality.OrganizationSpecialityLanguages.Select(t => t.LanguageId).Contains(e.LanguageId));

					foreach (var institutionLanguage in languagesToRemove.ToList())
					{
						institutionSpeciality.RemoveLanguage(institutionLanguage.Id);
					}
				}
				else
				{
					model.InstitutionSpecialities.Add(new InstitutionSpeciality {
						ExternalId = dtoInstitutionSpeciality.Id,
						Speciality = speciality,
						SpecialityId = speciality.Id,
						EducationalFormId = dtoInstitutionSpeciality.EducationalFormId,
						Duration = dtoInstitutionSpeciality.Duration,
						IsActive = dtoInstitutionSpeciality.IsActive
					});

					model.InstitutionSpecialities.Add(new InstitutionSpeciality(dtoInstitutionSpeciality.Id, dtoInstitutionSpeciality.InstitutionId, speciality.Id, dtoInstitutionSpeciality.EducationalFormId, dtoInstitutionSpeciality.Duration, dtoInstitutionSpeciality.IsActive, dtoInstitutionSpeciality.OrganizationSpecialityLanguages));
				}
			}

			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SpecialityInformationDto>> GetInstitutionSpecialities(SpecialityFilterDto filter)
		{
			var institutionSpecialities = this.context.Set<InstitutionSpeciality>()
				.Where(i => i.InstitutionId == filter.EntityId &&
						i.EducationalForm.Name == RegularEdicationalForm)
					.Include(f => f.EducationalForm)
					.Include(s => s.Speciality)
						.ThenInclude(q => q.EducationalQualification)
					   .Include(i => i.InstitutionSpecialityLanguages)
						.ThenInclude(x => x.Language)
				.AsNoTracking()
				.OrderBy(i => i.Speciality.EducationalQualification.Name)
				.GetFiltered(filter);

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				institutionSpecialities = institutionSpecialities.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await institutionSpecialities.Select(i =>
				new SpecialityInformationDto(i.Speciality.Id, i.Speciality.Name, i.Duration.Value, i.EducationalForm, i.Speciality.EducationalQualification, i.InstitutionSpecialityLanguages.ToList(), i.Institution))
				.ToListAsync();

			return result;
		}

		public async Task<IEnumerable<SpecialityInformationDto>> GetInstitutionSpecialitiesByEducationalQualification(SpecialityFilterDto filter, int educationalQualificationId)
		{
			var institutionSpecialities = this.context.Set<InstitutionSpeciality>()
				.Where(i => i.InstitutionId == filter.EntityId &&
						i.EducationalForm.Name == RegularEdicationalForm && i.Speciality.EducationalQualificationId == educationalQualificationId)
					.Include(f => f.EducationalForm)
					.Include(s => s.Speciality)
						.ThenInclude(q => q.EducationalQualification)
					.Include(i => i.InstitutionSpecialityLanguages)
						.ThenInclude(x => x.Language)
				.AsNoTracking()
				.OrderBy(i => i.Speciality.EducationalQualification.Name)
				.GetFiltered(filter);

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				institutionSpecialities = institutionSpecialities.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await institutionSpecialities.Select(i =>
				new SpecialityInformationDto(i.Speciality.Id, i.Speciality.Name, i.Duration.Value, i.EducationalForm, i.Speciality.EducationalQualification, i.InstitutionSpecialityLanguages.ToList(), i.Institution))
				.ToListAsync();

			return result;
		}

		public async Task<IEnumerable<NomenclatureDto<Language>>> GetInstitutionSpecialityLanguages(int? institutionId, int? specialityId)
		{
			var institutionSpecialityId = await this.context.Set<InstitutionSpeciality>()
				.Where(x => x.SpecialityId == specialityId && x.InstitutionId == institutionId && x.EducationalFormId == 1 && x.IsActive == true)
				.Select(x => x.Id)
				.FirstOrDefaultAsync();

			var specialityLanguages = await this.context.Set<InstitutionSpecialityLanguage>()
				.Include(x => x.Language)
				.Where(x => x.InstitutionSpecialityId == institutionSpecialityId)
				.Select(x => new NomenclatureDto<Language> {
					Id = x.LanguageId.Value,
					Name = x.Language.Name
				})
				.ToListAsync();

			//if specialitylanguage is 0, it is set to bulgarian in client by default
			if (specialityLanguages.Count == 0)
			{
				specialityLanguages.Clear();

				specialityLanguages = await this.context.Set<Language>()
					.Select(x => new NomenclatureDto<Language> {
						Id = x.Id,
						Name = x.Name
					})
					.ToListAsync();
			}

			return specialityLanguages;
		}

		public async Task<IEnumerable<SpecialityInformationDto>> SpecialitiesByUniversity(SpecialityFilterDto filter)
		{
			var result = new List<SpecialityInformationDto>();

			var institutionSpecialities = new List<InstitutionSpeciality>();

			if (filter.FacultyId == null)
			{
				var faculties = this.context.Set<Institution>()
				.Where(i => i.RootId == filter.EntityId)
				.AsQueryable();

				institutionSpecialities = await this.context.Set<InstitutionSpeciality>()
					.Where(i => faculties.Any(f => f.Id == i.InstitutionId) && i.EducationalForm.Name == RegularEdicationalForm)
					.Include(f => f.Institution)
					.Include(s => s.Speciality)
					.AsNoTracking()
					.GetFiltered(filter)
					.ToListAsync();
			}
			else
			{
				institutionSpecialities = await this.context.Set<InstitutionSpeciality>()
					.Where(i => i.InstitutionId == filter.FacultyId &&
							i.EducationalForm.Name == RegularEdicationalForm)
					.Include(f => f.Institution)
					.Include(s => s.Speciality)
					.AsNoTracking()
					.OrderBy(i => i.Speciality.EducationalQualification.Name)
					.GetFiltered(filter)
					.ToListAsync();
			}

			var groupedSpecialities = institutionSpecialities
			.GroupBy(x => x.Speciality.Name)
			.Select(x => x.First())
			.AsQueryable();

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				groupedSpecialities = groupedSpecialities.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			result = groupedSpecialities.Select(i =>
				new SpecialityInformationDto(i.Speciality.Id, i.Speciality.Name, i.Duration.Value, i.EducationalForm, i.Speciality.EducationalQualification, i.InstitutionSpecialityLanguages.ToList(), i.Institution))
				.ToList();

			return result;
		}
	}
}
