using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VisaD.Application.Applications.Commands.Extensions;
using VisaD.Application.Common.Behaviours;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Common.Services;
using VisaD.Application.DomainValidation;
using VisaD.Application.Emails;
using VisaD.Application.Ems.Converters;
using VisaD.Application.InstitutionSpecialities.Services;
using VisaD.Application.Logging;
using VisaD.Application.Nomenclatures.Services;
using VisaD.Application.Users.Services;

namespace VisaD.Application.Common.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services, Assembly assembly)
		{
			services
				.AddMediatR(assembly);

			services
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>))
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandTransactionBehaviour<,>))
			;

			services
				.AddTransient<IPasswordService, PasswordService>()
				.AddTransient(typeof(INomenclatureService<>), typeof(NomenclatureService<>))
				.AddTransient(typeof(IEmailService), typeof(EmailService))
				.AddTransient<IPdfFileService, PdfFileService>()
				.AddTransient<QrCodeService>()
				.AddTransient<IApplicationFileTypeService, ApplicationFileTypeService>()
				.AddTransient<IBankService, BankService>()
				.AddTransient<ISchoolYearService, SchoolYearService>()
			;

			services
				.AddApplicationPartCommands();

			services
				.AddValidatorsFromAssembly(assembly);

			services
				.AddScoped<DomainValidationService>();

			services
				.AddScoped<InstitutionSpecialitiesService>();

			services
				.AddScoped<ILoggingService, DbLoggingService>()
				.AddScoped<IEnumUtility, EnumUtility>()
				.AddScoped<IExcelProcessor, ExcelProcessor>()
				.AddScoped<ITemplateService, TemplateService>()
				.AddScoped<IImageFileService, ImageFileService>()
				.AddScoped<IEmsApplicationConverter, EmsApplicationConverter>()
			;

			services
				.AddScoped<InstitutionService>();

			return services;
		}
	}
}
