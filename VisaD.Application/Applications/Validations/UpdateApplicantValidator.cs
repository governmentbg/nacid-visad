using FluentValidation;
using VisaD.Application.Applications.Commands.Entities;

namespace VisaD.Application.Applications.Validations
{
	public class UpdateApplicantValidator : AbstractValidator<UpdateApplicantCommand>
	{
		public UpdateApplicantValidator()
		{
			RuleFor(a => a.Model.FirstName).NotEmpty().NotNull();
			RuleFor(a => a.Model.LastName).NotEmpty().NotNull();
			RuleFor(a => a.Model.Position).NotEmpty().NotNull();
			RuleFor(a => a.Model.Mail).EmailAddress().NotEmpty().NotNull();
		}
	}
}
