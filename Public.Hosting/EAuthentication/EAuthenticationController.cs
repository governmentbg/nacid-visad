using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Public.Hosting.Models;
using System;
using System.IO;

namespace Public.Hosting.EAuthentication
{
	[Route("api/EAuthentication")]
	[ApiController]
	[AllowAnonymous]
	public class EAuthenticationController : ControllerBase
	{
		private readonly IOptions<ProxyPath> config;

		public EAuthenticationController(IOptions<ProxyPath> config)
		{
			this.config = config;
		}

		[HttpPost("EAuthLogin")]
		public RedirectResult EAuthLogin([FromForm] SamlResponse dto)
		{
			string url;
			if (!string.IsNullOrEmpty(dto.SAMLResponse))
			{
				var decodedResponseStream = new MemoryStream(Convert.FromBase64String(dto.SAMLResponse));
				var eAuthLoginDataDto = SamlHelper.ParseEAuthResponse(decodedResponseStream);

				var name = !string.IsNullOrEmpty(eAuthLoginDataDto.Name) ? eAuthLoginDataDto.Name : null;
				url = this.config.Value.PortalUrl + "/eAuth?responseStatus=" + eAuthLoginDataDto.ResponseStatus + "&name=" + name;
			}
			else
			{
				url = this.config.Value.PortalUrl + "/eAuth?responseStatus=" + EAuthResponseStatus.InvalidResponseXML;
			}

			return Redirect(url);
		}

		[HttpGet("EAuthMetadata")]
		public FileStreamResult GetEAuthMetadata()
		{
			var xmlResponse = SamlHelper.GenerateXmlMetadata("device.pfx", "12345");

			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(xmlResponse);
			writer.Flush();
			stream.Position = 0;

			return new FileStreamResult(stream, "text/xml");
		}
	}
}
