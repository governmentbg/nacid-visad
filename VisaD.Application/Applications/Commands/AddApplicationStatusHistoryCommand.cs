using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Users;

namespace VisaD.Application.Applications.Commands
{
	public class AddApplicationStatusHistoryCommand : IRequest<Unit>
	{
		public int LotId { get; set; }

		public int? CommitId { get; set; }

		public string CreatorUser { get; set; }

		public CommitState State { get; set; }

		public string ChangeStateDescription { get; set; }

		public string CandidateName { get; set; }

		public string RegisterNumber { get; set; }

		public DateTime CandidateBirthDate { get; set; }

		public string CandidateCountry { get; set; }

		public ApplicationLotResultType? LotResultType { get; set; }

		public class Handler : IRequestHandler<AddApplicationStatusHistoryCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly IUserContext userContext;

			public Handler(IAppDbContext context, IUserContext userContext)
			{
				this.context = context;
				this.userContext = userContext;
			}

			public async Task<Unit> Handle(AddApplicationStatusHistoryCommand request, CancellationToken cancellationToken)
			{
				var user = await this.context.Set<User>()
					.SingleAsync(x => x.Id == this.userContext.UserId);
				if (request.State == CommitState.Approved || request.State == CommitState.Deleted)
				{
					var lastActualCommit = await this.context.Set<ApplicationStatusHistory>()
						.Where(x => x.LotId == request.LotId && (x.CommitState == CommitState.Actual || x.CommitState == CommitState.RefusedSign))
						.OrderByDescending(x => x.Id)
						.FirstOrDefaultAsync();

					if (lastActualCommit != null)
					{
						lastActualCommit.CommitId = null;
					}
				}
				else if (request.State == CommitState.Actual)
				{
					var lastCommit = await this.context.Set<ApplicationStatusHistory>()
						.Where(x => x.LotId == request.LotId && x.CommitState == CommitState.Deleted)
						.OrderByDescending(x => x.Id)
						.FirstOrDefaultAsync();

					if (lastCommit != null)
					{
						lastCommit.CommitId = null;
					}
				}
				else if (request.State == CommitState.Annulled)
				{
					var lastCommit = await this.context.Set<ApplicationStatusHistory>()
						.Where(x => x.LotId == request.LotId && (x.ApplicationLotResultType.Value == ApplicationLotResultType.Certificate || x.ApplicationLotResultType == ApplicationLotResultType.Rejection))
						.OrderByDescending(x => x.Id)
						.FirstOrDefaultAsync();

					if (lastCommit != null)
					{
						lastCommit.CommitId = null;
					}
				}
				else if (request.State == CommitState.RefusedSign)
				{
					var lastCommit = await this.context.Set<ApplicationStatusHistory>()
						.Where(x => x.LotId == request.LotId && (x.ApplicationLotResultType.Value == ApplicationLotResultType.Certificate || x.ApplicationLotResultType == ApplicationLotResultType.Rejection))
						.OrderByDescending(x => x.Id)
						.FirstOrDefaultAsync();

					if (lastCommit != null)
					{
						lastCommit.CommitId = null;
					}
				}

				var applicationStatusHistory = new ApplicationStatusHistory(request.LotId, request.CommitId, $"{user.FirstName} {user.LastName}", DateTime.Now, request.State, 
					request.ChangeStateDescription, request.CandidateName, request.RegisterNumber, request.CandidateBirthDate, request.CandidateCountry, request.LotResultType);

				this.context.Set<ApplicationStatusHistory>().Add(applicationStatusHistory);
				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
