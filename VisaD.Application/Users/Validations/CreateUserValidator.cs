using FluentValidation;
using VisaD.Application.Common.Constants;
using VisaD.Application.Users.Commands;

namespace VisaD.Application.Users.Validations
{
	public class CreateUserValidator : AbstractValidator<CreateUserCommand>
	{
		public CreateUserValidator()
		{
			RuleFor(e => e.Username).NotEmpty().NotNull();
			RuleFor(e => e.FirstName).NotEmpty().NotNull();
			RuleFor(e => e.LastName).NotEmpty().NotNull();
			RuleFor(e => e.Email).EmailAddress().NotNull();
			RuleFor(e => e.RoleId).NotEmpty().NotNull();
			RuleFor(e => e.Institution.Name).NotEmpty().NotNull().When(e => e.RoleAlias == UserRoleAliases.UNIVERSITY_USER);
		}
	}
}
