using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Application.Nomenclatures.Extensions;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.InstitutionSpecialities.Services
{
    public class InstitutionService
    {
		private readonly IAppDbContext context;

		public InstitutionService(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<TDto>> GetUniversities<TFilter, TDto>(NomenclatureFilterDto<Institution> filter)
			where TDto : IMapping<Institution, TDto>, new()
		{
			IQueryable<Institution> query = this.context.Set<Institution>()
				.AsNoTracking()
				.Where(u => u.Level == Data.Nomenclatures.Enums.Level.First)
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders)
			;

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await query.Select(new TDto().Map())
				.ToListAsync();

			return result;
		}

		public async Task<IEnumerable<TDto>> GetFaculties<TFilter, TDto>(NomenclatureFilterDto<Institution> filter)
			where TDto : IMapping<Institution, TDto>, new()
		{

			IQueryable<int> uniQuery = this.context.Set<Institution>()
				.AsNoTracking()
				.Where(u => u.Id == filter.EntityId)
				.Select(e => e.RootId);
			var query = context.Set<Institution>()
				.Where(f => uniQuery.Contains(f.RootId) && (f.Level == Data.Nomenclatures.Enums.Level.Second || f.Level == Data.Nomenclatures.Enums.Level.Third))
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders);

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await query.Select(new TDto().Map())
				.ToListAsync();

			return result;
		}

		public async Task<IEnumerable<TDto>> GetFacultiesByEducationalQualification<TFilter, TDto>(NomenclatureFilterDto<Institution> filter, int educationalQualificationId, bool isTrainee)
			where TDto : IMapping<Institution, TDto>, new()
		{
			IQueryable<int> uniQuery = this.context.Set<Institution>()
				.AsNoTracking()
				.Where(u => u.Id == filter.EntityId)
				.Select(e => e.RootId);
			var query = context.Set<Institution>()
				.Where(f => uniQuery.Contains(f.RootId) && (f.Level == Data.Nomenclatures.Enums.Level.Second || f.Level == Data.Nomenclatures.Enums.Level.Third) 
				&& (isTrainee ? f.InstitutionSpecialities.Any(x => x.Speciality.EducationalQualificationId != educationalQualificationId) : f.InstitutionSpecialities.Any(x => x.Speciality.EducationalQualificationId == educationalQualificationId)))
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders);

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await query.Select(new TDto().Map())
				.ToListAsync();

			return result;
		}

		public async Task<Institution> GetSingleInstitutionByName(string institutionName)
		{
			var institution = await this.context.Set<Institution>()
				.AsNoTracking()
				.SingleOrDefaultAsync(u => u.Name == institutionName);

			return institution;
		}

		public async Task<TDto> GetSingle<TFilter, TDto>(NomenclatureFilterDto<Institution> filter, string institutionName)
			where TDto : IMapping<Institution, TDto>, new()
		{
			var query = context.Set<Institution>()
				.Where(u => u.Name == institutionName)
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders);

			var result = await query.Select(new TDto().Map())
				.SingleOrDefaultAsync();

			return result;
		}

		public async Task<IEnumerable<NomenclatureDto<Institution>>> GetApplicationsUniversities<TFilter, TDto>(NomenclatureFilterDto<Institution> filter)
			where TDto : IMapping<Institution, TDto>, new()
		{
			var universities = await this.context.Set<ApplicationCommit>()
				.AsNoTracking()
				.Select(u => u.ApplicantPart.Entity.Institution)
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders)
				.ToListAsync();

			var groupedUniversities = universities
				.GroupBy(x => x.Name)
				.Select(x => x.First())
				.AsQueryable();

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				groupedUniversities = groupedUniversities.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = groupedUniversities
				.Select(u => new NomenclatureDto<Institution> {
					Id = u.Id,
					Name = u.Name
				})
				.ToList();

			return result;
		}
	}
}
