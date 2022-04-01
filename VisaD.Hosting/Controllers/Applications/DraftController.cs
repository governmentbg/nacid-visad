using VisaD.Hosting.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Security.Claims;
using System.Threading.Tasks;
using VisaD.Application.Common.Constants;
using VisaD.Hosting.Infrastructure.Auth;
using VisaD.Application.Applications.Commands;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Applications.Queries;
using System.Collections.Generic;

namespace VisaD.Hosting.Controllers.Applications
{
	[Route("api/[controller]")]
	public class DraftController : BaseMediatorController
	{
		public DraftController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpGet]
		public async Task<IEnumerable<ApplicationDraftDto>> GetAllDrafts()
			=> await this.mediator.Send(new GetApplicationDraftsQuery());

		[HttpGet("{id:int}")]
		public async Task<ApplicationDraftDto> GetById([FromRoute]int id)
			=> await this.mediator.Send(new GetApplicationDraftByIdQuery { DraftId = id});

		[HttpPost]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
		public async Task CreateDraft([FromBody] ApplicationDraftDto draft)
			=> await this.mediator.Send(new CreateDraftCommand { Draft = draft} );

		[HttpPut("{id:int}")]
		public async Task SaveDraft([FromRoute] int id, [FromBody] ApplicationDraftDto draft)
		=> await this.mediator.Send(new SaveApplicationDraftCommand { DraftId = id, Draft = draft });

		[HttpDelete("{id:int}")]
		public async Task Delete([FromRoute] int id)
		=> await this.mediator.Send(new DeleteApplicationDraftCommand { DraftId = id });
	}
}
