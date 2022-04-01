using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
	public class CancelMedicalCertificatePartModificationCommandHandler : CancelPartModificationCommandHandler<ApplicationCommit, MedicalCertificatePart, MedicalCertificate>
	{
		public CancelMedicalCertificatePartModificationCommandHandler(IAppDbContext context)
			: base(context)
		{
		}

		protected override IQueryable<MedicalCertificatePart> LoadPart()
		{
			return context.Set<MedicalCertificatePart>()
				.Include(e => e.Entity);
		}
	}
}
