using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Candidates.Queries;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Queries
{
	public class SearchCandidateCommitQuery : IRequest<SearchResultItemDto<CandidateSearchResultItemDto>>
	{
		public string Name { get; set; }
		public int? CountryId { get; set; }
		public string BirthPlace { get; set; }
		public DateTime? BirthDateFrom { get; set; }
		public DateTime? BirthDateTo { get; set; }
		public int? NationalityId { get; set; }
		public string Mail { get; set; }
		public string Phone { get; set; }
		public string PassportNumber { get; set; }

		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public class Handler : IRequestHandler<SearchCandidateCommitQuery, SearchResultItemDto<CandidateSearchResultItemDto>>
		{
			private readonly IAppDbContext context;
			private readonly IMediator mediator;
			private const string cyrillycPattern = @"[аАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ, -]+$";

			public Handler(IAppDbContext context, IMediator mediator)
			{
				this.context = context;
				this.mediator = mediator;
			}

			public async Task<SearchResultItemDto<CandidateSearchResultItemDto>> Handle(SearchCandidateCommitQuery request, CancellationToken cancellationToken)
			{
				var query = context.Set<CandidateCommit>()
					.Include(e => e.CandidatePart.Entity.OtherNationalities)
					.Where(e => e.State == CommitState.InitialDraft || e.State == CommitState.CommitReady || e.State == CommitState.Actual || e.State == CommitState.Modification || e.State == CommitState.Deleted);

				if (!string.IsNullOrWhiteSpace(request.Name))
				{
					request.Name = Regex.Replace(request.Name, @"\s+", " ").Trim();

					var names = request.Name
							.Split(" ")
							.Select(e => e.ToLower().Trim())
							.ToList();

					Expression<Func<Candidate, bool>> namesExpression = Regex.IsMatch(request.Name, cyrillycPattern)
						? ExpressionHelper.BuildOrStringExpression<Candidate>(nameof(CandidatePart.Entity.FullNameCyrillic), names)
						: ExpressionHelper.BuildOrStringExpression<Candidate>(nameof(CandidatePart.Entity.Fullname), names);
					
					var innerQuery = context.Set<Candidate>()
							.Where(namesExpression)
							.Select(e => e.Id);
					query = query.Where(e => innerQuery.Contains(e.CandidatePart.EntityId));
				}

				if (request.BirthDateFrom.HasValue)
				{
					query = query.Where(e => e.CandidatePart.Entity.BirthDate >= request.BirthDateFrom);
				}

				if (request.BirthDateTo.HasValue)
				{
					query = query.Where(e => e.CandidatePart.Entity.BirthDate <= request.BirthDateTo);
				}

				if (!string.IsNullOrWhiteSpace(request.BirthPlace))
				{
					query = query.Where(e => e.CandidatePart.Entity.BirthPlace.Trim().ToLower().Contains(request.BirthPlace.Trim().ToLower()));
				}

				if (request.CountryId.HasValue)
				{
					query = query.Where(e => e.CandidatePart.Entity.Country.Id == request.CountryId);
				}

				if (request.NationalityId.HasValue)
				{
					query = query.Where(e => e.CandidatePart.Entity.Nationality.Id == request.NationalityId || e.CandidatePart.Entity.OtherNationalities.Any(x => x.NationalityId == request.NationalityId));
				}

				if (!string.IsNullOrWhiteSpace(request.PassportNumber))
				{
					query = query.Where(e => e.CandidatePart.Entity.PassportNumber.Trim().ToLower().Contains(request.PassportNumber.Trim().ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(request.Mail))
				{
					query = query.Where(e => e.CandidatePart.Entity.Mail.Trim().ToLower().Contains(request.Mail.Trim().ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(request.Phone))
				{
					query = query.Where(e => e.CandidatePart.Entity.Phone.Trim().ToLower().Contains(request.Phone.Trim().ToLower()));
				}

				var items = await query
					.Select(CandidateSearchResultItemDto.SelectExpression)
					.OrderByDescending(e => e.LotId)
						.ThenBy(e => e.CommitId)
					.Skip(request.Offset)
					.Take(request.Limit)
					.ToListAsync(cancellationToken);

				foreach (var item in items)
				{
					item.ApplicationsCount = this.mediator.Send(new GetCandidateApplicationsQuery { CandidateLotId = item.LotId, CandidateCommitId = item.CommitId, CandidateCommitState = item.State }, cancellationToken).Result.Count();
				}

				return new SearchResultItemDto<CandidateSearchResultItemDto> {
					Items = items,
					TotalCount = query.Count()
				};
			}
		}
	}
}
