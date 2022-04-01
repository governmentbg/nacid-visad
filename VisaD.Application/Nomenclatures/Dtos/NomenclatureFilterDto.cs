using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class NomenclatureFilterDto<TNomenclature> : BaseNomenclatureFilterDto<TNomenclature>
		where TNomenclature : Nomenclature
	{
		public int? EntityId { get; set; }

		public override ICollection<Expression<Func<TNomenclature, object>>> Orders => new List<Expression<Func<TNomenclature, object>>> {
			e => e.ViewOrder,
			e => e.Id
		};
	}
}
