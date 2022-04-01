using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Users;

namespace VisaD.Application.Candidates.Queries
{
	public class GetCandidateLotHistoryQuery : IRequest<CandidateLotHistoryDto>
	{
		public int LotId { get; set; }

		public class Handler : IRequestHandler<GetCandidateLotHistoryQuery, CandidateLotHistoryDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<CandidateLotHistoryDto> Handle(GetCandidateLotHistoryQuery request, CancellationToken cancellationToken)
			{
				var users = context.Set<User>()
					.AsNoTracking()
					.Select(cu => new CreatorUserDto {
						Id = cu.Id,
						Email = cu.Email,
						FirstName = cu.FirstName,
						LastName = cu.LastName
					})
					.AsQueryable();

				var actualCommitId = await this.context.Set<CandidateCommit>()
					.Where(x => x.LotId == request.LotId)
					.OrderByDescending(x => x.Id)
					.Select(x => x.Id)
					.FirstAsync();

				var lot = await context.Set<CandidateLot>()
					.Where(e => e.Id == request.LotId)
					.Select(e => new CandidateLotHistoryDto {
						ActualCommitId = actualCommitId,
						Commits = e.Commits
							.OrderByDescending(c => c.CreateDate)
								.ThenByDescending(c => c.Id)
							.Select(e => new CandidateCommitHistoryItemDto {
								Id = e.Id,
								LotId = e.LotId,
								State = e.State,
								CandidateName = $"{e.CandidatePart.Entity.FirstName} {e.CandidatePart.Entity.LastName}",
								CreateDate = e.CreateDate,
								Number = e.Number,
								Country = e.CandidatePart.Entity.Country.Name,
								BirthDate = e.CandidatePart.Entity.BirthDate,
								CandidateCyrillicName = $"{e.CandidatePart.Entity.FirstNameCyrillic} {e.CandidatePart.Entity.LastNameCyrillic}",
								Mail = e.CandidatePart.Entity.Mail,
								CreatorUser = users.SingleOrDefault(u => u.Id == e.CandidatePart.Entity.CreatorUserId),
								ChangeStateDescription = e.ChangeStateDescription
							})
					})
					.SingleAsync(cancellationToken);

				return lot;
			}
		}
	}
}
