using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VisaD.Application.Common.Commands;
using VisaD.Application.Common.Constants;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Users.Commands;
using VisaD.Application.Users.Dtos;
using VisaD.Application.Users.Queries;
using VisaD.Application.Utils;
using VisaD.Data.Users.Enums;
using VisaD.Hosting.Infrastructure.Auth;

namespace VisaD.Hosting.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IExcelProcessor excelProcessor;

        public UserController(
            IMediator mediator,
            IExcelProcessor excelProcessor
        )
        {
            this.mediator = mediator;
            this.excelProcessor = excelProcessor;
        }

        [HttpGet]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public Task<SearchResultItemDto<UserSearchResultDto>> SearchUsers([FromQuery] UserSearchQuery filter)
            => this.mediator.Send(filter);

        [HttpPost("Excel")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public async Task<FileStreamResult> ExportUsersFiltered([FromBody] UserSearchQuery query)
        {
            query.Limit = int.MaxValue;
            query.Offset = 0;

            var searchResult = await this.mediator.Send(query);

            var excelStream = excelProcessor.Export(searchResult.Items,
                e => new ExcelTableTuple { CellItem = e.FullName, ColumnName = "Имена" },
                e => new ExcelTableTuple { CellItem = e.Role, ColumnName = "Вид потребител" },
                e => new ExcelTableTuple { CellItem = e.InstitutionName, ColumnName = "Организация" },
                e => new ExcelTableTuple { CellItem = e.Username, ColumnName = "Имейл" },
                e => new ExcelTableTuple { CellItem = e.Phone, ColumnName = "Телефон" },
                e => new ExcelTableTuple { CellItem = e.Status, ColumnName = "Статус" }
            );
            return new FileStreamResult(excelStream, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.OOXML_EXCEL).MimeType) { FileDownloadName = "Users.xlsx" };
        }

        [HttpPost("PDF")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public async Task<FileContentResult> ExportApplicationFilteredPdf([FromBody] UserSearchQuery query)
        {
            query.Limit = int.MaxValue;
            query.Offset = 0;

            var users = await this.mediator.Send(query);

            var bytes = await this.mediator.Send(new GeneratePdfCommand {
                Items = users.Items,
                TemplateAlias = FileTemplateAliases.USERS_EXPORT
            });
            return new FileContentResult(bytes, MimeTypeHelper.GetExtensionWithMime(MimeTypeHelper.PDF).MimeType) { FileDownloadName = "Users.pdf" };
        }

        [HttpPost]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public async Task<int> CreateUser([FromBody] CreateUserCommand model)
        {
            var userId = await this.mediator.Send(model);

            await this.mediator.Send(new SendUserActivationLinkCommand { Id = userId });

            return userId;
        }

        [HttpGet("{id:int}")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public Task<UserEditDto> GetUserById([FromRoute] int id)
            => this.mediator.Send(new GetUserByIdQuery { Id = id });

        [HttpPut]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
        public Task UpdateUserData([FromBody] UserEditDto model)
            => this.mediator.Send(new EditUserDataCommand { User = model });

        [HttpPost("NewPassword")]
        public Task ChangePassword([FromBody] ChangeUserPasswordCommand command)
            => this.mediator.Send(command);

        [HttpPut("changeStatus")]
        public async Task<UserStatus> ChangeUserActivation([FromBody] int id)
            => await this.mediator.Send(new ChangeUserActiveStatusCommand { Id = id });
    }
}
