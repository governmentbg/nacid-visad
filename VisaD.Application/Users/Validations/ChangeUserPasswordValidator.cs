using FluentValidation;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
	{
		public ChangeUserPasswordValidator()
		{
			RuleFor(u => u.OldPassword).NotEmpty().NotNull();
			RuleFor(u => u.NewPassword).NotEmpty().NotNull();
			RuleFor(u => u.NewPasswordAgain).NotEmpty().NotNull().Equal(u => u.NewPassword);
		}
	}
}
