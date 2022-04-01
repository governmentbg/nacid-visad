using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Applications.Queries;
using VisaD.Application.Candidates.Commands;
using VisaD.Application.Candidates.Commands.Entities;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Candidates.Queries;
using VisaD.Application.Common.Commands;
using VisaD.Application.Common.Constants;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Application.Utils;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Hosting.Controllers.Common;
using VisaD.Hosting.Infrastructure.Auth;

namespace VisaD.Hosting.Controllers.Candidates
{
	[Route("api/[controller]")]
	public class CandidateController : BaseMediatorController
	{
		private readonly IExcelProcessor excelProcessor;

		public CandidateController(
			IMediator mediator,
			IExcelProcessor excelProcessor
			)
			: base(mediator)
		{
			this.excelProcessor = excelProcessor;
		}

		[HttpGet]
		public async Task<SearchResultItemDto<CandidateSearchResultItemDto>> GetCandidatesFiltered([FromQuery] SearchCandidateCommitQuery query)
			=> await this.mediator.Send(query);

		[HttpGet("Select")]
		public async Task<SearchResultItemDto<CandidateCommitDto>> SelectCandidates([FromQuery] SelectCandidateCommitQuery query)
			=> await this.mediator.Send(query);

		[HttpGet("candidateData")]
		public async Task<CandidateApplicationDataDto> GetCandidateApplicationData([FromQuery] int candidateCommitId)
		{
			var candidateApplicationData = await this.mediator.Send(new GetCandidateApplicationDataQuery { CandidateCommitId = candidateCommitId });

			return candidateApplicationData;
		}

        [HttpPost("Excel")]
        public async Task<FileStreamResult> ExportApplicationsFiltered([FromBody] SearchCandidateCommitQuery query)
        {
            query.Limit = int.MaxValue;
            query.Offset = 0;

            var searchResult = await this.mediator.Send(query);

            var excelStream = excelProcessor.Export(searchResult.Items,
                e => new ExcelTableTuple { CellItem = e.Name, ColumnName = "Име" },
                e => new ExcelTableTuple { CellItem = e.Nationality, ColumnName = "Гражданство" },
				e => new ExcelTableTuple { CellItem = e.ConvertedBirthInfo, ColumnName = "Роден в" },
                e => new ExcelTableTuple { CellItem = e.ConvertedContactInfo, ColumnName = "Ел. поща и телефон" },
                e => new ExcelTableTuple { CellItem = e.ApplicationsCount, ColumnName = "Бр. заявления" }
			);

            return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Candidates.xlsx" };
        }

        [HttpPost("PDF")]
        public async Task<FileContentResult> ExportApplicationFilteredPdf([FromBody] SearchCandidateCommitQuery query)
        {
            query.Limit = int.MaxValue;
            query.Offset = 0;

			var candidates = await this.mediator.Send(query);

            var bytes = await this.mediator.Send(new GeneratePdfCommand {
                Items = candidates.Items,
                TemplateAlias = FileTemplateAliases.CANDIDATES_EXPORT
            });
            return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Candidates.pdf" };
        }

        [HttpGet("lot/{lotId:int}/commit/{commitId:int}")]
		public async Task<CandidateApplicationsDto> GetCandidateCommit([FromRoute] int lotId, [FromRoute] int commitId)
		{
			var candidate = await this.mediator.Send(new GetCandidateCommitQuery { LotId = lotId, CommitId = commitId });
			var hasOtherCommits = await this.mediator.Send(new GetCandidateCommitCountQuery { LotId = lotId });
			var applications = await this.mediator.Send(new GetCandidateApplicationsQuery { CandidateLotId = lotId, CandidateCommitId = commitId, CandidateCommitState = candidate.State });

			var result = new CandidateApplicationsDto { CandidateCommit = candidate, Applications = applications, HasOtherCommits = hasOtherCommits };

			return result;
		}

		[HttpGet("lot/{lotId:int}/history")]
		public Task<CandidateLotHistoryDto> GetApplicationCommitsHistory([FromRoute] int lotId)
			=> this.mediator.Send(new GetCandidateLotHistoryQuery { LotId = lotId });

		[HttpPost]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
		public Task<CommitInfoDto> CreateCandidate([FromBody] CreateCandidateCommand command)
			=> this.mediator.Send(command);

