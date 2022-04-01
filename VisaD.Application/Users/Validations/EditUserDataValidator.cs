using FluentValidation;
using VisaD.Application.Common.Constants;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class EditUserDataValidator : AbstractValidator<EditUserDataCommand>
	{
		public EditUserDataValidator()
		{
			RuleFor(u => u.User.Id).NotEmpty().NotNull();
			RuleFor(u => u.User.Username).NotEmpty().NotNull();
			RuleFor(u => u.User.FirstName).NotEmpty().NotNull();
			RuleFor(u => u.User.LastName).NotEmpty().NotNull();
			RuleFor(u => u.User.Email).EmailAddress().NotEmpty().NotNull();
			RuleFor(u => u.User.Institution.Name).NotEmpty().NotNull().When(e => e.User.Role.Alias == UserRoleAliases.UNIVERSITY_USER);
			RuleFor(u => u.User.Role.Name).NotEmpty().NotNull();
		}
	}
}
