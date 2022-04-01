using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProxyKit;
using Public.Hosting.Extensions;
using Public.Hosting.Middlewares;
using Public.Hosting.Models;

namespace Public.Hosting
{
	public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public IWebHostEnvironment Environment { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options => {
                    options.OutputFormatters.Add(new HttpNoContentOutputFormatter());
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddJson()
            ;

            var configuration = services.ConfigureApplicationConfiguration(Environment);
            services.ConfigureApplicationConfiguration(Environment);

            services
                .AddProxy()
                .AddControllers()
                ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ProxyPath> proxyPathsOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureProxy(proxyPathsOptions.Value);

            app.UseMiddleware<RedirectionMiddleware>();

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

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
