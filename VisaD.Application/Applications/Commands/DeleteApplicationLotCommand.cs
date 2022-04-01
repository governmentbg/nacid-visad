using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Commands
{
	public class DeleteApplicationLotCommand : IRequest<Unit>
	{
		public int LotId { get; set; }

		public class Handler : IRequestHandler<DeleteApplicationLotCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(DeleteApplicationLotCommand request, CancellationToken cancellationToken)
			{
				var lot = await context.Set<ApplicationLot>()
					.Include(e => e.Commits)
						.ThenInclude(e => e.ApplicantPart)
							.ThenInclude(a => a.Entity)
					.Include(e => e.Commits)
						.ThenInclude(e => e.EducationPart)
							.ThenInclude(ed => ed.Entity)
					.Include(e => e.Commits)
						.ThenInclude(e => e.TrainingPart)
							.ThenInclude(t => t.Entity)
								.ThenInclude(te => te.Proficiencies)
					.Include(e => e.Commits)
						.ThenInclude(e => e.TaxAccountPart)
							.ThenInclude(t => t.Entity)
								.ThenInclude(te => te.Taxes)
					.Include(e => e.Commits)
						.ThenInclude(e => e.DocumentPart)
							.ThenInclude(t => t.Entity)
								.ThenInclude(de => de.Files)
					.Include(e => e.Commits)
						.ThenInclude(e => e.RepresentativePart)
							.ThenInclude(r => r.Entity)
					.Include(e => e.Commits)
						.ThenInclude(e => e.DiplomaPart)
							.ThenInclude(d => d.Entity)
								.ThenInclude(de => de.DiplomaFiles)
					.Include(e => e.Commits)
						.ThenInclude(e => e.MedicalCertificatePart)
							.ThenInclude(e => e.Entity)
					.SingleAsync(e => e.Id == request.LotId, cancellationToken);

				if(lot.Commits.Any(c => c.State != CommitState.InitialDraft))
				{
					// TODO: Add more specific error
					throw new ArgumentException("Cannot delete lot with commits not in initial draft!");
				}

				lot.Result.Type = ApplicationLotResultType.Deleted;

				context.Set<ApplicationCommit>().RemoveRange(lot.Commits);
				context.Set<Applicant>().RemoveRange(lot.Commits.Select(c => c.ApplicantPart.Entity));
				context.Set<Education>().RemoveRange(lot.Commits.Select(c => c.EducationPart.Entity));
				context.Set<Training>().RemoveRange(lot.Commits.Select(c => c.TrainingPart.Entity));
				context.Set<TaxAccount>().RemoveRange(lot.Commits.Select(c => c.TaxAccountPart.Entity));
				context.Set<Document>().RemoveRange(lot.Commits.Select(c => c.DocumentPart.Entity));
				context.Set<Representative>().RemoveRange(lot.Commits.Select(c => c.RepresentativePart.Entity));
				context.Set<Diploma>().RemoveRange(lot.Commits.Select(c => c.DiplomaPart.Entity));
				context.Set<MedicalCertificate>().RemoveRange(lot.Commits.Select(c => c.MedicalCertificatePart.Entity));
				context.Set<ApplicationLot>().Remove(lot);
				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}

	}
}
