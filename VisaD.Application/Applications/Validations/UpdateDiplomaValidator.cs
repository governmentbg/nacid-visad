using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using VisaD.Application.Applications.Commands.Entities;

namespace VisaD.Application.Applications.Validations
{
	public class UpdateDiplomaValidator : AbstractValidator<UpdateDiplomaCommand>
	{
		public UpdateDiplomaValidator()
		{
            var today = DateTime.UtcNow;

            RuleForEach(a => a.Model.DiplomaFiles).ChildRules(diploma =>
            {
                diploma.RuleFor(a => a.Country).NotEmpty().NotNull();

                diploma.RuleFor(a => a.City).NotEmpty().NotNull().Length(3, 30);

                diploma.RuleFor(a => a.Type).NotEmpty().NotNull();

                diploma.RuleFor(a => a.OrganizationName).NotEmpty().NotNull().Length(5, 70)
                    .Must(text => BeValidLettersAndNumbers(text));

                diploma.RuleFor(a => a.DiplomaNumber).NotEmpty().NotNull().Length(4, 20)
                    .Must(number => BeValidDiplomaNumber(number));

                diploma.RuleFor(a => a.IssuedDate).NotEmpty().NotNull()
                    .Must(date => BeAValidDate(date,
                        new DateTime(1970, 1, 1),
                        new DateTime(today.Year, today.Month, today.Day)));
            });

            RuleFor(a => a.Model.Description).MaximumLength(256);
        }

        private bool BeAValidDate(DateTime date, DateTime min, DateTime max)
        {
            return min <= date && date <= max;
        }

        private bool BeValidLettersAndNumbers(string text)
        {
            //LETTERS_AND_NUMBERS_REGEX
            var regex = new Regex("^[A-Za-zаАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ,0-9, -.]+$");
            try
            {
                return regex.IsMatch(text);
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }

        private bool BeValidDiplomaNumber(string number)
        {
            //DIPLOMA_NUMBER_REGEX
            var regex = new Regex("^[A-Za-zаАбБвВгГдДеЕжЖзЗиИйЙкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩьъЪюЮяЯ,0-9,-]+$");
            try
            {
                return regex.IsMatch(number);
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }
    }
}
