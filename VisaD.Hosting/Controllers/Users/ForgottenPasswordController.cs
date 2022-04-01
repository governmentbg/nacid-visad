using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisaD.Application.Users.Commands;

namespace VisaD.Hosting.Controllers.Users
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ForgottenPasswordController : ControllerBase
	{
		private readonly IMediator mediator;

		public ForgottenPasswordController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpPost]
		public Task SendForgottenPasswordMail([FromBody] CreateForgottenPasswordRecoveryCommand command)
			=> this.mediator.Send(command);

		[HttpPost("Recovery")]
		public Task RecoverPassword([FromBody] RecoverForgottenPasswordCommand command)
			=> this.mediator.Send(command);
	}
}
