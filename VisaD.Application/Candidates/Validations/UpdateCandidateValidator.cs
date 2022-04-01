using FluentValidation;
using VisaD.Application.Candidates.Commands.Entities;

namespace VisaD.Application.Candidates.Validations
{
	public class UpdateCandidateValidator : AbstractValidator<UpdateCandidateCommand>
	{
		public UpdateCandidateValidator()
		{
			RuleFor(a => a.Model.FirstName).NotEmpty().NotNull();
			RuleFor(a => a.Model.LastName).NotEmpty().NotNull();
			RuleFor(a => a.Model.PassportNumber).NotEmpty().NotNull();
			RuleFor(a => a.Model.PassportValidUntil).NotEmpty().NotNull();
			RuleFor(a => a.Model.BirthDate).NotEmpty().NotNull();
			RuleFor(a => a.Model.BirthPlace).NotEmpty().NotNull();
			RuleFor(a => a.Model.Country.Name).NotEmpty().NotNull();
			RuleFor(a => a.Model.Nationality.Name).NotEmpty().NotNull();
			RuleFor(a => a.Model.Phone).NotEmpty().NotNull();
			RuleFor(a => a.Model.Mail).EmailAddress().NotEmpty().NotNull();
			RuleFor(a => a.Model.FirstNameCyrillic).NotEmpty().NotNull();
			RuleFor(a => a.Model.LastNameCyrillic).NotEmpty().NotNull();
			RuleFor(a => a.Model.ImgFile.Size).NotEmpty().NotNull();
		}
	}
}
