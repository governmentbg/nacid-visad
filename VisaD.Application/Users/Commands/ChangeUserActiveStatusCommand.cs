using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Users;
using VisaD.Data.Users.Enums;

namespace VisaD.Application.Users.Commands
{
	public class ChangeUserActiveStatusCommand : IRequest<UserStatus>
	{
		public int Id { get; set; }

		public class Handler : IRequestHandler<ChangeUserActiveStatusCommand, UserStatus>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<UserStatus> Handle(ChangeUserActiveStatusCommand request, CancellationToken cancellationToken)
			{
				var user = await this.context.Set<User>()
					.SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

				if (user.Status == UserStatus.Active)
				{
					user.Status = UserStatus.Deactivated;
				}
				else if (user.Status == UserStatus.Deactivated)
				{
					user.Status = UserStatus.Active;
				}

				await this.context.SaveChangesAsync(cancellationToken);

				return user.Status;
			}
		}
	}
}
