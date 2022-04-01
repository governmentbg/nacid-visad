using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VisaD.Hosting.Controllers.Common
{
	[ApiController]
	public abstract class BaseMediatorController : ControllerBase
	{
		protected IMediator mediator;

		public BaseMediatorController(IMediator mediator)
		{
			this.mediator = mediator;
		}
	}
}
