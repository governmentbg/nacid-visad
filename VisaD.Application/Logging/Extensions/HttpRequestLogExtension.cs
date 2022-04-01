using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace VisaD.Application.Logging.Extensions
{
	public static class HttpRequestLogExtension
	{
		public static int? GetUserId(this HttpRequest request)
		{
			int? userId = null;

			try
			{
				if (request != null)
				{
					var user = request.HttpContext.User;

					if (user != null)
					{
						var userClaims = user.Claims;
						var claim = user.Claims.SingleOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Jti));

						if (claim != null && int.TryParse(claim.Value, out int uId))
						{
							userId = uId;
						}
					}
				}
			}
			catch
			{
			}

			return userId;
		}
	}
}
