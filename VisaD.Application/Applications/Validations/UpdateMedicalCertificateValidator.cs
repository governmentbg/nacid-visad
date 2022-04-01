using FluentValidation;
using VisaD.Application.Applications.Commands.Entities;

namespace VisaD.Application.Applications.Validations
{
	public class UpdateMedicalCertificateValidator : AbstractValidator<UpdateMedicalCertificateCommand>
	{
		public UpdateMedicalCertificateValidator()
		{
			RuleFor(a => a.Model.IssuedDate).NotEmpty().NotNull();
			RuleFor(a => a.Model.File).NotEmpty().NotNull();
		}
	}
}
