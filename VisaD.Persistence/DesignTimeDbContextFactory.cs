using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Persistence
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
	{
		public AppDbContext CreateDbContext(string[] args)
		{
			string basePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}VisaD.Hosting";
			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.Development.json", optional: true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString = configuration.GetSection("DbConfiguration:ConnectionString").Value;
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseNpgsql(connectionString)
				.EnableSensitiveDataLogging();

			return new AppDbContext(null, options.Options);
		}
	}

	public class DesignTimeLogContextFactory : IDesignTimeDbContextFactory<AppLogContext>
	{
		public AppLogContext CreateDbContext(string[] args)
		{
			string basePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}VisaD.Hosting";
			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.Development.json", optional: true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString = configuration.GetSection("DbConfiguration:LogConnectionString").Value;
			var options = new DbContextOptionsBuilder<AppLogContext>()
				.UseNpgsql(connectionString);

			return new AppLogContext(options.Options);
		}
	}
}
