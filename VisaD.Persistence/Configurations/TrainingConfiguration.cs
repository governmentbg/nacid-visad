using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications;

namespace VisaD.Persistence.Configurations
{
	public class TrainingConfiguration : IEntityTypeConfiguration<Training>
	{
		public void Configure(EntityTypeBuilder<Training> builder)
		{
			builder
				.Metadata
				.FindNavigation(nameof(Training.Proficiencies))
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder
				.HasMany(e => e.Proficiencies)
				.WithOne(t => t.Training)
				.HasForeignKey(t => t.TrainingId)
				.OnDelete(DeleteBehavior.Cascade);

			builder
			   .HasOne(e => e.TrainingLanguageDocument)
			   .WithOne(t => t.Training)
			   .HasForeignKey<TrainingLanguageDocumentFile>(t => t.TrainingId)
			   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
