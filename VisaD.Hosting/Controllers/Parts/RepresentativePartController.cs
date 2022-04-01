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
    [ApiController]
    public class RepresentativePartController : BaseMediatorController
    {
		public RepresentativePartController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdateRepresentative([FromRoute] int partId, [FromBody] RepresentativeDto model)
			=> this.mediator.Send(new UpdateRepresentativeCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<RepresentativeDto>> StartRepresentativePartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<RepresentativePart, Representative> { Id = partId });
			return await this.mediator.Send(new GetRepresentativePartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<RepresentativeDto>> CancelRepresentativePartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, RepresentativePart, Representative> { Id = partId });
			return await this.mediator.Send(new GetRepresentativePartQuery { PartId = resultPartId });
		}
	}
}
