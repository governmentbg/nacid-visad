using System;
using System.Linq.Expressions;
using VisaD.Application.Common.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class DiplomaTypeNomenclatureDto : NomenclatureDto<DiplomaType>, IMapping<DiplomaType, DiplomaTypeNomenclatureDto>
	{
		public bool IsNacidVerificationRequired { get; set; }

		public string Alias { get; set; }

		public new Expression<Func<DiplomaType, DiplomaTypeNomenclatureDto>> Map()
		{
			return e => new DiplomaTypeNomenclatureDto {
				Id = e.Id,
				Name = e.Name,
				IsNacidVerificationRequired = e.IsNacidVerificationRequired,
				Alias = e.Alias
			};
		}
	}
}
