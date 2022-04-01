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
	public class TaxAccountPartController : BaseMediatorController
	{
		public TaxAccountPartController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdateTaxAccount([FromRoute] int partId, [FromBody] TaxAccountDto model)
			=> this.mediator.Send(new UpdateTaxAccountCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<TaxAccountDto>> StartTaxAccountPartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<TaxAccountPart, TaxAccount> { Id = partId });
			return await this.mediator.Send(new GetTaxAccountPartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<TaxAccountDto>> CancelTaxAccountPartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, TaxAccountPart, TaxAccount> { Id = partId });
			return await this.mediator.Send(new GetTaxAccountPartQuery { PartId = resultPartId });
		}
	}
}
