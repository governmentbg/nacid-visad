using FluentValidation;
using System.Linq;
using VisaD.Application.Applications.Commands.Entities;

namespace VisaD.Application.Applications.Validations
{
	public class UpdateDocumentValidator : AbstractValidator<UpdateDocumentCommand>
	{
		public UpdateDocumentValidator()
		{
			//RuleFor(a => a.Model.Files.Select(f => f.Type.Name)).NotEmpty().NotNull();
			//RuleFor(a => a.Model.Files.Select(f => f.AttachedFile)).NotEmpty().NotNull();
		}
	}
}
