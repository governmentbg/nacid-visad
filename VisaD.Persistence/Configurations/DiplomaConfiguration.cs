using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Diplomas;

namespace VisaD.Persistence.Configurations
{
    public class DiplomaConfiguration : IEntityTypeConfiguration<Diploma>
    {
        public void Configure(EntityTypeBuilder<Diploma> builder)
        {
            builder
                .Metadata
                .FindNavigation(nameof(Diploma.DiplomaFiles))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder
                .HasMany(e => e.DiplomaFiles)
                .WithOne(t => t.Diploma)
                .HasForeignKey(t => t.DiplomaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class DiplomaFileConfiguration : IEntityTypeConfiguration<DiplomaFile>
    {
        public void Configure(EntityTypeBuilder<DiplomaFile> builder)
        {
            builder
                .HasMany(e => e.DiplomaDocumentFiles)
                .WithOne(t => t.DiplomaFile)
                .HasForeignKey(t => t.DiplomaFileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
