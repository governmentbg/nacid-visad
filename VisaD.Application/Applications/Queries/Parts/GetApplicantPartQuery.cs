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
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Queries.Parts
{
	public class GetApplicantPartQuery : IRequest<PartDto<ApplicantDto>>
	{
		public int PartId { get; set; }

		public class Handler : IRequestHandler<GetApplicantPartQuery, PartDto<ApplicantDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<PartDto<ApplicantDto>> Handle(GetApplicantPartQuery request, CancellationToken cancellationToken)
			{
				var result = await context.Set<ApplicantPart>()
					.AsNoTracking()
					.Select(e => new PartDto<ApplicantDto> {
						Id = e.Id,
						Entity = new ApplicantDto {
							FirstName = e.Entity.FirstName,
							MiddleName = e.Entity.MiddleName,
							LastName = e.Entity.LastName,
							Position = e.Entity.Position,
							Phone = e.Entity.Phone,
							Mail = e.Entity.Mail,
							Institution = e.Entity.Institution != null
							? new NomenclatureDto<Institution> {
								Id = e.Entity.Institution.Id,
								Name = e.Entity.Institution.Name
							}
							: null
						},
						State = e.State
					})
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				return result;
			}
		}
	}
}
