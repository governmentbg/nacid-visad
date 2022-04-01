using System.Linq;
using VisaD.Application.InstitutionSpecialities.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.InstitutionSpecialities.Extensions
{
    public static class InstitutionSpecialitiesFilterExtension
    {
		public static IQueryable<InstitutionSpeciality> GetFiltered(this IQueryable<InstitutionSpeciality> query, SpecialityFilterDto filter)
		{
			if (!filter.IncludeInactive.HasValue || (filter.IncludeInactive.HasValue && !filter.IncludeInactive.Value))
			{
				query = query.Where(e => e.IsActive);
			}

			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				query = query.Where(e => e.Speciality.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}

			return query;
		}
	}
}
