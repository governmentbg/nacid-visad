using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VisaD.Application.Common.Constants;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Application.Nomenclatures.Services;
using VisaD.Data.Common.Models;
using VisaD.Hosting.Infrastructure.Auth;

namespace VisaD.Hosting.Controllers.Common
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseNomenclatureController<T, TDto, TFilter> : ControllerBase
		where T : Nomenclature
		where TDto: IMapping<T,TDto>, new()
		where TFilter: BaseNomenclatureFilterDto<T>
	{
		protected readonly INomenclatureService<T> nomenclatureService;

		public BaseNomenclatureController(INomenclatureService<T> nomenclatureService)
		{
			this.nomenclatureService = nomenclatureService;
		}

		[HttpGet]
		public Task<IEnumerable<T>> GetNomenclatures([FromQuery] TFilter filter)
			=> this.nomenclatureService.GetNomenclaturesAsync(filter);

		[HttpGet("Select")]
		public Task<IEnumerable<TDto>> SelectNomenclatures([FromQuery] TFilter filter)
			=> this.nomenclatureService.SelectNomenclaturesAsync<TFilter, TDto>(filter);

		[HttpPost]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
		public Task<T> AddNomenclature([FromBody] T model)
			=> this.nomenclatureService.InsertNomenclatureAsync(model);

		[HttpPut("{id:int}")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
		public Task<T> UpdateNomenclature([FromRoute]int id, [FromBody] T model)
			=> this.nomenclatureService.UpdateNomenclatureAsync(id, model);

		[HttpDelete("{id:int}")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
		public Task DeleteNomenclature([FromRoute] int id)
			=> this.nomenclatureService.DeleteNomenclatureAsync(id);
	}
}
