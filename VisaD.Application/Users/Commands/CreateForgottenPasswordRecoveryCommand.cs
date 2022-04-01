using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Application.Emails;
using VisaD.Data.Emails;
using VisaD.Data.Nomenclatures.Constants;
using VisaD.Data.Users;
using VisaD.Data.Users.Enums;

namespace VisaD.Application.Users.Commands
{
	public class CreateForgottenPasswordRecoveryCommand : IRequest<Unit>
	{
		public string Mail { get; set; }

		public class Handler : IRequestHandler<CreateForgottenPasswordRecoveryCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly IEmailService emailService;
			private readonly DomainValidationService validation;
			private readonly AuthConfiguration authConfig;

			public Handler(
				DomainValidationService validation, 
				IAppDbContext context, 
				IEmailService emailService, 
				IOptions<AuthConfiguration> options
				)
			{
				this.context = context;
				this.emailService = emailService;
				this.validation = validation;
				this.authConfig = options.Value;
			}

			public async Task<Unit> Handle(CreateForgottenPasswordRecoveryCommand request, CancellationToken cancellationToken)
			{
				var user = await this.context.Set<User>()
					.AsNoTracking()
					.SingleOrDefaultAsync(e => e.Email.Trim().ToLower() == request.Mail.Trim().ToLower());

				if (user == null)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
				}
				
				if (user.IsLocked || user.Status == UserStatus.Deactivated)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_CannotRestoreUserPassword);
				}

				PasswordToken passwordToken = new PasswordToken(user.Id, 20160);
				this.context.Set<PasswordToken>().Add(passwordToken);

				var payload = new 
				{
					Username = user.Username,
					ForgottenPasswordLink = $"{authConfig.Issuer}/passwordRecovery?token={passwordToken.Value}"
				};

				Email email = await this.emailService.ComposeEmailAsync(EmailTypeAlias.FORGOTTEN_PASSWORD, payload, user.Email);
				this.context.Set<Email>().Add(email);
				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
