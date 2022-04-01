using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace VisaD.Hosting.Extensions
{
	public static class JwtConfigurationExtension
	{
		public static IServiceCollection ConfigureJwtAuthService(this IServiceCollection services, string secretKey, string issuer, string audience)
		{
			var keyByteArray = Encoding.UTF8.GetBytes(secretKey);
			var signingKey = new SymmetricSecurityKey(keyByteArray);

			var tokenValidationParameters = new TokenValidationParameters {
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = signingKey,
				ValidIssuer = issuer,
				ValidAudience = audience,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};

			services
				.AddAuthentication(o => {
					o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(o => {
					o.RequireHttpsMetadata = false;
					o.SaveToken = true;
					o.TokenValidationParameters = tokenValidationParameters;
					o.Events = new JwtBearerEvents {
						OnAuthenticationFailed = context => {
							return Task.CompletedTask;
						},
						OnTokenValidated = context => {
							return Task.CompletedTask;
						}
					};
				});

			return services;
		}
	}
}
