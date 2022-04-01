using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VisaD.Application.Nomenclatures.Dtos;

namespace VisaD.Application.InstitutionSpecialities.Dtos
{
	public class SpecialityFilterDto : BaseNomenclatureFilterDto<SpecialityInformationDto>
	{
		public int? EntityId { get; set; }

		public int? FacultyId {get; set;}

		public override ICollection<Expression<Func<SpecialityInformationDto, object>>> Orders => new List<Expression<Func<SpecialityInformationDto, object>>> {
			e => e.Name,
			e => e.Id
		};
	}
}
