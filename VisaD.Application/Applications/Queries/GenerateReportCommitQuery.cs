using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Constants;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;

namespace VisaD.Application.Applications.Queries
{
	public class GenerateReportCommitQuery : IRequest<ReportDto>
	{
		public ReportType ReportType { get; set; }
		public DateTime? CreatedReportDate { get; set; }

		public int? InstitutionId { get; set; }
		public string InstitutionName { get; set; }

		public int? SchoolYearId { get; set; }
		public string SchoolYearName { get; set; }
		public SchoolYear SchoolYear { get; set; }

		public int? EducationalQualificationId { get; set; }
		public string EducationalQualificationName { get; set; }

		public int? CountryId { get; set; }
		public string CountryName { get; set; }

		public int? NationalityId { get; set; }
		public string NationalityName { get; set; }

		public string HideHtmlElement { get; set; }

		public class Handler : IRequestHandler<GenerateReportCommitQuery, ReportDto>
		{
			private readonly IAppDbContext context;
			private readonly IUserContext userContext;

			public Handler(IAppDbContext context, IUserContext userContext)
			{
				this.context = context;
				this.userContext = userContext;
			}

			public async Task<ReportDto> Handle(GenerateReportCommitQuery request, CancellationToken cancellationToken)
			{
				var commits = this.context.Set<ApplicationCommit>()
					.Where(c => c.State == CommitState.Actual || c.State == CommitState.Approved || c.State == CommitState.Modification || c.State == CommitState.Annulled)
					.Include(c => c.EducationPart.Entity.EducationalQualification)
					.Include(c => c.ApplicantPart.Entity.Institution)
					.Include(c => c.CandidateCommit.CandidatePart.Entity.Country)
					.Include(c => c.CandidateCommit.CandidatePart.Entity.Nationality)
					.Include(c => c.Lot.Result)
					.AsQueryable();

				if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
				{
					var userInstitutionId = await this.context.Set<User>()
					.Where(x => x.Id == this.userContext.UserId)
					.Select(x => x.InstitutionId)
					.SingleAsync();

					commits = commits.Where(c => c.ApplicantPart.Entity.InstitutionId == userInstitutionId);
				}

				if (request.InstitutionId.HasValue)
				{
					commits = commits.Where(c => c.ApplicantPart.Entity.InstitutionId == request.InstitutionId);
				}

				if (request.EducationalQualificationId.HasValue)
				{
					commits = commits.Where(c => c.EducationPart.Entity.EducationalQualificationId == request.EducationalQualificationId);
				}

				if (request.SchoolYearId.HasValue && request.ReportType != ReportType.ReportByCandidateWithMoreThanOneCertificate && request.SchoolYearName != "Всички години")
				{
					commits = commits.Where(c => c.EducationPart.Entity.SchoolYearId == request.SchoolYearId);
				}

				if (request.NationalityId.HasValue)
				{
					commits = commits.Where(c => c.CandidateCommit.CandidatePart.Entity.NationalityId == request.NationalityId);
				}

				if (request.CountryId.HasValue)
				{
					commits = commits.Where(c => c.CandidateCommit.CandidatePart.Entity.CountryId == request.CountryId);
				}

				var report = new ReportDto {
					ReportType = request.ReportType,
				};

				var materializedCommits = commits.ToList();

				if (request.ReportType == ReportType.ReportByInstitution)
				{
					var groupedCommits = materializedCommits
						.GroupBy(c => c.ApplicantPart.Entity.Institution.Name)
						.Select(c => c.First())
						.ToList();

					foreach (var commit in groupedCommits)
					{
						var filteredCommits = commits.Where(c => c.ApplicantPart.Entity.InstitutionId == commit.ApplicantPart.Entity.InstitutionId);

						var reportItem = new ReportItemDto {
							Institution = commit.ApplicantPart.Entity.Institution.Name,
							ModificationCommitsCount = filteredCommits.Where(c => c.State == CommitState.Modification).Count(),
							UnsignedCommitsCount = filteredCommits.Where(c => c.State == CommitState.Approved && c.Lot.Result.IsSigned == false).Count(),
							PendingCommitsCount = filteredCommits.Where(c => c.State == CommitState.Actual).Count(),
							CertificateCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Certificate && c.Lot.Result.IsSigned == true).Count(),
							RejectedCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Rejection && c.Lot.Result.IsSigned == true).Count(),
							AnnulledCommitsCount = filteredCommits.Where(c => c.State == CommitState.Annulled).Count(),
							HideHtmlElement = this.userContext.Role == UserRoleAliases.UNIVERSITY_USER ? "hidden" : ""
						};

						report.Reports.Add(reportItem);
					}
				}
				else if (request.ReportType == ReportType.ReportByNationality)
				{
					var groupedCommits = materializedCommits
						.GroupBy(c => c.CandidateCommit.CandidatePart.Entity.Nationality.Name)
						.Select(c => c.First())
						.ToList();

					foreach (var commit in groupedCommits)
					{
						var filteredCommits = commits.Where(c => c.CandidateCommit.CandidatePart.Entity.Nationality.Id == commit.CandidateCommit.CandidatePart.Entity.Nationality.Id);

						var reportItem = new ReportItemDto {
							Nationality = commit.CandidateCommit.CandidatePart.Entity.Nationality.Name,
							ModificationCommitsCount = filteredCommits.Where(c => c.State == CommitState.Modification).Count(),
							UnsignedCommitsCount = filteredCommits.Where(c => c.State == CommitState.Approved && c.Lot.Result.IsSigned == false).Count(),
							PendingCommitsCount = filteredCommits.Where(c => c.State == CommitState.Actual).Count(),
							CertificateCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Certificate && c.Lot.Result.IsSigned == true).Count(),
							RejectedCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Rejection && c.Lot.Result.IsSigned == true).Count(),
							AnnulledCommitsCount = filteredCommits.Where(c => c.State == CommitState.Annulled).Count(),
							HideHtmlElement = this.userContext.Role == UserRoleAliases.UNIVERSITY_USER ? "hidden" : ""
						};

