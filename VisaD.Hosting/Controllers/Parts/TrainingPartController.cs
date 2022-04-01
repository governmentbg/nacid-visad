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
	public class TrainingPartController : BaseMediatorController
	{
		public TrainingPartController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdateTraining([FromRoute] int partId, [FromBody] TrainingDto model)
			=> this.mediator.Send(new UpdateTrainingCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<TrainingDto>> StartTrainingPartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<TrainingPart, Training> { Id = partId });
			return await this.mediator.Send(new GetTrainingPartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<TrainingDto>> CancelTrainingPartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, TrainingPart, Training> { Id = partId });
			return await this.mediator.Send(new GetTrainingPartQuery { PartId = resultPartId });
		}
	}
}
