using FileStorageNetCore.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries.Parts
{
	public class GetMedicalCertiticatePartQuery : IRequest<PartDto<MedicalCertificateDto>>
	{
		public int PartId { get; set; }

		public class Handler : IRequestHandler<GetMedicalCertiticatePartQuery, PartDto<MedicalCertificateDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<PartDto<MedicalCertificateDto>> Handle(GetMedicalCertiticatePartQuery request, CancellationToken cancellationToken)
			{
				var result = await this.context.Set<MedicalCertificatePart>()
					.AsNoTracking()
					.Select(x => new PartDto<MedicalCertificateDto> {
						Id = x.Id,
						State = x.State,
						Entity = new MedicalCertificateDto {
							Id = x.Entity.Id,
							File = new AttachedFile {
								Key = x.Entity.Key,
								Hash = x.Entity.Hash,
								Size = x.Entity.Size,
								Name = x.Entity.Name,
								MimeType = x.Entity.MimeType,
								DbId = x.Entity.DbId
							},
							IssuedDate = x.Entity.IssuedDate
						}
					}).SingleOrDefaultAsync(x => x.Id == request.PartId);

				return result;
			}
		}
	}
}
