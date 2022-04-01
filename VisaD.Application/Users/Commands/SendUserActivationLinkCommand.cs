using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
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

namespace VisaD.Application.Users.Commands
{
	public class SendUserActivationLinkCommand : IRequest<Unit>
	{
		public int Id { get; set; }

		public class Handler : IRequestHandler<SendUserActivationLinkCommand, Unit>
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
				this.validation = validation;
				this.context = context;
				this.emailService = emailService;
				this.authConfig = options.Value;
			}

			public async Task<Unit> Handle(SendUserActivationLinkCommand request, CancellationToken cancellationToken)
			{
				var user = await this.context.Set<User>()
					.AsNoTracking()
					.Include(e => e.Role)
					.SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

				if(!user.IsLocked)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_UserAlreadyUnlocked);
				}

				var oldTokens = await this.context.Set<PasswordToken>()
					.Where(e => !e.IsUsed && e.UserId == request.Id)
					.ToListAsync(cancellationToken);

				foreach (var oldToken in oldTokens)
				{
					oldToken.Use();
				}

				await this.context.SaveChangesAsync(cancellationToken);

				PasswordToken passwordToken = new PasswordToken(user.Id, 20160);
				this.context.Set<PasswordToken>().Add(passwordToken);

				var payload = new {
					FullName = user.FirstName + " " + user.LastName,
					Role = user.Role.Name,
					ActivationLink = $"{this.authConfig.Issuer}/user/activation?token={passwordToken.Value}"
				};

				Email email = await this.emailService.ComposeEmailAsync(EmailTypeAlias.USER_ACTIVATION, payload, user.Email);
				this.context.Set<Email>().Add(email);

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