		[HttpPost("ApplicationCandidate")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<CandidateCommitDto> CreateApplicationCandidate([FromBody] CreateCandidateCommand command)
		{
			var commitInfo = await this.mediator.Send(command);
			return await this.mediator.Send(new GetCandidateCommitQuery { LotId = commitInfo.LotId, CommitId = commitInfo.CommitId });
		}

		[HttpPost("lot/{lotId:int}/startmodification")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.LOT_RESULT_USER)]
		public async Task<CandidateCommitDto> StartModification([FromRoute] int lotId, [FromQuery] string changeStateDescription)
		{
			var modificationCommitInfo = await this.mediator.Send(new StartCommitModificationCommand<CandidateCommit> { LotId = lotId, ChangeStateDescription = changeStateDescription });
			var commit = await this.mediator.Send(new GetCandidateCommitQuery { LotId = modificationCommitInfo.LotId, CommitId = modificationCommitInfo.CommitId });

			int newPartId = await this.mediator.Send(new StartPartModificationCommand<CandidatePart, Candidate> { Id = commit.CandidatePart.Id });
			commit.CandidatePart = await this.mediator.Send(new GetCandidatePartQuery { PartId = newPartId });

			return commit;
		}

        [HttpPost("lot/{lotId:int}/part/{partId:int}/finishmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.LOT_RESULT_USER )]
		public async Task<CandidateCommitDto> FinishModification([FromRoute] int lotId, [FromRoute] int partId, [FromBody] CandidateDto model)
		{
            await this.mediator.Send(new UpdateCandidateCommand { PartId = partId, Model = model });

            var command = new FinishCommitModificationCommand<CandidateLot, CandidateCommit> {
				LotId = lotId,
				ShouldRegisterLot = false
			};
			var finishedModificationInfo = await this.mediator.Send(command);

			var query = new GetCandidateCommitQuery {
				LotId = finishedModificationInfo.LotId,
				CommitId = finishedModificationInfo.CommitId
			};
			return await this.mediator.Send(query);
		}

		[HttpPost("commit/{commitId:int}/finishapplicationcandidatemodification")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<CandidateCommitDto> FinishApplicationCandidateModification([FromRoute] int commitId, [FromBody] FinishApplicationCandidateCommitModificationCommand command)
			=> await this.mediator.Send(command);


        [HttpPost("lot/{lotId:int}/cancelmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.LOT_RESULT_USER)]
		public async Task<CandidateCommitDto> CancelModification([FromRoute] int lotId)
		{
            //await this.mediator.Send(new CancelPartModificationCommand<CandidateCommit, CandidatePart, Candidate> { Id = partId });

            var newActualModification = await this.mediator.Send(new CancelCandidateModificationCommand { LotId = lotId });
			return await this.mediator.Send(new GetCandidateCommitQuery { LotId = newActualModification.LotId, CommitId = newActualModification.CommitId });
		}

		//[HttpPost("lot/{lotId:int}/erase")]
		//public async Task<CandidateCommitDto> EraseApplication([FromRoute] int lotId, [FromQuery] string changeStateDescription)
		//{
		//	var erasedCommitInfo = await this.mediator.Send(new EraseCommitCommand<CandidateCommit> { LotId = lotId, ChangeStateDescription = changeStateDescription });
		//	return await this.mediator.Send(new GetCandidateCommitQuery { LotId = erasedCommitInfo.LotId, CommitId = erasedCommitInfo.CommitId });
		//}

		[HttpPost("lot/{lotId:int}/reverterased")]
		public async Task<CandidateCommitDto> RevertErasedApplication([FromRoute] int lotId)
		{
			var erasedCommitInfo = await this.mediator.Send(new RevertErasedCommitCommand<CandidateCommit> { LotId = lotId });
			return await this.mediator.Send(new GetCandidateCommitQuery { LotId = erasedCommitInfo.LotId, CommitId = erasedCommitInfo.CommitId });
		}

		[HttpDelete("lot/{lotId:int}")]
		public Task DeleteApplicationLot([FromRoute] int lotId)
			=> this.mediator.Send(new DeleteCandidateLotCommand { LotId = lotId });
	}
}
