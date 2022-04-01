using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications;

namespace VisaD.Persistence.Configurations
{
    public class RepresentativeConfiguration : IEntityTypeConfiguration<Representative>
    {
        public void Configure(EntityTypeBuilder<Representative> builder)
        {
            builder
               .HasMany(e => e.RepresentativeDocumentFiles)
               .WithOne(t => t.Representative)
               .HasForeignKey(t => t.RepresentativeId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
