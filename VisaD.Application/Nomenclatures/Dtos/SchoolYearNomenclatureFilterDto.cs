using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class SchoolYearNomenclatureFilterDto : BaseNomenclatureFilterDto<SchoolYear>
	{
		public int? FromStartYear { get; set; }
		public int? ToStartYear { get; set; }
		public bool? IsSchoolYear { get; set; }

		public override ICollection<Expression<Func<SchoolYear, object>>> Orders => new List<Expression<Func<SchoolYear, object>>> {
			e => e.FromYear,
			e => e.Id
		};
	}
}
