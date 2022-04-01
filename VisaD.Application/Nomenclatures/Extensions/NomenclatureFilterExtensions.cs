using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Common.Models;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Extensions
{
	public static class NomenclatureFilterExtensions
	{
		public static IQueryable<TNomenclature> GetFiltered<TNomenclature>(this IQueryable<TNomenclature> query, BaseNomenclatureFilterDto<TNomenclature> filter)
			where TNomenclature : Nomenclature
		{
			if (!filter.IncludeInactive.HasValue || (filter.IncludeInactive.HasValue && !filter.IncludeInactive.Value))
			{
				query = query.Where(e => e.IsActive);
			}

			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				query = query.Where(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}

			return query;
		}

		public static IQueryable<SchoolYear> GetFiltered(this IQueryable<SchoolYear> query, SchoolYearNomenclatureFilterDto filter)
		{
			query = query.GetFiltered<SchoolYear>(filter);

			if (filter.FromStartYear.HasValue)
			{
				query = query.Where(e => e.FromYear >= filter.FromStartYear.Value);
			}

            if (filter.ToStartYear.HasValue)
            {
				query = query.Where(e => e.FromYear <= filter.ToStartYear.Value);
            }

			return query;
		}

		public static IQueryable<TNomenclature> ApplyOrder<TNomenclature>(this IQueryable<TNomenclature> query, ICollection<Expression<Func<TNomenclature, object>>> orders)
		{
			for (int i = 0; i <= orders.Count - 1; i++)
			{
				if (i == 0)
				{
					query = query.OrderBy(orders.ElementAt(i));
				}
				else
				{
					query = ((IOrderedQueryable<TNomenclature>)query).ThenBy(orders.ElementAt(i));
				}

			}

			return query;
		}
	}
}
