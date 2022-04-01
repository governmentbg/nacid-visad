using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Application.Users.Dtos;
using VisaD.Application.Users.Services;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;
using VisaD.Data.Users.Enums;

namespace VisaD.Application.Users.Commands
{
	public class LoginUserCommand : IRequest<UserLoginInfoDto>
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public class Handler : IRequestHandler<LoginUserCommand, UserLoginInfoDto>
		{
			private readonly IAppDbContext context;
			private readonly IPasswordService passwordService;
			private readonly IMediator mediator;
			private readonly DomainValidationService validation;

			public Handler(
				IAppDbContext context,
				IPasswordService passwordService,
				IMediator mediator,
				DomainValidationService validation
				)
			{
				this.context = context;
				this.passwordService = passwordService;
				this.mediator = mediator;
				this.validation = validation;
			}

			public async Task<UserLoginInfoDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
			{
				var user = await this.context.Set<User>()
					.AsNoTracking()
					.Include(u => u.Role)
					.Include(u => u.Institution)
					.SingleOrDefaultAsync(u => u.Username.Trim() == request.Username.Trim(), cancellationToken);

				if (user == null)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
				}

				if (user.Status == UserStatus.Deactivated || user.IsLocked)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_UserDeactivatedOrLocked);
				}

				bool isSamePassword = this.passwordService.VerifyHashedPassword(user.Password, request.Password, user.PasswordSalt);
				if (!isSamePassword)
				{
					this.validation.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
				}

				var result = new UserLoginInfoDto {
					Fullname = user.FirstName + " " + user.LastName,
					RoleAlias = user.Role.Alias,
					Institution = user.Institution != null
							? new NomenclatureDto<Institution> {
								Id = user.Institution.Id,
								Name = user.Institution.Name
							}
							: null,
					Token = await this.mediator.Send(new CreateLoginTokenCommand { UserId = user.Id, Username = user.Username, OrganizationName = user.Institution?.Name, Role = user.Role.Alias }, cancellationToken)
				};

				return result;
			}
		}
	}
}
