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
	public class ApplicantPartController : BaseMediatorController
	{
		public ApplicantPartController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdateApplicant([FromRoute] int partId, [FromBody] ApplicantDto model)
			=> this.mediator.Send(new UpdateApplicantCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<ApplicantDto>> StartApplicantPartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<ApplicantPart, Applicant> { Id = partId });
			return await this.mediator.Send(new GetApplicantPartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<ApplicantDto>> CancelApplicantPartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, ApplicantPart, Applicant> { Id = partId });
			return await this.mediator.Send(new GetApplicantPartQuery { PartId = resultPartId });
		}
	}
}
