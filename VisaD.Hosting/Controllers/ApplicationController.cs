using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VisaD.Application.Applications.Commands;
using VisaD.Application.Applications.Commands.Entities;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Applications.Queries;
using VisaD.Application.Common.Commands;
using VisaD.Application.Common.Constants;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Application.Utils;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures.Constants;
using VisaD.Hosting.Controllers.Common;
using VisaD.Hosting.Infrastructure.Auth;

namespace VisaD.Hosting.Controllers
{
	[Route("api/[controller]")]
	public class ApplicationController : BaseMediatorController
	{
		private readonly IUserContext userContext;
		private readonly IExcelProcessor excelProcessor;

		public ApplicationController(
			IMediator mediator,
			IUserContext userContext,
			IExcelProcessor excelProcessor
			)
			: base(mediator)
		{
			this.userContext = userContext;
			this.excelProcessor = excelProcessor;
		}

		[HttpGet]
		public async Task<SearchResultItemDto<ApplicationSearchResultItemDto>> GetApplicationsFiltered([FromQuery] SearchApplicationCommitQuery query)
		{
			if (!string.IsNullOrWhiteSpace(userContext.OrganizationName) && userContext.Role == UserRoleAliases.UNIVERSITY_USER)
			{
				query.Institution = userContext.OrganizationName;
			}

			return await this.mediator.Send(query);
		}

		[HttpPost("Excel")]
		public async Task<FileStreamResult> ExportApplicationsFiltered([FromBody] SearchApplicationCommitQuery query)
		{
			if (!string.IsNullOrWhiteSpace(userContext.OrganizationName) && userContext.Role == UserRoleAliases.UNIVERSITY_USER)
			{
				query.Institution = userContext.OrganizationName;
			}

			query.Limit = int.MaxValue;
			query.Offset = 0;

			var searchResult = await this.mediator.Send(query);

			var excelStream = excelProcessor.Export(searchResult.Items,
				e => new ExcelTableTuple { CellItem = e.RegisterNumber, ColumnName = "Номер" },
				e => new ExcelTableTuple { CellItem = e.CandidateName, ColumnName = "Имена" },
				e => new ExcelTableTuple { CellItem = e.CandidateInfo, ColumnName = "Роден в" },
				e => new ExcelTableTuple { CellItem = e.CandidateNationality, ColumnName = "Гражданство" },
				e => new ExcelTableTuple { CellItem = e.OrganizationName, ColumnName = "Висше училище" },
				e => new ExcelTableTuple { CellItem = e.SpecialityInfo, ColumnName = "Специалност" },
				e => new ExcelTableTuple { CellItem = e.FormEducationInfo, ColumnName = "Форма на обучение" },
				e => new ExcelTableTuple { CellItem = e.ResultType, ColumnName = "Статус" }

			);
			return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Applications.xlsx" };
		}

		[HttpPost("PDF")]
		public async Task<FileContentResult> ExportApplicationFilteredPdf([FromBody] SearchApplicationCommitQuery query)
		{
			if (!string.IsNullOrWhiteSpace(userContext.OrganizationName))
			{
				query.Institution = userContext.OrganizationName;
			}

			query.Limit = int.MaxValue;
			query.Offset = 0;

			var applications = await this.mediator.Send(query);

			var bytes = await this.mediator.Send(new GeneratePdfCommand {
				Items = applications.Items,
				TemplateAlias = FileTemplateAliases.APPLICATIONS_EXPORT
			});
			return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Applications.pdf" };
		}

		[HttpGet("applicant")]
		public Task<ApplicantDto> GetApplicantData()
			=> this.mediator.Send(new GetApplicantDataQuery { UserId = this.userContext.UserId });

		[HttpGet("lot/{lotId:int}/commit/{commitId:int}")]
		public Task<ApplicationCommitDto> GetApplicationCommit([FromRoute] int lotId, [FromRoute] int commitId)
			=> this.mediator.Send(new GetApplicationCommitQuery { LotId = lotId, CommitId = commitId });

		[HttpGet("lot/{lotId:int}/history")]
		public Task<ApplicationLotHistoryDto> GetApplicationCommitsHistory([FromRoute] int lotId)
			=> this.mediator.Send(new GetApplicationLotHistoryQuery { LotId = lotId });

		[HttpPost]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
		public Task<CommitInfoDto> CreateApplication([FromBody] CreateApplicationCommand command)
			=> this.mediator.Send(command);

