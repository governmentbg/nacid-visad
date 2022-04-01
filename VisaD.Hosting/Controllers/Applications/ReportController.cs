using VisaD.Application.Common.Interfaces;
using VisaD.Hosting.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Applications.Queries;
using VisaD.Data.Applications.Enums;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Utils;
using VisaD.Application.Common.Commands;
using VisaD.Application.Common.Constants;
using System.IO;

namespace VisaD.Hosting.Controllers.Applications
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportController : BaseMediatorController
	{
		private readonly IExcelProcessor excelProcessor;
		private readonly IUserContext userContext;

		public ReportController(IMediator mediator, IExcelProcessor excelProcessor, IUserContext userContext)
			: base(mediator)
		{
			this.excelProcessor = excelProcessor;
			this.userContext = userContext;
		}

		[HttpGet]
		public async Task<ReportDto> GetReports([FromQuery] GenerateReportCommitQuery query)
			=> await this.mediator.Send(query);

		[HttpPost("Excel")]
		public async Task<FileStreamResult> ExportApplicationsFiltered([FromBody] GenerateReportCommitQuery query)
		{
			var report = await this.mediator.Send(query);

			var excelStream = new MemoryStream();

			if (report.ReportType == ReportType.ReportByInstitution)
			{
				if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Institution, ColumnName = "Висше училище" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
				else
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Institution, ColumnName = "Висше училище" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.ModificationCommitsCount, ColumnName = "Върнати за редакция" },
					e => new ExcelTableTuple { CellItem = e.UnsignedCommitsCount, ColumnName = "За подпис" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
			}
			else if (report.ReportType == ReportType.ReportByNationality)
			{
				if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Nationality, ColumnName = "Гражданство" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
				else
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Nationality, ColumnName = "Гражданство" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.ModificationCommitsCount, ColumnName = "Върнати за редакция" },
					e => new ExcelTableTuple { CellItem = e.UnsignedCommitsCount, ColumnName = "За подпис" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
			}
			else if (report.ReportType == ReportType.ReportByCountry)
			{
				if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Country, ColumnName = "Месторождение" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
				else
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.Country, ColumnName = "Месторождение" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.ModificationCommitsCount, ColumnName = "Върнати за редакция" },
					e => new ExcelTableTuple { CellItem = e.UnsignedCommitsCount, ColumnName = "За подпис" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
			}
			else if (report.ReportType == ReportType.ReportByEducationalQualification)
			{
				if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.EducationalQualification, ColumnName = "ОКС" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
				else
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.EducationalQualification, ColumnName = "ОКС" },
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.ModificationCommitsCount, ColumnName = "Върнати за редакция" },
					e => new ExcelTableTuple { CellItem = e.UnsignedCommitsCount, ColumnName = "За подпис" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
			}
			else if (report.ReportType == ReportType.DefaultReport)
			{
				if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
				else
				{
					excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.PendingCommitsCount, ColumnName = "Изпратени за разглеждане" },
					e => new ExcelTableTuple { CellItem = e.ModificationCommitsCount, ColumnName = "Върнати за редакция" },
					e => new ExcelTableTuple { CellItem = e.UnsignedCommitsCount, ColumnName = "За подпис" },
					e => new ExcelTableTuple { CellItem = e.CertificateCommitsCount, ColumnName = "Издадени удостоверения" },
					e => new ExcelTableTuple { CellItem = e.RejectedCommitsCount, ColumnName = "Издадени откази" },
					e => new ExcelTableTuple { CellItem = e.AnnulledCommitsCount, ColumnName = "Анулирани удостоверения" }
					);
				}
			}
			else if (report.ReportType == ReportType.ReportByCandidateWithMoreThanOneCertificate)
			{
				excelStream = this.excelProcessor.ExportReports(query, report.Reports,
					e => new ExcelTableTuple { CellItem = e.ConcatenatedCandidateNames, ColumnName = "Име" },
					e => new ExcelTableTuple { CellItem = e.CandidateNationality, ColumnName = "Гражданство" },
					e => new ExcelTableTuple { CellItem = e.ConcatenatedCandidateBirthInfo, ColumnName = "Роден в" },
					e => new ExcelTableTuple { CellItem = e.CandidateCertficatesCount, ColumnName = "Издадени удостоверения" }
					);
			}

			return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
		}

		[HttpPost("PDF")]
		public async Task<FileContentResult> ExportApplicationsFilteredPdf([FromBody] GenerateReportCommitQuery query)
		{
			var report = await this.mediator.Send(query);

			if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
			{
				query.HideHtmlElement = "hidden";
			}
			else
			{
				query.HideHtmlElement = "";
			}

			if (report.ReportType == ReportType.DefaultReport)
			{
				var bytes = await this.mediator.Send(new GenerateReportPdfCommand {
					Items = report.Reports,
					TemplateAlias = FileTemplateAliases.DEFAULT_REPORT_TEMPLATE,
					Query = query
				});

				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByInstitution)
			{
				var bytes = await this.mediator.Send(new GenerateReportPdfCommand {
					Items = report.Reports,
					TemplateAlias = FileTemplateAliases.REPORT_BY_INSTITUTION_TEMPLATE,
					Query = query
				});

				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByNationality)
			{
				var bytes = await this.mediator.Send(new GenerateReportPdfCommand {
					Items = report.Reports,
					TemplateAlias = FileTemplateAliases.REPORT_BY_NATIONALITY_TEMPLATE,
					Query = query
				});

				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByCountry)
			{
				var bytes = await this.mediator.Send(new GenerateReportPdfCommand {
					Items = report.Reports,
					TemplateAlias = FileTemplateAliases.REPORT_BY_COUNTRY_TEMPLATE,
					Query = query
				});

				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByEducationalQualification)
			{
				var bytes = await this.mediator.Send(new GenerateReportPdfCommand {
					Items = report.Reports,
					TemplateAlias = FileTemplateAliases.REPORT_BY_EDUCATIONAL_QUALIFICATION_TEMPLATE,
					Query = query
				});

				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}
			else if (report.ReportType == ReportType.ReportByCandidateWithMoreThanOneCertificate)
			{
				var bytes = await this.mediator.Send(new GenerateReportPdfCommand {
					Items = report.Reports,
					TemplateAlias = FileTemplateAliases.REPORT_BY_CANDIDATES_WITH_MORETHANONE_CERTIFICATE_TEMPLATE,
					Query = query
				});

				return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Reports.pdf" };
			}

			return null;
		}
	}
}
