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
	public class DocumentPartController : BaseMediatorController
	{
		public DocumentPartController(IMediator mediator)
			:base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdateDocument([FromRoute] int partId, [FromBody] DocumentDto model)
			=> this.mediator.Send(new UpdateDocumentCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<DocumentDto>> StartDocumentPartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<DocumentPart, Document> { Id = partId });
			return await this.mediator.Send(new GetDocumentPartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<DocumentDto>> CancelDocumentPartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, DocumentPart, Document> { Id = partId });
			return await this.mediator.Send(new GetDocumentPartQuery { PartId = resultPartId });
		}
	}
}
