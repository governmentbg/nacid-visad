using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Candidates.Queries
{
	public class SelectCandidateCommitQuery : IRequest<SearchResultItemDto<CandidateCommitDto>>
	{
		public int? CountryId { get; set; }
		public DateTime? BirthDate { get; set; }

		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public class Handler : IRequestHandler<SelectCandidateCommitQuery, SearchResultItemDto<CandidateCommitDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<SearchResultItemDto<CandidateCommitDto>> Handle(SelectCandidateCommitQuery request, CancellationToken cancellationToken)
			{
				var query = context.Set<CandidateCommit>()
					.Where(e => e.State == CommitState.Actual || e.State == CommitState.Modification);

				if (request.BirthDate.HasValue)
				{
					query = query.Where(e => e.CandidatePart.Entity.BirthDate == request.BirthDate);
				}

				if (request.CountryId.HasValue)
				{
					query = query.Where(e => e.CandidatePart.Entity.Country.Id == request.CountryId);
				}

				var items = await query
					.Select(CandidateCommitDto.SelectExpression)
					.OrderBy(e => e.CandidatePart.Entity.FirstName)
						.ThenBy(e => e.Id)
					.Skip(request.Offset)
					.Take(request.Limit)
					.ToListAsync(cancellationToken);

				return new SearchResultItemDto<CandidateCommitDto> {
					Items = items,
					TotalCount = query.Count()
				};
			}
		}
	}
}
