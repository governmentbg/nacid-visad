using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateMedicalCertificateCommand : IRequest<Unit>
	{
		public int PartId { get; set; }

		public MedicalCertificateDto Model { get; set; }

		public class Handler : IRequestHandler<UpdateMedicalCertificateCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateMedicalCertificateCommand request, CancellationToken cancellationToken)
			{
				var part = await context.Set<MedicalCertificatePart>()
					.Include(e => e.Entity)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				part.Entity.Update(request.Model.File.Key, request.Model.File.Hash, request.Model.File.Size, request.Model.File.Name, request.Model.File.MimeType, request.Model.File.DbId, request.Model.IssuedDate);

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
