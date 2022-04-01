using System.Linq;

namespace VisaD.Application.Common.Extensions
{
	public static class QueryExtensions
	{
		public static IQueryable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> query, int offset, int limit)
			where TEntity : class
		{
			return query.Skip(offset)
				.Take(limit);
		}
	}
}
