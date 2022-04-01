using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications.Register;

namespace VisaD.Persistence.Configurations
{
	public class ApplicationLotConfiguration : IEntityTypeConfiguration<ApplicationLot>
	{
		public void Configure(EntityTypeBuilder<ApplicationLot> builder)
		{
			builder
				.HasMany(e => e.Commits)
				.WithOne(e => e.Lot)
				.HasForeignKey(e => e.LotId);
		}
	}
}
