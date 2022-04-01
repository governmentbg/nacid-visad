using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Candidates.Register;

namespace VisaD.Persistence.Configurations.Candidates
{
	public class CandidateLotConfiguration : IEntityTypeConfiguration<CandidateLot>
	{
		public void Configure(EntityTypeBuilder<CandidateLot> builder)
		{
			builder
				.HasMany(e => e.Commits)
				.WithOne(e => e.Lot)
				.HasForeignKey(e => e.LotId);
		}
	}
}
