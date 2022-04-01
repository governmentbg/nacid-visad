using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VisaD.Persistence.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistence<TDbContext, TDbContextImaplementation>(this IServiceCollection services, string connectionString, bool isEnabledSensitiveDataLogging)
			where TDbContext : class
			where TDbContextImaplementation : DbContext, TDbContext
		{
			services
				.AddDbContext<TDbContextImaplementation>(options => {
					options.UseNpgsql(connectionString);

					if(isEnabledSensitiveDataLogging)
					{
						options.EnableSensitiveDataLogging();
					}
				});

			services
				.AddScoped<TDbContext>(provider => provider.GetRequiredService<TDbContextImaplementation>());

			return services;
		}
	}
}
