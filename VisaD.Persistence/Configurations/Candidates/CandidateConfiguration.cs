using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisaD.Data.Candidates;

namespace VisaD.Persistence.Configurations.Candidates
{
	public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
	{
		public void Configure(EntityTypeBuilder<Candidate> builder)
		{
			builder
				.HasMany(e => e.OtherNationalities)
				.WithOne(t => t.Candidate)
				.HasForeignKey(t => t.CandidateId)
				.OnDelete(DeleteBehavior.Cascade);

			builder
				.HasOne(e => e.CandidatePassportDocument)
				.WithOne(t => t.Candidate)
				.HasForeignKey<CandidatePassportDocument>(b => b.CandidateId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

	public class CandidateNationalityConfiguration : IEntityTypeConfiguration<CandidateNationality>
	{
		public void Configure(EntityTypeBuilder<CandidateNationality> builder)
		{
			builder.HasKey(e => e.Id);
		}
	}

	public class CandidatePassportDocumentConfiguration : IEntityTypeConfiguration<CandidatePassportDocument>
	{
		public void Configure(EntityTypeBuilder<CandidatePassportDocument> builder)
		{
			builder.HasKey(e => e.Id);
		}
	}
}
