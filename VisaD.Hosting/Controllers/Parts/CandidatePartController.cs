
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisaD.Application.Candidates.Commands.Entities;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Candidates.Queries;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Hosting.Controllers.Common;

namespace VisaD.Hosting.Controllers.Parts
{
	[Route("api/[controller]")]
	public class CandidatePartController : BaseMediatorController
	{
		public CandidatePartController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public async Task UpdateCandidate([FromRoute] int partId, [FromBody] CandidateDto model)
			=> await this.mediator.Send(new UpdateCandidateCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<CandidateDto>> StartCandidatePartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<CandidatePart, Candidate> { Id = partId });
			return await this.mediator.Send(new GetCandidatePartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<CandidateDto>> CancelCandidatePartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<CandidateCommit, CandidatePart, Candidate> { Id = partId });
			return await this.mediator.Send(new GetCandidatePartQuery { PartId = resultPartId });
		}
	}
}
