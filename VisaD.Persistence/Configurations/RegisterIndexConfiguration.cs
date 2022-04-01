using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Nomenclatures;

namespace VisaD.Persistence.Configurations
{
	public class RegisterIndexConfiguration : IEntityTypeConfiguration<RegisterIndex>
	{
		public void Configure(EntityTypeBuilder<RegisterIndex> builder)
		{
			builder.HasMany(e => e.Counters)
				.WithOne(e => e.RegisterIndex)
				.HasForeignKey(e => e.RegisterIndexId);
		}
	}
}
