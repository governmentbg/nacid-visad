using FluentValidation;
using System;
using System.Text.RegularExpressions;
using VisaD.Application.Applications.Commands.Entities;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Validations
{
	public class UpdateRepresentativeValidator : AbstractValidator<UpdateRepresentativeCommand>
	{
		public UpdateRepresentativeValidator()
		{
            var today = DateTime.UtcNow;

            RuleFor(a => a.Model.FirstName).NotEmpty().NotNull().Length(2, 30)
                .Must(name => BeValidName(name))
                .When(a => a.Model.HasRepresentative)
                .When(a => a.Model.Type == RepresentativeType.Individual);

            RuleFor(a => a.Model.FirstName).NotEmpty().NotNull().Length(2, 80)
                .Must(name => BeValidCyrilicName(name))
                .When(a => a.Model.HasRepresentative)
                .When(a => a.Model.Type == RepresentativeType.LegalEntity);

            RuleFor(a => a.Model.FirstName).NotEmpty().NotNull().Length(2, 80)
                .Must(name => BeValidName(name))
                .When(a => a.Model.HasRepresentative)
                .When(a => a.Model.Type == RepresentativeType.Foreigner);

            RuleFor(a => a.Model.LastName).NotEmpty().NotNull().Length(2, 30)
                .Must(name => BeValidName(name))
                .When(a => a.Model.HasRepresentative)
                .When(a => a.Model.Type == RepresentativeType.Individual);

            RuleFor(a => a.Model.IdentificationCode).NotEmpty().NotNull().Length(10)
                .When(a => a.Model.HasRepresentative)
                .When(a => a.Model.Type == RepresentativeType.Individual);

            RuleFor(a => a.Model.IdentificationCode).Length(9)
                .When(a => a.Model.HasRepresentative)
                .When(a => a.Model.Type == RepresentativeType.LegalEntity);

            RuleFor(a => a.Model.IdentificationCode).NotEmpty().NotNull().Length(8, 15)
                .When(a => a.Model.HasRepresentative)
                .When(a => a.Model.Type == RepresentativeType.Foreigner);

            RuleFor(a => a.Model.Mail).NotEmpty().NotNull()
                .Must(email => BeValidEmail(email))
                .When(a => a.Model.HasRepresentative);

            RuleFor(a => a.Model.Phone).NotEmpty().NotNull().Length(9, 14)
                .When(a => a.Model.HasRepresentative);

            RuleFor(a => a.Model.Note).MaximumLength(256)
                .When(a => a.Model.HasRepresentative);

            RuleFor(a => a.Model.SubmissionDate).NotEmpty().NotNull()
                .Must(date => BeAValidDate(date,
                    new DateTime(today.Year - 1, today.Month, today.Day),
                    new DateTime(today.Year, today.Month, today.Day)));
        }

        private bool BeAValidDate(DateTime date, DateTime min, DateTime max)
        {
            return min <= date && date <= max;
        }

        private bool BeValidEmail(string email)
        {
            //EMAIL_REGEX
            var regex = new Regex("[A-Za-z0-9._%+-]{2,}@[a-zA-Z-,0-9]{1,}([.]{1}[a-zA-Z, 0-9]{2,}|[.]{1}[a-zA-Z, 0-9, -]{2,}[.]{1}[a-zA-Z, -]{2,})(|[.]{1}[a-zA-Z, -]{2,}[.]{1}[a-zA-Z, -]{2,})");
            try
            {
                return regex.IsMatch(email);
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }

        private bool BeValidName(string name)
        {
            //LATIN_AND_CYRILLIC_NAMES_REGEX
            var regex = new Regex("^[A-Za-zаАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ, -]+$");
            try
            {
                return regex.IsMatch(name);
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }

        private bool BeValidCyrilicName(string name)
        {
            //CYRILLIC_NAMES_REGEX
            var regex = new Regex("^[аАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ, -.]+$");
            try
            {
                return regex.IsMatch(name);
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }
    }
}
