using EnumsNET;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Queries;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Application.Common.Commands
{
	public class GenerateReportPdfCommand : IRequest<byte[]>
	{
		public object Items { get; set; }
		public string TemplateAlias { get; set; }

		public GenerateReportCommitQuery Query { get; set; }

		public class Handler : IRequestHandler<GenerateReportPdfCommand, byte[]>
		{
			private readonly ITemplateService templateService;
			private readonly IPdfFileService applicationFileService;

			public Handler(
				ITemplateService templateService,
				IPdfFileService applicationFileService
			)
			{
				this.templateService = templateService;
				this.applicationFileService = applicationFileService;
			}

			public async Task<byte[]> Handle(GenerateReportPdfCommand request, CancellationToken cancellationToken)
			{
				var template = await this.templateService.GetTemplateAsync(request.TemplateAlias);
				var stream = await this.applicationFileService.GeneratePdfFile(new {
					Items = request.Items,
					ReportTypeName = request.Query.ReportType.AsString(EnumFormat.Description),
					InstitutionName = request.Query.InstitutionName ?? "Всички",
					SchoolYearName = request.Query.SchoolYearName ?? "Всички",
					NationalityName = request.Query.NationalityName ?? "Всички",
					CountryName = request.Query.CountryName ?? "Всички",
					EducationalQualificationName = request.Query.EducationalQualificationName ?? "Всички",
					ReportDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
					HideElement = request.Query.HideHtmlElement
				}, template);

				return stream.ToArray();
			}
		}
	}
}
