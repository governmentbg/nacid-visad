using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VisaD.Hosting.Infrastructure.Auth
{
	public class ClaimAuthorizationAttribute : TypeFilterAttribute
	{
		public ClaimAuthorizationAttribute(string claimType, string claim1Value, ClaimOperator claimOperator = ClaimOperator.Single, string claim2Value = "")
			: base(typeof(ClaimAuthorizationFilter))
		{
			Arguments = new object[]
			{
				new Claim(claimType, claim1Value),
				claimOperator,
				new Claim(claimType, claim2Value)
			};
		}
	}
}
