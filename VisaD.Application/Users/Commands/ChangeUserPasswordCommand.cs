using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Application.Users.Services;
using VisaD.Data.Users;

namespace VisaD.Application.Users.Commands
{
	public class ChangeUserPasswordCommand : IRequest<Unit>
	{
		public string OldPassword { get; set; }

		public string NewPassword { get; set; }

		public string NewPasswordAgain { get; set; }

		public class Handler : IRequestHandler<ChangeUserPasswordCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly IPasswordService passwordService;
			private readonly DomainValidationService validation;
			private readonly IUserContext userContext;

			public Handler(IAppDbContext context, IPasswordService passwordService, DomainValidationService validation, IUserContext userContext)
			{
				this.context = context;
				this.passwordService = passwordService;
				this.validation = validation;
				this.userContext = userContext;
			}

			public async Task<Unit> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
			{
				if (request.NewPassword != request.NewPasswordAgain)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_ChangePasswordNewPasswordMismatch);
				}

				var user = await this.context.Set<User>()
					.SingleAsync(e => e.Id== this.userContext.UserId, cancellationToken);

				if (!this.passwordService.VerifyHashedPassword(user.Password, request.OldPassword, user.PasswordSalt))
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_ChangePasswordOldPasswordMismatch);
				}

				string newPasswordSalt = this.passwordService.GenerateSalt(128);
				string newPasswordHash = this.passwordService.HashPassword(request.NewPassword, newPasswordSalt);
				user.ChangePassword(newPasswordHash, newPasswordSalt);

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
