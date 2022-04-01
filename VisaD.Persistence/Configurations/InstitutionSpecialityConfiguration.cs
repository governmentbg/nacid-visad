using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Nomenclatures;

namespace VisaD.Persistence.Configurations
{
	public class InstitutionSpecialityConfiguration : IEntityTypeConfiguration<InstitutionSpeciality>
	{
		public void Configure(EntityTypeBuilder<InstitutionSpeciality> builder)
		{
			builder
				.HasMany(e => e.InstitutionSpecialityLanguages)
				.WithOne(l => l.InstitutionSpeciality)
				.HasForeignKey(l => l.InstitutionSpecialityId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
