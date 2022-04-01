using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public abstract class BaseNomenclatureFilterDto<TNomenclature>
			where TNomenclature : class
	{
		public string TextFilter { get; set; }
		public bool? IncludeInactive { get; set; }

		public int? Limit { get; set; } = 10;
		public int? Offset { get; set; } = 0;

		public abstract ICollection<Expression<Func<TNomenclature, object>>> Orders { get; }
	}
}
