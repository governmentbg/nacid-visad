using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace VisaD.Hosting.Infrastructure.Captcha
{
	public class CaptchaFilter : IAsyncActionFilter
	{
		private readonly CaptchaService captchaService;

		public CaptchaFilter(CaptchaService captchaService)
		{
			this.captchaService = captchaService;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var captcha = context.ActionArguments["captcha"] as string;

			bool isValid = await this.captchaService.Verify(captcha);
			if (!isValid)
			{
				var controller = context.Controller as ControllerBase;
				context.Result = controller.Forbid();
				return;
			}

			await next();
		}
	}
}
