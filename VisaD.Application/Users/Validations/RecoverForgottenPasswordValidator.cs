using FluentValidation;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class RecoverForgottenPasswordValidator : AbstractValidator<RecoverForgottenPasswordCommand>
	{
		public RecoverForgottenPasswordValidator()
		{
			RuleFor(u => u.Token).NotEmpty().NotNull();
			RuleFor(u => u.NewPassword).NotEmpty().NotNull();
			RuleFor(u => u.NewPasswordAgain).NotEmpty().NotNull().Equal(u => u.NewPassword);
		}
	}
}
