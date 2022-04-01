using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Application.Users.Services;
using VisaD.Data.Users;

namespace VisaD.Application.Users.Commands
{
	public class UnlockUserCommand : IRequest<Unit>
	{
		public string Token { get; set; }

		public string Password { get; set; }

		public class Handler : IRequestHandler<UnlockUserCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly DomainValidationService validation;
			private readonly IPasswordService passwordService;

			public Handler(IAppDbContext context, DomainValidationService validation, IPasswordService passwordService)
			{
				this.context = context;
				this.validation = validation;
				this.passwordService = passwordService;
			}

			public async Task<Unit> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
			{
				PasswordToken passwordToken = await this.context.Set<PasswordToken>()
					.Include(e => e.User)
					.SingleOrDefaultAsync(e => e.Value == request.Token, cancellationToken);

				if (passwordToken.IsUsed)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_ActivationTokenAlreadyUsed);
				}

				if (passwordToken.ExpirationTime < DateTime.UtcNow)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_ActivationTokenExpired);
				}

				passwordToken.Use();

				string salt = this.passwordService.GenerateSalt(128);
				string hash = this.passwordService.HashPassword(request.Password, salt);
				passwordToken.User.Activate(hash, salt);

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
