using FileStorageNetCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Ems.Models;
using VisaD.Hosting.BackgroundServices;
using VisaD.Hosting.BackgroundServices.Configurations;
using VisaD.Hosting.Controllers;
using VisaD.Hosting.Extensions;
using VisaD.Hosting.Infrastructure.Configurations;
using VisaD.Hosting.Infrastructure.Middlewares;
using VisaD.Infrastructure;
using VisaD.Persistence;
using VisaD.Persistence.Extensions;

namespace VisaD.Hosting
{
	public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public Startup(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options => {
                    options.OutputFormatters.Add(new HttpNoContentOutputFormatter());
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddFluentValidation()
                .AddJson()
            ;

            var configuration = services.ConfigureApplicationConfiguration(environment);

            services
                .AddPersistence<IAppDbContext, AppDbContext>(configuration.GetSection("DbConfiguration:ConnectionString").Value, environment.IsDevelopment())
                .AddPersistence<IAppLogContext, AppLogContext>(configuration.GetSection("DbConfiguration:LogConnectionString").Value, environment.IsDevelopment())
                .AddApplication(typeof(IUserContext).Assembly)
                .AddApiServices()
            ;

            var authConfig = configuration.GetSection("AuthConfiguration").Get<AuthConfiguration>();
            services.ConfigureJwtAuthService(authConfig.SecretKey, authConfig.Issuer, authConfig.Audience);

            var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailsConfiguration>();
            if (emailConfiguration.JobEnabled)
			{
                services.AddHostedService<EmailJob>();
			}

            services.AddFileStorage(configuration.GetSection("DbConfiguration:Descriptors"), configuration.GetSection("DbConfiguration:Encryption"));

            services.ConfigureAuthorization();

            var emsConfiguration = configuration.GetSection("EmsConfiguration").Get<EmsConfiguration>();
            services.AddEmsService(emsConfiguration.EmsUrl);

            var rndConsumerConfiguration = configuration.GetSection("RndConsumerConfiguration").Get<RndConsumerConfiguration>();
            if (rndConsumerConfiguration.IsConsumerEnabled)
            {
                services.AddConsumers();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMiddleware<RedirectionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions {
                OnPrepareResponse = context => {
                    if (context.File.Name == "index.html")
                    {
                        context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                        context.Context.Response.Headers.Add("Expires", "-1");
                    }
                }
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints
                .MapControllers()
				.RequireAuthorization()
			);
        }
    }
}
