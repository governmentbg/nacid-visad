using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Ems.Models;
using VisaD.Hosting.BackgroundServices.Configurations;
using VisaD.Hosting.Infrastructure.Configurations;

namespace VisaD.Hosting.Extensions
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
				.Configure<AuthConfiguration>(config => configuration.GetSection("AuthConfiguration").Bind(config))
				.Configure<EmailsConfiguration>(config => configuration.GetSection("EmailConfiguration").Bind(config))
				.Configure<ApplicationFileConfiguration>(configuration.GetSection("ApplicationFileConfiguration"))
				.Configure<RndConsumerConfiguration>(configuration.GetSection("RndConsumerConfiguration"))
				.Configure<ReCaptchaConfiguration>(configuration.GetSection("ReCaptchaConfiguration"))
				.Configure<EmsConfiguration>(configuration.GetSection("EmsConfiguration"))
				.AddOptions();

			return configuration;
		}
	}
}
