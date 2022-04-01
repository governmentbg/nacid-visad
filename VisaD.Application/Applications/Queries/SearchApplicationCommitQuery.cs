using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Queries
{
	public class SearchApplicationCommitQuery : IRequest<SearchResultItemDto<ApplicationSearchResultItemDto>>
	{
		public string RegisterNumber { get; set; }
		public string Institution { get; set; }
		public int? InstitutionId { get; set; }
		public DateTime? FromDate {get;set;}
		public DateTime? ToDate { get; set; }
		public string SpecialityName { get; set; }
		public int? SpecialityId { get; set; }
		public int? AcademicDegreeId { get; set; }
		public int? FacultyId { get; set; }
		public SearchFilterEndResultType? EndResult { get; set; }

		public string CandidateName { get; set; }
		public string CandidateBirthPlace { get; set; }
		public int? CandidateCountryId { get; set; }

		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public class Handler : IRequestHandler<SearchApplicationCommitQuery, SearchResultItemDto<ApplicationSearchResultItemDto>>
		{
			private readonly IAppDbContext context;
			private const string cyrillycPattern = @"[аАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ, -]+$";

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<SearchResultItemDto<ApplicationSearchResultItemDto>> Handle(SearchApplicationCommitQuery request, CancellationToken cancellationToken)
			{
				var query = context.Set<ApplicationCommit>()
					.Where(e => e.State == CommitState.Actual 
					|| e.State == CommitState.Modification 
					|| e.State == CommitState.Deleted 
					|| e.State == CommitState.Approved
					|| e.State == CommitState.Annulled
					|| e.State == CommitState.RefusedSign);

                if (!string.IsNullOrWhiteSpace(request.RegisterNumber))
                {
					query = query.Where(e => e.Lot.RegisterNumber.Trim().ToLower().Contains(request.RegisterNumber.Trim().ToLower()));
				}

				if (request.InstitutionId.HasValue)
				{
					query = query.Where(e => e.ApplicantPart.Entity.Institution.Id == request.InstitutionId);
				}

				if (!string.IsNullOrWhiteSpace(request.SpecialityName))
				{
					query = query.Where(e => e.EducationPart.Entity.Speciality.Name == request.SpecialityName);
				}

                if (request.AcademicDegreeId.HasValue)
                {
                    query = query.Where(e => e.EducationPart.Entity.EducationalQualification.Id == request.AcademicDegreeId);
                }

                if (request.FacultyId.HasValue)
				{
					query = query.Where(e => e.EducationPart.Entity.Institution.Id == request.FacultyId);
				}

				if (!string.IsNullOrWhiteSpace(request.CandidateName))
				{
					request.CandidateName = Regex.Replace(request.CandidateName, @"\s+", " ").Trim();

					var names = request.CandidateName
							.Split(" ")
							.Select(e => e.ToLower().Trim())
							.ToList();

					Expression<Func<Candidate, bool>> namesExpression = Regex.IsMatch(request.CandidateName, cyrillycPattern)
						? ExpressionHelper.BuildOrStringExpression<Candidate>(nameof(CandidatePart.Entity.FullNameCyrillic), names)
						: ExpressionHelper.BuildOrStringExpression<Candidate>(nameof(CandidatePart.Entity.Fullname), names);

					var innerQuery = context.Set<Candidate>()
							.Where(namesExpression)
							.Select(e => e.Id);
					query = query.Where(e => innerQuery.Contains(e.CandidateCommit.CandidatePart.EntityId));
				}

				if (request.FromDate.HasValue)
				{
					query = query.Where(e => e.CandidateCommit.CandidatePart.Entity.BirthDate >= request.FromDate);
				}

				if (request.ToDate.HasValue)
				{
					query = query.Where(e => e.CandidateCommit.CandidatePart.Entity.BirthDate <= request.ToDate);
				}

				if (!string.IsNullOrWhiteSpace(request.CandidateBirthPlace))
				{
					query = query.Where(e => e.CandidateCommit.CandidatePart.Entity.BirthPlace.Trim().ToLower().Contains(request.CandidateBirthPlace.Trim().ToLower()));
				}

				if (request.CandidateCountryId.HasValue)
				{
					query = query.Where(e => e.CandidateCommit.CandidatePart.Entity.Country.Id == request.CandidateCountryId);
				}

				if (request.EndResult.HasValue)
                {
					switch(request.EndResult)
                    {
						case SearchFilterEndResultType.Certificate:
							query = query.Where(e => e.Lot.Result.Type == ApplicationLotResultType.Certificate && e.Lot.Result.IsSigned);
							break;
						case SearchFilterEndResultType.Rejection:
							query = query.Where(e => e.Lot.Result.Type == ApplicationLotResultType.Rejection && e.Lot.Result.IsSigned);
							break;
						case SearchFilterEndResultType.Actual:
							query = query.Where(e => e.Lot.Result.Type == ApplicationLotResultType.Actual);
							break;
						case SearchFilterEndResultType.Modification:
							query = query.Where(e => e.Lot.Result.Type == ApplicationLotResultType.Modification);
							break;
						case SearchFilterEndResultType.Deleted:
							query = query.Where(e => e.Lot.Result.Type == ApplicationLotResultType.Deleted);
							break;
						case SearchFilterEndResultType.ForSign:
							query = query.Where(e => e.State == CommitState.Approved && !e.Lot.Result.IsSigned);
							break;
						case SearchFilterEndResultType.Annulled:
							query = query.Where(e => e.Lot.Result.Type == ApplicationLotResultType.Annulled);
							break;
						case SearchFilterEndResultType.RefusedSign:
							query = query.Where(e => e.Lot.Result.Type == ApplicationLotResultType.RefusedSign);
							break;
					}
                }

				var items = await query
					.Select(ApplicationSearchResultItemDto.SelectExpression)
					.OrderByDescending(e => e.CommitId)
					.Skip(request.Offset)
					.Take(request.Limit)
					.ToListAsync(cancellationToken);

				return new SearchResultItemDto<ApplicationSearchResultItemDto> {
					Items = items,
					TotalCount = query.Count()
				};
			}
		}
	}
}
