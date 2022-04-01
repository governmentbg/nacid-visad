using FluentValidation;
using VisaD.Application.Candidates.Commands;

namespace VisaD.Application.Candidates.Validations
{
	public class CreateCandidateValidator : AbstractValidator<CreateCandidateCommand>
	{
		public CreateCandidateValidator()
		{
			RuleFor(a => a.Candidate.FirstName).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.LastName).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.PassportNumber).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.PassportValidUntil).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.BirthDate).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.BirthPlace).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.Country.Name).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.Nationality.Name).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.Phone).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.Mail).EmailAddress().NotEmpty().NotNull();
			RuleFor(a => a.Candidate.FirstNameCyrillic).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.LastNameCyrillic).NotEmpty().NotNull();
			RuleFor(a => a.Candidate.ImgFile.Size).NotEmpty().NotNull();
		}
    }
}
