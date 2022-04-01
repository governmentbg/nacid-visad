using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands
{
	public class StartApplicationCommitModificationCommandHandler : StartCommitModificationCommandHandler<ApplicationCommit>
	{
		public StartApplicationCommitModificationCommandHandler(IAppDbContext context)
			:base(context)
		{

		}

		protected override IQueryable<ApplicationCommit> LoadRelatedData(IQueryable<ApplicationCommit> query)
		{
			return query
				.Include(e => e.ApplicantPart)
				.Include(e => e.EducationPart)
				.Include(e => e.TrainingPart)
				.Include(e => e.TaxAccountPart)
				.Include(e => e.DocumentPart)
				.Include(e => e.DiplomaPart)
				.Include(e => e.RepresentativePart)
				.Include(e => e.PreviousApplicationPart)
				.Include(e => e.MedicalCertificatePart)
			;
		}
	}
}
