using FluentValidation;
using VisaD.Application.Applications.Commands.Entities;

namespace VisaD.Application.Applications.Validations
{
    public class UpdateEducationValidator : AbstractValidator<UpdateEducationCommand>
    {
        public UpdateEducationValidator()
        {
            //!!Id = Id of Trainee!!!
            RuleFor(a => a.Model.EducationalQualification).NotEmpty().NotNull();

            RuleFor(a => a.Model.SchoolYear).NotEmpty().NotNull();

            RuleFor(a => a.Model.Faculty).NotEmpty().NotNull();

            RuleFor(a => a.Model.Form).NotEmpty().NotNull();

            //IsNotTrainee
            RuleFor(a => a.Model.Speciality).NotEmpty().NotNull()
                .When(a => a.Model.EducationalQualification.Id != 6)
                .Empty()
                .When(a => a.Model.EducationalQualification.Id == 6, ApplyConditionTo.CurrentValidator);

            RuleFor(a => a.Model.EducationSpecialityLanguages).NotEmpty().NotNull()
                .When(a => a.Model.EducationalQualification.Id != 6)
                .Empty()
                .When(a => a.Model.EducationalQualification.Id == 6, ApplyConditionTo.CurrentValidator);

            RuleFor(a => a.Model.Duration).NotEmpty().NotNull().GreaterThan(0)
                .When(a => a.Model.EducationalQualification.Id != 6)
                .Empty()
                .When(a => a.Model.EducationalQualification.Id == 6, ApplyConditionTo.CurrentValidator);

            //IsTrainee
            RuleFor(a => a.Model.Specialization).NotEmpty().NotNull().Length(5, 256)
                .When(a => a.Model.EducationalQualification.Id == 6)
                .Empty()
                .When(a => a.Model.EducationalQualification.Id != 6, ApplyConditionTo.CurrentValidator);

            RuleFor(a => a.Model.TraineeDuration).NotEmpty().NotNull()
                .When(a => a.Model.EducationalQualification.Id == 6)
                .Empty()
                .When(a => a.Model.EducationalQualification.Id != 6, ApplyConditionTo.CurrentValidator);
        }
    }
}
