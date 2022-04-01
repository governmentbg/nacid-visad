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
	public class MedicalCertificatePartController : BaseMediatorController
	{
		public MedicalCertificatePartController(IMediator mediator)
			: base(mediator)
		{
		}

		[HttpPut("{partId:int}/entity")]
		public Task UpdateMedicalCertificate([FromRoute] int partId, [FromBody] MedicalCertificateDto model)
			=> this.mediator.Send(new UpdateMedicalCertificateCommand { PartId = partId, Model = model });

		[HttpPost("{partId:int}/startmodification")]
		public async Task<PartDto<MedicalCertificateDto>> StartMedicalCertificatePartModification([FromRoute] int partId)
		{
			int newPartId = await this.mediator.Send(new StartPartModificationCommand<MedicalCertificatePart, MedicalCertificate> { Id = partId });
			return await this.mediator.Send(new GetMedicalCertiticatePartQuery { PartId = newPartId });
		}

		[HttpPost("{partId:int}/cancelmodification")]
		public async Task<PartDto<MedicalCertificateDto>> CancelMedicalCertificatePartModification([FromRoute] int partId)
		{
			int resultPartId = await this.mediator.Send(new CancelPartModificationCommand<ApplicationCommit, MedicalCertificatePart, MedicalCertificate> { Id = partId });
			return await this.mediator.Send(new GetMedicalCertiticatePartQuery { PartId = resultPartId });
		}
	}
}
