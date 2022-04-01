using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Applications;

namespace VisaD.Persistence.Configurations
{
	public class TaxAccountConfiguration : IEntityTypeConfiguration<TaxAccount>
	{
		public void Configure(EntityTypeBuilder<TaxAccount> builder)
		{
			builder
				.Metadata
				.FindNavigation(nameof(TaxAccount.Taxes))
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder
				.HasMany(e => e.Taxes)
				.WithOne(t => t.TaxAccount)
				.HasForeignKey(t => t.TaxAccountId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
