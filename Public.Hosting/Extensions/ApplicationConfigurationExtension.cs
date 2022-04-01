using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Public.Hosting.Models;

namespace Public.Hosting.Extensions
{
	public static class ApplicationConfigurationExtension
    {
		public static IConfiguration ConfigureApplicationConfiguration(this IServiceCollection services, IWebHostEnvironment environment)
		{
			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
			IConfiguration configuration = configurationBuilder.Build();

			services
				.Configure<ProxyPath>(configuration.GetSection("ProxyPaths"))
				.AddOptions()
				;

			return configuration;
		}
	}
}
