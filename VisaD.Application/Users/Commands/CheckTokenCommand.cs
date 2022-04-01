using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Data.Users;

namespace VisaD.Application.Users.Commands
{
	public class CheckTokenCommand : IRequest<Unit>
	{
		public string Token { get; set; }

		public class Handler : IRequestHandler<CheckTokenCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly DomainValidationService validation;

			public Handler(IAppDbContext context, DomainValidationService validation)
			{
				this.context = context;
				this.validation = validation;
			}

			public async Task<Unit> Handle(CheckTokenCommand request, CancellationToken cancellationToken)
			{
				PasswordToken passwordToken = await this.context.Set<PasswordToken>()
					.SingleOrDefaultAsync(e => e.Value == request.Token, cancellationToken);

				if (passwordToken.IsUsed)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_ActivationTokenAlreadyUsed);
				}

				if (passwordToken.ExpirationTime < DateTime.UtcNow)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_TokenExpired);
				}

				return Unit.Value;
			}
		}
	}
}
