using FluentValidation;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class ChangeUserActiveStatusValidator : AbstractValidator<ChangeUserActiveStatusCommand>
	{
		public ChangeUserActiveStatusValidator()
		{
			RuleFor(u => u.Id).NotEmpty().NotNull();
		}
	}
}
