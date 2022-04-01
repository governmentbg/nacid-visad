using FluentValidation;
using System.Linq;
using VisaD.Application.Applications.Commands.Entities;

namespace VisaD.Application.Applications.Validations
{
	public class UpdateTrainingValidator : AbstractValidator<UpdateTrainingCommand>
	{
		public UpdateTrainingValidator()
		{
			RuleFor(a => a.Model.LanguageProficiencies.Select(st => st.Language)).NotEmpty().NotNull();
			RuleFor(a => a.Model.LanguageProficiencies.Select(st => st.Reading.Name)).NotEmpty().NotNull();
			RuleFor(a => a.Model.LanguageProficiencies.Select(st => st.Writing.Name)).NotEmpty().NotNull();
			RuleFor(a => a.Model.LanguageProficiencies.Select(st => st.Speaking.Name)).NotEmpty().NotNull();

		}
	}
}
