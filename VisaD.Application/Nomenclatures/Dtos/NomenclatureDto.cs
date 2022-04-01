using System;
using System.Linq.Expressions;
using VisaD.Application.Common.Dtos;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class NomenclatureDto<TNomenclature> : IMapping<TNomenclature, NomenclatureDto<TNomenclature>>
		where TNomenclature : Nomenclature
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public virtual Expression<Func<TNomenclature, NomenclatureDto<TNomenclature>>> Map()
		{
			return e => new NomenclatureDto<TNomenclature> {
				Id = e.Id,
				Name = e.Name
			};
		}
	}
}
