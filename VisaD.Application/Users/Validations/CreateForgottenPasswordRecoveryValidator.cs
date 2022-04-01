using FluentValidation;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class CreateForgottenPasswordRecoveryValidator : AbstractValidator<CreateForgottenPasswordRecoveryCommand>
	{
		public CreateForgottenPasswordRecoveryValidator()
		{
			RuleFor(e => e.Mail).EmailAddress().NotEmpty().NotNull();
		}
	}
}
