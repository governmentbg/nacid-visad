using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisaD.Application.Users.Commands;
using VisaD.Application.Users.Dtos;

namespace VisaD.Hosting.Controllers.Users
{
	[ApiController]
	[AllowAnonymous]
	[Route("api/[controller]")]
	public class LoginController : ControllerBase
	{
		private readonly IMediator mediator;

		public LoginController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet]
		public Task<UnauthorizedUserInfoDto> GetUnauthorizedUserInformation()
			=> this.mediator.Send(new GetUnauthorizedUserInformationCommand());

		[HttpPost]
		public Task<UserLoginInfoDto> LoginUser([FromBody] LoginUserCommand command)
			=> this.mediator.Send(command);
	}
}
