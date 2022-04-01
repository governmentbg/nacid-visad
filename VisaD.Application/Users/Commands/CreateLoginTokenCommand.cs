using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Configurations;

namespace VisaD.Application.Users.Commands
{
	public class CreateLoginTokenCommand : IRequest<string>
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public string OrganizationName { get; set; }
		public string Role { get; set; }

		public class Handler : IRequestHandler<CreateLoginTokenCommand, string>
		{
			private readonly AuthConfiguration authConfiguration;

			public Handler(IOptions<AuthConfiguration> options)
			{
				this.authConfiguration = options.Value;
			}

			public Task<string> Handle(CreateLoginTokenCommand request, CancellationToken cancellationToken)
			{
				var claims = new List<Claim> { 
					new Claim("username", request.Username),
					new Claim(JwtRegisteredClaimNames.Jti, request.UserId.ToString()),
					new Claim("organizationName", request.OrganizationName ?? string.Empty),
					new Claim("role", request.Role)
				};

				var expires = DateTime.Now.AddHours(authConfiguration.ValidHours);

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.SecretKey));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					authConfiguration.Issuer,
					authConfiguration.Audience,
					claims,
					expires: expires,
					signingCredentials: creds
				);

				string tokenString = new JwtSecurityTokenHandler()
					.WriteToken(token);
				return Task.FromResult(tokenString);
			}
		}
	}
}
