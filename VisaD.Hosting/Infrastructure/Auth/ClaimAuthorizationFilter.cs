using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;

namespace VisaD.Hosting.Infrastructure.Auth
{
	public class ClaimAuthorizationFilter : IAuthorizationFilter
	{
		private readonly Claim claim1;
		private readonly ClaimOperator claimOperator;
		private readonly Claim claim2;

		public ClaimAuthorizationFilter(Claim claim1, ClaimOperator claimOperator, Claim claim2)
		{
			this.claim1 = claim1;
			this.claimOperator = claimOperator;
			this.claim2 = claim2;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var user = context.HttpContext.User;
			if (user == null)
			{ 
				context.Result = new UnauthorizedResult(); 
			}
			else
			{
				switch (claimOperator)
				{
					case ClaimOperator.Single:
						if (!user.HasClaim(claim1.Type, claim1.Value))
						{ 
							context.Result = new ForbidResult(); 
						}
						break;

					case ClaimOperator.And:
						if (!user.HasClaim(claim1.Type, claim1.Value)
							|| !user.HasClaim(claim2.Type, claim2.Value))
						{ 
							context.Result = new ForbidResult(); 
						}
						break;

					case ClaimOperator.Or:
						if (!user.HasClaim(claim1.Type, claim1.Value)
							&& !user.HasClaim(claim2.Type, claim2.Value))
						{ 
							context.Result = new ForbidResult(); 
						}
						break;

					default:
						throw new InvalidOperationException();
				}
			}
		}
	}
}
