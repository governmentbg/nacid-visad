using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisaD.Application.Users.Commands;
using VisaD.Data.Users.Enums;

namespace VisaD.Hosting.Controllers.Users
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ActivationController : ControllerBase
	{
		private readonly IMediator mediator;

		public ActivationController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpPost]
		public async Task ActivateUser([FromBody] UnlockUserCommand command)
			=> await this.mediator.Send(command);

		

		[HttpGet("userActivation")]
		public async Task SendActivationLink([FromQuery] int userId)
			=> await this.mediator.Send(new SendUserActivationLinkCommand { Id = userId });

		[HttpGet]
		public async Task CheckToken([FromQuery] string token)
			=> await this.mediator.Send(new CheckTokenCommand { Token = token});
	}
}
