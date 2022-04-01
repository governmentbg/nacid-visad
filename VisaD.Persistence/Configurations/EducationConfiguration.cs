using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications;

namespace VisaD.Persistence.Configurations
{
	public class EducationConfiguration : IEntityTypeConfiguration<Education>
	{
		public void Configure(EntityTypeBuilder<Education> builder)
		{
			builder
				.HasMany(e => e.EducationSpecialityLanguages)
				.WithOne(el => el.Education)
				.HasForeignKey(el => el.EducationId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
