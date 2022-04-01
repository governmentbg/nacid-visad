using System;
using System.Linq.Expressions;
using VisaD.Application.Common.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Dtos
{
	public class ApplicationFileTypeNomenclatureDto : NomenclatureDto<ApplicationFileType>, IMapping<ApplicationFileType, ApplicationFileTypeNomenclatureDto>
	{
		public bool HasDate { get; set; }

		public bool IsForBachelor { get; set; }

		public bool IsForDoctor { get; set; }

		public bool IsForMaster { get; set; }

		public bool IsForMasterWithSecondary { get; set; }

		public string Description { get; set; }

		public string Alias { get; set; }

		public new Expression<Func<ApplicationFileType, ApplicationFileTypeNomenclatureDto>> Map()
		{
			return e => new ApplicationFileTypeNomenclatureDto {
				Id = e.Id,
				Name = e.Name,
				HasDate = e.HasDate,
				IsForBachelor = e.IsForBachelor,
				IsForDoctor = e.IsForDoctor,
				IsForMaster = e.IsForMaster,
				Description = e.Description,
				Alias = e.Alias,
				IsForMasterWithSecondary = e.IsForMasterWithSecondary
			};
		}
	}
}
