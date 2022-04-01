using FluentValidation;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class LoginUserValidator : AbstractValidator<LoginUserCommand>
	{
		public LoginUserValidator()
		{
			RuleFor(u => u.Username).NotEmpty().NotNull();
			RuleFor(u => u.Password).NotEmpty().NotNull();
		}
	}
}
