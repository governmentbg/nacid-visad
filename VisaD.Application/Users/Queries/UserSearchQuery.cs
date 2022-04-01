using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Users.Dtos;
using VisaD.Data.Users;
using VisaD.Data.Users.Enums;

namespace VisaD.Application.Users.Queries
{
	public class UserSearchQuery : IRequest<SearchResultItemDto<UserSearchResultDto>>
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? RoleId { get; set; }
		public int? InstitutionId { get; set; }
		public UserStatus? Status { get; set; }

		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public class Handler : IRequestHandler<UserSearchQuery, SearchResultItemDto<UserSearchResultDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<SearchResultItemDto<UserSearchResultDto>> Handle(UserSearchQuery request, CancellationToken cancellationToken)
			{
				IQueryable<User> query = context.Set<User>();

				if (!string.IsNullOrWhiteSpace(request.FirstName))
				{
					query = query.Where(e => e.FirstName.Trim().ToLower().Contains(request.FirstName.Trim().ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(request.MiddleName))
				{
					query = query.Where(e => e.MiddleName.Trim().ToLower().Contains(request.MiddleName.Trim().ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(request.LastName))
				{
					query = query.Where(e => e.LastName.Trim().ToLower().Contains(request.LastName.Trim().ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(request.Username))
				{
					query = query.Where(e => e.Username.Trim().ToLower().Contains(request.Username.Trim().ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(request.Email))
				{
					query = query.Where(e => e.Email.Trim().ToLower().Contains(request.Email.Trim().ToLower()));
				}

				if (request.RoleId.HasValue)
				{
					query = query.Where(e => e.RoleId == request.RoleId.Value);
				}

				if (request.InstitutionId.HasValue)
				{
					query = query.Where(e => e.InstitutionId == request.InstitutionId.Value);
				}

				if (request.Status.HasValue)
				{
					switch (request.Status)
					{
						case UserStatus.Active:
							query = query.Where(e => e.Status == UserStatus.Active);
							break;
						case UserStatus.Deactivated:
							query = query.Where(e => e.Status == UserStatus.Deactivated);
							break;
						case UserStatus.Inactive:
							query = query.Where(e => e.Status == UserStatus.Inactive);
							break;
					}
				}

				var result = await query
					.Select(UserSearchResultDto.SelectExpression)
					.OrderByDescending(e => e.Id)
					.Skip(request.Offset)
					.Take(request.Limit)
					.ToListAsync(cancellationToken);

				return new SearchResultItemDto<UserSearchResultDto> {
					Items = result,
					TotalCount = query.Count()
				};
			}
		}
	}
}
