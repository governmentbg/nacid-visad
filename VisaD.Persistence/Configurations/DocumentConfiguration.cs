using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications;

namespace VisaD.Persistence.Configurations
{
	public class DocumentConfiguration : IEntityTypeConfiguration<Document>
	{
		public void Configure(EntityTypeBuilder<Document> builder)
		{
			builder
				.Metadata
				.FindNavigation(nameof(Document.Files))
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder
				.HasMany(e => e.Files)
				.WithOne(t => t.Document)
				.HasForeignKey(t => t.DocumentId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
