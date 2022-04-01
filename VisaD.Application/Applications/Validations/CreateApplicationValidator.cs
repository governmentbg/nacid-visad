using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using VisaD.Application.Applications.Commands;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Validations
{
    public class CreateApplicationValidator : AbstractValidator<CreateApplicationCommand>
    {
        public CreateApplicationValidator()
        {
            var today = DateTime.UtcNow;

            #region Applicant
            RuleFor(a => a.Applicant.FirstName).NotEmpty().NotNull();
            RuleFor(a => a.Applicant.LastName).NotEmpty().NotNull();
            RuleFor(a => a.Applicant.Position).NotEmpty().NotNull();
            RuleFor(a => a.Applicant.Mail).EmailAddress().NotEmpty().NotNull();
            RuleFor(a => a.Applicant.Institution.Name).NotEmpty().NotNull();
            #endregion

            #region Education
            //!!Id = Id of Trainee!!!
            RuleFor(a => a.Education.EducationalQualification).NotEmpty().NotNull();

            RuleFor(a => a.Education.SchoolYear).NotEmpty().NotNull();

            RuleFor(a => a.Education.Faculty).NotEmpty().NotNull();

            RuleFor(a => a.Education.Form).NotEmpty().NotNull();

            //IsNotTrainee
            RuleFor(a => a.Education.Speciality).NotEmpty().NotNull()
                .When(a => a.Education.EducationalQualification.Id != 6)
                .Empty()
                .When(a => a.Education.EducationalQualification.Id == 6, ApplyConditionTo.CurrentValidator);

            RuleFor(a => a.Education.EducationSpecialityLanguages).NotEmpty().NotNull()
                .When(a => a.Education.EducationalQualification.Id != 6)
                .Empty()
                .When(a => a.Education.EducationalQualification.Id == 6, ApplyConditionTo.CurrentValidator);

            RuleFor(a => a.Education.Duration).NotEmpty().NotNull().GreaterThan(0)
                .When(a => a.Education.EducationalQualification.Id != 6)
                .Empty()
                .When(a => a.Education.EducationalQualification.Id == 6, ApplyConditionTo.CurrentValidator);

            //IsTrainee
            RuleFor(a => a.Education.Specialization).NotEmpty().NotNull().Length(5, 256)
                .When(a => a.Education.EducationalQualification.Id == 6)
                .Empty()
                .When(a => a.Education.EducationalQualification.Id != 6, ApplyConditionTo.CurrentValidator);

            RuleFor(a => a.Education.TraineeDuration).NotEmpty().NotNull()
                .When(a => a.Education.EducationalQualification.Id == 6)
                .Empty()
                .When(a => a.Education.EducationalQualification.Id != 6, ApplyConditionTo.CurrentValidator);
            #endregion

            #region Training
            RuleFor(a => a.Training.LanguageProficiencies.Select(st => st.Language)).NotEmpty().NotNull();
            RuleFor(a => a.Training.LanguageProficiencies.Select(st => st.Reading.Name)).NotEmpty().NotNull();
            RuleFor(a => a.Training.LanguageProficiencies.Select(st => st.Writing.Name)).NotEmpty().NotNull();
            RuleFor(a => a.Training.LanguageProficiencies.Select(st => st.Speaking.Name)).NotEmpty().NotNull();
            #endregion

            #region TaxAccount
            RuleFor(a => a.TaxAccount.Taxes.Select(t => t.Type)).NotEmpty().NotNull();
            RuleFor(a => a.TaxAccount.Taxes.Select(t => t.Amount)).NotEmpty().NotNull();
            RuleFor(a => a.TaxAccount.Taxes.Select(t => t.CurrencyType.Name)).NotEmpty().NotNull();
            RuleFor(a => a.TaxAccount.Taxes.Select(t => t.Iban)).NotEmpty().NotNull();
            RuleFor(a => a.TaxAccount.Taxes.Select(t => t.Bank)).NotEmpty().NotNull();
            RuleFor(a => a.TaxAccount.Taxes.Select(t => t.Bic)).NotEmpty().NotNull();
            #endregion

            #region Document
            //RuleFor(a => a.Document.Files.Select(f => f.Type.Name)).NotEmpty().NotNull();
            //RuleFor(a => a.Document.Files.Select(f => f.AttachedFile)).NotEmpty().NotNull();
            #endregion

            #region Diploma
            RuleForEach(a => a.Diploma.DiplomaFiles).ChildRules(diploma =>
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

            RuleFor(a => a.Diploma.Description).MaximumLength(256);
            #endregion

            #region Representative
            RuleFor(a => a.Representative.FirstName).NotEmpty().NotNull().Length(2, 30)
                .Must(name => BeValidName(name))
                .When(a => a.Representative.HasRepresentative)
                .When(a => a.Representative.Type == RepresentativeType.Individual);

            RuleFor(a => a.Representative.FirstName).NotEmpty().NotNull().Length(2, 80)
                .Must(name => BeValidCyrilicName(name))
                .When(a => a.Representative.HasRepresentative)
                .When(a => a.Representative.Type == RepresentativeType.LegalEntity);

            RuleFor(a => a.Representative.FirstName).NotEmpty().NotNull().Length(2, 80)
                .Must(name => BeValidName(name))
                .When(a => a.Representative.HasRepresentative)
                .When(a => a.Representative.Type == RepresentativeType.Foreigner);

            RuleFor(a => a.Representative.LastName).NotEmpty().NotNull().Length(2, 30)
                .Must(name => BeValidName(name))
                .When(a => a.Representative.HasRepresentative)
                .When(a => a.Representative.Type == RepresentativeType.Individual);

            RuleFor(a => a.Representative.IdentificationCode).NotEmpty().NotNull().Length(10)
                .When(a => a.Representative.HasRepresentative)
                .When(a => a.Representative.Type == RepresentativeType.Individual);

            RuleFor(a => a.Representative.IdentificationCode).Length(9)
                .When(a => a.Representative.HasRepresentative)
                .When(a => a.Representative.Type == RepresentativeType.LegalEntity);

            RuleFor(a => a.Representative.IdentificationCode).NotEmpty().NotNull().Length(8, 15)
                .When(a => a.Representative.HasRepresentative)
                .When(a => a.Representative.Type == RepresentativeType.Foreigner);

            RuleFor(a => a.Representative.Mail).NotEmpty().NotNull()
                .Must(email => BeValidEmail(email))
                .When(a => a.Representative.HasRepresentative);

            RuleFor(a => a.Representative.Phone).NotEmpty().NotNull().Length(9, 14)
                .When(a => a.Representative.HasRepresentative);

            RuleFor(a => a.Representative.Note).MaximumLength(256)
                .When(a => a.Representative.HasRepresentative);

            RuleFor(a => a.Representative.SubmissionDate).NotEmpty().NotNull()
                .Must(date => BeAValidDate(date,
                    new DateTime(today.Year - 1, today.Month, today.Day),
                    new DateTime(today.Year, today.Month, today.Day)));
            #endregion

            #region MedicalCertificate
            RuleFor(a => a.MedicalCertificate.IssuedDate).NotEmpty().NotNull();
            RuleFor(a => a.MedicalCertificate.File).NotEmpty().NotNull();
            #endregion
        }

        #region Helpers
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
            catch(ArgumentNullException e)
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
        #endregion
    }
}
