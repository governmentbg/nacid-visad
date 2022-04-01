using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates.Register;

namespace VisaD.Persistence.Configurations
{
	public class ApplicationCommitConfiguration : IEntityTypeConfiguration<ApplicationCommit>
	{
		public void Configure(EntityTypeBuilder<ApplicationCommit> builder)
		{
			builder
				.HasOne(e => e.ApplicantPart)
				.WithOne()
				.HasForeignKey<ApplicantPart>(e => e.Id);

			builder
				.HasOne(e => e.EducationPart)
				.WithOne()
				.HasForeignKey<EducationPart>(e => e.Id);

			builder
				.HasOne(e => e.TrainingPart)
				.WithOne()
				.HasForeignKey<TrainingPart>(e => e.Id);

			builder
				.HasOne(e => e.TaxAccountPart)
				.WithOne()
				.HasForeignKey<TaxAccountPart>(e => e.Id);

			builder
				.HasOne(e => e.DocumentPart)
				.WithOne()
				.HasForeignKey<DocumentPart>(e => e.Id);

			builder
				.HasOne(e => e.DiplomaPart)
				.WithOne()
				.HasForeignKey<DiplomaPart>(e => e.Id);

			builder
				.HasOne(e => e.RepresentativePart)
				.WithOne()
				.HasForeignKey<RepresentativePart>(e => e.Id);

			builder
				.HasOne(e => e.PreviousApplicationPart)
				.WithOne()
				.HasForeignKey<PreviousApplicationPart>(e => e.Id);

			builder
				.HasOne(e => e.MedicalCertificatePart)
				.WithOne()
				.HasForeignKey<MedicalCertificatePart>(e => e.Id);
		}
	}
}
