using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Hosting.Infrastructure.Auth
{
	public class UserContext : IUserContext
	{
		private ClaimsPrincipal _user;

		public int UserId => int.Parse(_user.Claims.Single(c => c.Type.Equals(JwtRegisteredClaimNames.Jti)).Value);
		public string Username => _user.Claims.Single(c => c.Type.Equals("username")).Value;
		public string OrganizationName => _user.Claims.Single(c => c.Type.Equals("organizationName")).Value;
		public string Role => _user.Claims.Single(c => c.Type == ClaimTypes.Role).Value;


		public UserContext(IHttpContextAccessor contextAccessor)
		{
			this._user = contextAccessor.HttpContext?.User;

            //UserId = int.Parse(contextAccessor.HttpContext.User.Claims.Single(c => c.Type.Equals(JwtRegisteredClaimNames.Jti)).Value);
            //Username = contextAccessor.HttpContext.User.Claims.Single(c => c.Type.Equals("username")).Value;
            //OrganizationName = contextAccessor.HttpContext.User.Claims.Single(c => c.Type.Equals("organizationName")).Value;
        }
	}
}
