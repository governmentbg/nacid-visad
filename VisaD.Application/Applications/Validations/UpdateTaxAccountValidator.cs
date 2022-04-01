using FluentValidation;
using System.Linq;
using VisaD.Application.Applications.Commands.Entities;

namespace VisaD.Application.Applications.Validations
{
	public class UpdateTaxAccountValidator : AbstractValidator<UpdateTaxAccountCommand>
	{
		public UpdateTaxAccountValidator()
		{
            RuleFor(a => a.Model.Taxes.Select(t => t.Type)).NotEmpty().NotNull();
            RuleFor(a => a.Model.Taxes.Select(t => t.Amount)).NotEmpty().NotNull();
            RuleFor(a => a.Model.Taxes.Select(t => t.CurrencyType.Name)).NotEmpty().NotNull();
            RuleFor(a => a.Model.Taxes.Select(t => t.Iban)).NotEmpty().NotNull();
		}
	}
}
