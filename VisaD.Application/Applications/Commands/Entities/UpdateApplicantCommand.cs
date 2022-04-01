using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateApplicantCommand : IRequest<Unit>
	{
		public ApplicantDto Model { get; set; }
		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdateApplicantCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateApplicantCommand request, CancellationToken cancellationToken)
			{
				var part = await context.Set<ApplicantPart>()
					.Include(e => e.Entity)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				part.Entity.Update(request.Model.Institution.Id, request.Model.FirstName, request.Model.MiddleName, request.Model.LastName, request.Model.Position, request.Model.Phone, request.Model.Mail);
				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
