using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Application.Users.Dtos;
using VisaD.Data.Users;

namespace VisaD.Application.Users.Commands
{
	public class EditUserDataCommand : IRequest<Unit>
	{
		public UserEditDto User { get; set; }

		public class Handler : IRequestHandler<EditUserDataCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly DomainValidationService validationService;

			public Handler(IAppDbContext context, DomainValidationService validationService)
			{
				this.context = context;
				this.validationService = validationService;
			}

			public async Task<Unit> Handle(EditUserDataCommand request, CancellationToken cancellationToken)
			{
				var user = await this.context.Set<User>()
					.SingleOrDefaultAsync(e => e.Id == request.User.Id);

				if (user == null)
				{
					this.validationService.ThrowErrorMessage(UserErrorCode.User_NotFound);
				}

				bool isEmailTaken = await context.Set<User>()
					.AnyAsync(e => e.Email.Trim().ToLower() == request.User.Email.Trim().ToLower() && e.Id != request.User.Id, cancellationToken);

				if (isEmailTaken)
				{
					this.validationService.ThrowErrorMessage(UserErrorCode.User_EmailTaken);
				}

				user.Update(request.User.Email, request.User.Email, request.User.Phone, request.User.FirstName, request.User.MiddleName, request.User.LastName, 
					request.User.Institution?.Id, request.User.Role.Id, request.User.Position);
				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
