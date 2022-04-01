using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications.Register;

namespace VisaD.Persistence.Configurations
{
	public class ApplicationLotResultConfiguration : IEntityTypeConfiguration<ApplicationLotResult>
	{
		public void Configure(EntityTypeBuilder<ApplicationLotResult> builder)
		{
			builder
				.HasOne(e => e.File)
				.WithOne()
				.HasForeignKey<ApplicationLotResultFile>(e => e.ApplicationLotResultId);

			builder
				.HasOne(e => e.Lot)
				.WithOne(e => e.Result)
				.HasForeignKey<ApplicationLotResult>(e => e.LotId);
		}
	}
}