						report.Reports.Add(reportItem);
					}
				}
				else if (request.ReportType == ReportType.ReportByCountry)
				{
					var groupedCommits = materializedCommits
						.GroupBy(c => c.CandidateCommit.CandidatePart.Entity.Country.Name)
						.Select(c => c.First())
						.ToList();

					foreach (var commit in groupedCommits)
					{
						var filteredCommits = commits.Where(c => c.CandidateCommit.CandidatePart.Entity.Country.Id == commit.CandidateCommit.CandidatePart.Entity.Country.Id);

						var reportItem = new ReportItemDto {
							Country = commit.CandidateCommit.CandidatePart.Entity.Country.Name,
							ModificationCommitsCount = filteredCommits.Where(c => c.State == CommitState.Modification).Count(),
							UnsignedCommitsCount = filteredCommits.Where(c => c.State == CommitState.Approved && c.Lot.Result.IsSigned == false).Count(),
							PendingCommitsCount = filteredCommits.Where(c => c.State == CommitState.Actual).Count(),
							CertificateCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Certificate && c.Lot.Result.IsSigned == true).Count(),
							RejectedCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Rejection && c.Lot.Result.IsSigned == true).Count(),
							AnnulledCommitsCount = filteredCommits.Where(c => c.State == CommitState.Annulled).Count(),
							HideHtmlElement = this.userContext.Role == UserRoleAliases.UNIVERSITY_USER ? "hidden" : ""
						};

						report.Reports.Add(reportItem);
					}
				}
				else if (request.ReportType == ReportType.ReportByEducationalQualification)
				{
					var groupedCommits = materializedCommits
						.GroupBy(c => c.EducationPart.Entity.EducationalQualification.Name)
						.Select(c => c.First())
						.ToList();

					foreach (var commit in groupedCommits)
					{
						var filteredCommits = commits.Where(c => c.EducationPart.Entity.EducationalQualification.Id == commit.EducationPart.Entity.EducationalQualification.Id);

						var reportItem = new ReportItemDto {
							EducationalQualification = commit.EducationPart.Entity.EducationalQualification.Name,
							ModificationCommitsCount = filteredCommits.Where(c => c.State == CommitState.Modification).Count(),
							UnsignedCommitsCount = filteredCommits.Where(c => c.State == CommitState.Approved && c.Lot.Result.IsSigned == false).Count(),
							PendingCommitsCount = filteredCommits.Where(c => c.State == CommitState.Actual).Count(),
							CertificateCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Certificate && c.Lot.Result.IsSigned == true).Count(),
							RejectedCommitsCount = filteredCommits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Rejection && c.Lot.Result.IsSigned == true).Count(),
							AnnulledCommitsCount = filteredCommits.Where(c => c.State == CommitState.Annulled).Count(),
							HideHtmlElement = this.userContext.Role == UserRoleAliases.UNIVERSITY_USER ? "hidden" : ""
						};

						report.Reports.Add(reportItem);
					}
				}
				else if (request.ReportType == ReportType.DefaultReport)
				{
					var reportItem = new ReportItemDto {
						ModificationCommitsCount = commits.Where(c => c.State == CommitState.Modification).Count(),
						UnsignedCommitsCount = commits.Where(c => c.State == CommitState.Approved && c.Lot.Result.IsSigned == false).Count(),
						PendingCommitsCount = commits.Where(c => c.State == CommitState.Actual).Count(),
						CertificateCommitsCount = commits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Certificate && c.Lot.Result.IsSigned == true).Count(),
						RejectedCommitsCount = commits.Where(c => c.Lot.Result.Type == ApplicationLotResultType.Rejection && c.Lot.Result.IsSigned == true).Count(),
						AnnulledCommitsCount = commits.Where(c => c.State == CommitState.Annulled).Count(),
						HideHtmlElement = this.userContext.Role == UserRoleAliases.UNIVERSITY_USER ? "hidden" : ""
					};

					report.Reports.Add(reportItem);
				}
				else if (request.ReportType == ReportType.ReportByCandidateWithMoreThanOneCertificate) 
				{
					var applicationCandidates = materializedCommits
						.GroupBy(c => c.CandidateCommit.LotId)
						.Select(c => c.Last())
						.ToList();

					foreach (var candidate in applicationCandidates)
					{
						var filtered = commits.Where(c => c.CandidateCommit.LotId == candidate.CandidateCommit.LotId && c.Lot.Result.Type == ApplicationLotResultType.Certificate);

						if (filtered.Count() > 1)
						{
							var reportItem = new ReportItemDto {
								CandidateLatinName = candidate.CandidateCommit.CandidatePart.Entity.Fullname,
								CandidateCiryllicName = candidate.CandidateCommit.CandidatePart.Entity.FullNameCyrillic,
								CandidateCountry = candidate.CandidateCommit.CandidatePart.Entity.Country.Name,
								CandidateBirthPlace = candidate.CandidateCommit.CandidatePart.Entity.BirthPlace,
								CandidateBirthDate = candidate.CandidateCommit.CandidatePart.Entity.BirthDate,
								CandidateNationality = candidate.CandidateCommit.CandidatePart.Entity.Nationality.Name,
								CandidateCertficatesCount = filtered.Count(),
								FormatedCandidateBirthDate = candidate.CandidateCommit.CandidatePart.Entity.BirthDate.ToString("dd/MM/yyyy"),
								ConcatenatedCandidateNames = candidate.CandidateCommit.CandidatePart.Entity.Fullname + ", " + candidate.CandidateCommit.CandidatePart.Entity.FullNameCyrillic,
								ConcatenatedCandidateBirthInfo = candidate.CandidateCommit.CandidatePart.Entity.Country.Name + ", " + candidate.CandidateCommit.CandidatePart.Entity.BirthPlace + ", " + candidate.CandidateCommit.CandidatePart.Entity.BirthDate.ToString("dd/MM/yyyy")
							};

							report.Reports.Add(reportItem);
						}
					}
				}

				return report;
			}
		}
	}
}
