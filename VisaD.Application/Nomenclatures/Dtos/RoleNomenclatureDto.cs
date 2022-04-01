using System;
using System.Linq.Expressions;
using VisaD.Application.Common.Dtos;
using VisaD.Data.Users;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class RoleNomenclatureDto : NomenclatureDto<Role>, IMapping<Role, RoleNomenclatureDto>
	{
		public string Alias { get; set; }

		public new Expression<Func<Role, RoleNomenclatureDto>> Map()
		{
			return e => new RoleNomenclatureDto {
				Id = e.Id,
				Name = e.Name,
				Alias = e.Alias
			};
		}
	}
}
