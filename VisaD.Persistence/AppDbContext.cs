using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Emails;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;

namespace VisaD.Persistence
{
	public class AppDbContext : DbContext, IAppDbContext
	{
		private readonly IUserContext userContext;

		//public AppDbContext(DbContextOptions<AppDbContext> options)
		//	: base(options)
		//{
		//}

		public AppDbContext(
			IUserContext userContext,
			DbContextOptions<AppDbContext> options)
			: base(options)
		{
			this.userContext = userContext;
		}

		#region Applications
		public DbSet<Applicant> Applicants { get; set; }
		public DbSet<Education> Educations { get; set; }
		public DbSet<Diploma> Diplomas { get; set; }
		public DbSet<Training> Trainings { get; set; }
		public DbSet<LanguageProficiency> LanguageProficiencies { get; set; }
		public DbSet<SpecialTraining> SpecialTrainings { get; set; }

		public DbSet<TaxAccount> TaxAccounts { get; set; }
		public DbSet<Tax> Taxes { get; set; }
		public DbSet<PreviousApplication> PreviousApplications { get; set; }
		public DbSet<MedicalCertificate> MedicalCertificates { get; set; }

		public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }
		public DbSet<ApplicationDraft> ApplicationDrafts { get; set; }

		#endregion

		#region Candidates

		public DbSet<Candidate> Candidates { get; set; }
		public DbSet<CandidateNationality> CandidateNationalities {get; set;}
		public DbSet<CandidatePassportDocument> CandidatePassportDocuments { get; set; }

		#endregion

		#region Nomenclatures

		public DbSet<DurationType> DurationTypes { get; set; }
		public DbSet<CurrencyType> CurrencyTypes { get; set; }
		public DbSet<ApplicationFileType> ApplicationFileTypes { get; set; }
		public DbSet<EducationFormType> EducationFormTypes { get; set; }
		public DbSet<TrainingType> TrainingTypes { get; set; }
		public DbSet<EmailType> EmailTypes { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<LanguageDegree> LanguageDegrees { get; set; }
		public DbSet<Institution> Institutions { get; set; }
		public DbSet<Speciality> Specialties { get; set; }
		public DbSet<InstitutionSpeciality> InstitutionSpecialities { get; set; }
		public DbSet<EducationalQualification> EducationalQualifications { get; set; }
		public DbSet<SchoolYear> SchoolYears { get; set; }
		public DbSet<DiplomaType> DiplomaTypes { get; set; }

		public DbSet<RegisterIndex> RegisterIndices { get; set; }
		public DbSet<RegisterIndexCounter> RegisterIndexCounters { get; set; }
		public DbSet<FileTemplate> FileTemplates { get; set; }

		public DbSet<Language> Languages { get; set; }
		public DbSet<Bank> Banks { get; set; }

		public DbSet<Regulation> Regulations { get; set; }
		public DbSet<InstitutionOwnershipType> InstitutionOwnershipTypes { get; set; }

		#endregion

		#region Register applications

		public DbSet<ApplicationLot> ApplicationLots { get; set; }
		public DbSet<ApplicationLotResult> ApplicationLotResults { get; set; }
		public DbSet<ApplicationLotResultFile> ApplicationLotResultFiles { get; set; }
		public DbSet<ApplicationCommit> ApplicationCommits { get; set; }
		public DbSet<ApplicantPart> ApplicantParts { get; set; }
		public DbSet<DiplomaPart> DiplomaParts { get; set; }
		public DbSet<EducationPart> EducationParts { get; set; }
		public DbSet<TrainingPart> TrainingParts { get; set; }
		public DbSet<TaxAccountPart> TaxAccountParts { get; set; }
		public DbSet<RepresentativePart> RepresentativeParts { get; set; }
		public DbSet<PreviousApplicationPart> PreviousApplicationParts { get; set; }
		public DbSet<MedicalCertificatePart> MedicalCertificateParts { get; set; }


		#endregion

		#region Register candidates

		public DbSet<CandidateLot> CandidateLots { get; set; }
		public DbSet<CandidateCommit> CandidateCommits { get; set; }
		public DbSet<CandidatePart> CandidateParts { get; set; }

		#endregion

		#region Users

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<PasswordToken> PasswordTokens { get; set; }
		public DbSet<Email> Emails { get; set; }
		public DbSet<EmailAddressee> EmailAddressees { get; set; }

		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				// Configure name mappings
				entity.SetTableName(entity.ClrType.Name.ToLower());

				if (typeof(IEntity).IsAssignableFrom(entity.ClrType))
				{
					modelBuilder.Entity(entity.ClrType)
						.HasKey(nameof(IEntity.Id));
				}

				if (typeof(IConcurrency).IsAssignableFrom(entity.ClrType))
				{
					modelBuilder.Entity(entity.ClrType)
						.Property(nameof(IConcurrency.Version))
						.IsConcurrencyToken()
						.HasDefaultValue(0);
				}

				entity.GetProperties()
					.ToList()
					.ForEach(e => e.SetColumnName(e.Name.ToLower()));

				entity.GetForeignKeys()
					.Where(e => !e.IsOwnership && e.DeleteBehavior == DeleteBehavior.Cascade)
					.ToList()
					.ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);
			}

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}

		public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
		{
			return Database.BeginTransactionAsync(cancellationToken);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			foreach (var entry in ChangeTracker.Entries())
			{
				if (typeof(IAuditable).IsAssignableFrom(entry.Entity.GetType()) && entry.State == EntityState.Added)
				{
					var entity = entry.Entity as IAuditable;
					entity.CreateDate = DateTime.Now;
					entity.CreatorUserId = this.userContext.UserId;
				}

				if (typeof(IConcurrency).IsAssignableFrom(entry.Entity.GetType()) && entry.State == EntityState.Modified)
				{
					var entity = entry.Entity as IConcurrency;
					entity.Version++;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
