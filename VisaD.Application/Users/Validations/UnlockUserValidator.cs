using FluentValidation;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class UnlockUserValidator : AbstractValidator<UnlockUserCommand>
	{
		public UnlockUserValidator()
		{
			RuleFor(u => u.Token).NotEmpty().NotNull();
			RuleFor(u => u.Password).NotEmpty().NotNull();
		}
	}
}
