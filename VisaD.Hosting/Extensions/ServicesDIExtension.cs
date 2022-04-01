using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VisaD.Application.Common.Interfaces;
using VisaD.Hosting.Infrastructure.Auth;
using VisaD.Hosting.Infrastructure.Captcha;

namespace VisaD.Application.Common.Extensions
{
	public static class ServicesDIExtension
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			services
				.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
			;
			
			services
				.AddScoped<IUserContext, UserContext>()
				.AddScoped<CaptchaService>()
				;

			return services;
		}
	}
}
