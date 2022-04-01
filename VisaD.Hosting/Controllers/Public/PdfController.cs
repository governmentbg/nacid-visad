using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisaD.Application.Applications.Queries;
using VisaD.Hosting.Infrastructure.Captcha;

namespace VisaD.Hosting.Controllers.Public
{
	[Route("api/public/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class PdfController : ControllerBase
	{
		private readonly IMediator mediator;

		public PdfController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet]
		[TypeFilter(typeof(CaptchaFilter))]
		public async Task<string> GetPdfByAccessCode([FromQuery] string accessCode, [FromQuery] string captcha)
			=> await this.mediator.Send(new GetPublicApplicationPdfQuery { AccessCode = accessCode });
	}
}
