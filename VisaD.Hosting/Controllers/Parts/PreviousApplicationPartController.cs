using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisaD.Application.Applications.Commands.Entities;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Applications.Queries.Parts;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;
using VisaD.Hosting.Controllers.Common;

namespace VisaD.Hosting.Controllers.Parts
{
	[Route("api/[controller]")]
	public class PreviousApplicationPartController : BaseMediatorController
	{
		public PreviousApplicationPartController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdatePreviousApplication([FromRoute] int partId, [FromBody] PreviousApplicationDto model)
			=> this.mediator.Send(new UpdatePreviousApplicationCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<PreviousApplicationDto>> StartPreviousApplicationPartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<PreviousApplicationPart, PreviousApplication> { Id = partId });
			return await this.mediator.Send(new GetPreviousApplicationPartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<PreviousApplicationDto>> CancelPreviousApplicationPartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, PreviousApplicationPart, PreviousApplication> { Id = partId });
			return await this.mediator.Send(new GetPreviousApplicationPartQuery { PartId = resultPartId });
		}
	}
}
