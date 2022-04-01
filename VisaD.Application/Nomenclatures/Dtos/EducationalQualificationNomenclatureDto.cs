using System;
using System.Linq.Expressions;
using VisaD.Application.Common.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class EducationalQualificationNomenclatureDto : NomenclatureDto<EducationalQualification>, IMapping<EducationalQualification, EducationalQualificationNomenclatureDto>
	{
		public string Alias { get; set; }

		public new Expression<Func<EducationalQualification, EducationalQualificationNomenclatureDto>> Map()
		{
			return e => new EducationalQualificationNomenclatureDto {
				Id = e.Id,
				Name = e.Name,
				Alias = e.Alias
			};
		}
	}
}