		[HttpPost("lot/{lotId:int}/startmodification")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.LOT_RESULT_USER)]
		public async Task<ApplicationCommitDto> StartModification([FromRoute] int lotId, [FromQuery] string changeStateDescription)
		{
			var modificationCommitInfo = await this.mediator.Send(new StartCommitModificationCommand<ApplicationCommit> { LotId = lotId, ChangeStateDescription = changeStateDescription });
			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = modificationCommitInfo.LotId, Type = ApplicationLotResultType.Modification });
			var result = await this.mediator.Send(new GetApplicationCommitQuery { LotId = modificationCommitInfo.LotId, CommitId = modificationCommitInfo.CommitId });

			await this.mediator.Send(new AddApplicationStatusHistoryCommand {
				LotId = result.LotId,
				CommitId = null,
				ChangeStateDescription = result.ChangeStateDescription,
				State = result.State,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});

			var templateData = new {
				RegisterNumber = result.RegisterNumber,
				Description = changeStateDescription
			};

			await this.mediator.Send(new SendApplicationEmailCommand { 
				Alias = EmailTypeAlias.MODIFICATION_APPLICATION, 
				CreatorUserId = result.CreatorUserId,
				TemplateData = templateData
			});

			return result;
		}

		[HttpPost("lot/{lotId:int}/finishmodification")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<ApplicationCommitDto> FinishModification([FromRoute] int lotId)
		{
			var command = new FinishCommitModificationCommand<ApplicationLot, ApplicationCommit> {
				LotId = lotId,
				ShouldRegisterLot = true,
				RegisterIndexAlias = RegisterIndexAlias.APPLICATION_REGISTER_INDEX
			};
			var finishedModificationInfo = await this.mediator.Send(command);
			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = finishedModificationInfo.LotId, Type = ApplicationLotResultType.Actual });

			var result = await this.mediator.Send(new GetApplicationCommitQuery {
				LotId = finishedModificationInfo.LotId,
				CommitId = finishedModificationInfo.CommitId
			});

			await this.mediator.Send(new AddApplicationStatusHistoryCommand {
				LotId = result.LotId,
				CommitId = result.Id,
				ChangeStateDescription = null,
				State = result.State,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});

			return result;
		}

		[HttpPost("lot/{lotId:int}/cancelmodification")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<ApplicationCommitDto> CancelModification([FromRoute] int lotId)
		{
			var newActualModification = await this.mediator.Send(new CancelModificationCommand { LotId = lotId });
			return await this.mediator.Send(new GetApplicationCommitQuery { LotId = newActualModification.LotId, CommitId = newActualModification.CommitId });
		}

		[HttpPost("lot/{lotId:int}/erase")]
		public async Task<ApplicationCommitDto> EraseApplication([FromRoute] int lotId, [FromQuery] string changeStateDescription)
		{
			var erasedCommitInfo = await this.mediator.Send(new EraseCommitCommand<ApplicationCommit> { LotId = lotId, ChangeStateDescription = changeStateDescription });
			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = lotId, Type = ApplicationLotResultType.Deleted });

			var result = await this.mediator.Send(new GetApplicationCommitQuery { LotId = erasedCommitInfo.LotId, CommitId = erasedCommitInfo.CommitId });

			await this.mediator.Send(new AddApplicationStatusHistoryCommand {
				LotId = result.LotId,
				CommitId = result.Id,
				ChangeStateDescription = result.ChangeStateDescription,
				State = result.State,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});

			return result;
		}

		[HttpPost("lot/{lotId:int}/annulment")]
		public async Task AnnulApplication([FromRoute] int lotId, [FromQuery] string changeStateDescription)
		{
			var annuledCommitInfo = await this.mediator.Send(new AnnulCommitCommand<ApplicationCommit> { LotId = lotId, ChangeStateDescription = changeStateDescription });
			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = lotId, Type = ApplicationLotResultType.Annulled });

			var result = await this.mediator.Send(new GetApplicationCommitQuery { LotId = annuledCommitInfo.LotId, CommitId = annuledCommitInfo.CommitId });

			await this.mediator.Send(new AddApplicationStatusHistoryCommand
			{
				LotId = result.LotId,
				CommitId = result.Id,
				ChangeStateDescription = result.ChangeStateDescription,
				State = result.State,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});
		}

		[HttpPost("lot/{lotId:int}/refusesign")]
		public async Task RefuseSign([FromRoute] int lotId, [FromQuery] string changeStateDescription)
		{
			var annuledCommitInfo = await this.mediator.Send(new RefuseSignCommand<ApplicationCommit> { LotId = lotId, ChangeStateDescription = changeStateDescription });
			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = lotId, Type = ApplicationLotResultType.RefusedSign });

			var result = await this.mediator.Send(new GetApplicationCommitQuery { LotId = annuledCommitInfo.LotId, CommitId = annuledCommitInfo.CommitId });

			await this.mediator.Send(new AddApplicationStatusHistoryCommand {
				LotId = result.LotId,
				CommitId = result.Id,
				ChangeStateDescription = result.ChangeStateDescription,
				State = result.State,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});
		}

		[HttpPost("lot/{lotId:int}/reverterased")]
		public async Task<ApplicationCommitDto> RevertErasedApplication([FromRoute] int lotId)
		{
			var erasedCommitInfo = await this.mediator.Send(new RevertErasedCommitCommand<ApplicationCommit> { LotId = lotId });

			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = lotId, Type = ApplicationLotResultType.Actual });

			var result = await this.mediator.Send(new GetApplicationCommitQuery { LotId = erasedCommitInfo.LotId, CommitId = erasedCommitInfo.CommitId });

			await this.mediator.Send(new AddApplicationStatusHistoryCommand {
				LotId = result.LotId,
				CommitId = result.Id,
				ChangeStateDescription = null,
				State = result.State,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});

			return result;
		}

		[HttpPost("lot/{lotId:int}/approve")]
		public async Task<ApplicationCommitDto> ApproveApplication([FromRoute] int lotId)
		{
			var approvedCommitInfo = await this.mediator.Send(new ApproveCommitCommand<ApplicationCommit> { LotId = lotId });
			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = lotId, Type = ApplicationLotResultType.Approved });

			var result = await this.mediator.Send(new GetApplicationCommitQuery { LotId = approvedCommitInfo.LotId, CommitId = approvedCommitInfo.CommitId });

			await this.mediator.Send(new AddApplicationStatusHistoryCommand {
				LotId = result.LotId,
				CommitId = result.Id,
				ChangeStateDescription = null,
				State = result.State,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});

			return result;
		}

		[HttpPost("lot/{lotId:int}/result")]
		public async Task<ApplicationLotResultDto> AddApplicationLotResult([FromBody] AddApplicationLotResultCommand command)
		{
			var approvedCommitInfo = await this.mediator.Send(new ApproveCommitCommand<ApplicationCommit> { LotId = command.LotId });
			await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = command.LotId, Type = ApplicationLotResultType.Approved });

			var result = await this.mediator.Send(new GetApplicationCommitQuery { LotId = approvedCommitInfo.LotId, CommitId = approvedCommitInfo.CommitId });

			await this.mediator.Send(new AddApplicationStatusHistoryCommand
			{
				LotId = result.LotId,
				CommitId = result.Id,
				ChangeStateDescription = null,
				State = result.State,
				LotResultType = command.Type,
				CandidateName = result.CandidateCommit.CandidatePart.Entity.FirstName + " " + result.CandidateCommit.CandidatePart.Entity.LastName,
				CandidateBirthDate = result.CandidateCommit.CandidatePart.Entity.BirthDate,
				CandidateCountry = result.CandidateCommit.CandidatePart.Entity.Country.Name,
				RegisterNumber = result.RegisterNumber
			});

			return await this.mediator.Send(command);
		}

		[HttpGet("result/{resultId:int}/signingInformation")]
		public async Task<ApplicationResultSigningInformationDto> GetApplicationResultSigningInformation([FromRoute] int resultId)
			=> await this.mediator.Send(new GetApplicationResultSigningInformationQuery() { ResultId = resultId });

		[HttpPut("result/{resultId:int}/signingInformation")]
		public async Task<ApplicationLotResultDto> UpdateApplicationLotResultFile([FromRoute] int resultId, [FromBody] UpdateApplicationLotResultFileCommand command)
			=> await this.mediator.Send(command);

		[HttpDelete("lot/{lotId:int}")]
		public Task DeleteApplicationLot([FromRoute] int lotId)
			=> this.mediator.Send(new DeleteApplicationLotCommand { LotId = lotId });
	}
}
