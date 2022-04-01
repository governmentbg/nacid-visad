using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace VisaD.Application.Common.Interfaces
{
	public interface IAppLogContext
	{
		DbSet<T> Set<T>() where T : class;
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		EntityEntry<TEntity> Entry<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
	}
}
