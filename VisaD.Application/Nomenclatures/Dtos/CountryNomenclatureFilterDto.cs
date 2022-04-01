using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class CountryNomenclatureFilterDto : BaseNomenclatureFilterDto<Country>
	{
		public override ICollection<Expression<Func<Country, object>>> Orders => new List<Expression<Func<Country, object>>> {
			e => e.Name,
			e => e.ViewOrder,
			e => e.Id
		};
	}
}
