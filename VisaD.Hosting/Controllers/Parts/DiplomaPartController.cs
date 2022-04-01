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
	public class DiplomaPartController : BaseMediatorController
	{
		public DiplomaPartController(IMediator mediator)
			:base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdateDiploma([FromRoute] int partId, [FromBody] DiplomaDto model)
			=> this.mediator.Send(new UpdateDiplomaCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<DiplomaDto>> StartDiplomaPartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<DiplomaPart, Diploma> { Id = partId });
			return await this.mediator.Send(new GetDiplomaPartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<DiplomaDto>> CancelDiplomaPartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, DiplomaPart, Diploma> { Id = partId });
			return await this.mediator.Send(new GetDiplomaPartQuery { PartId = resultPartId });
		}
	}
}
