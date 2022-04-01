using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Candidates.Register;

namespace VisaD.Persistence.Configurations
{
	public class CandidateCommitConfiguration : IEntityTypeConfiguration<CandidateCommit>
	{
		public void Configure(EntityTypeBuilder<CandidateCommit> builder)
		{
			builder
				.HasOne(e => e.CandidatePart)
				.WithOne()
				.HasForeignKey<CandidatePart>(e => e.Id);
		}
	}
}
