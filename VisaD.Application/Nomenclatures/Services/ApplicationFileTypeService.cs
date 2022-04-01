using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Nomenclatures.Constants;

namespace VisaD.Application.Nomenclatures.Services
{
	public class ApplicationFileTypeService : IApplicationFileTypeService
	{
		private readonly IAppDbContext context;

		public ApplicationFileTypeService(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<ApplicationFileTypeNomenclatureDto>> GetApplicationFileTypes(string qualificationName)
		{
			var fileTypes = this.context.Set<ApplicationFileType>().AsNoTracking().Where(x => x.IsActive == true);

			if (qualificationName.ToLower().Contains(EducationalQualificationAlias.BACHELOR.ToLower()))
			{
				fileTypes = fileTypes.Where(x => x.IsForBachelor == true);
			}

			if (qualificationName.ToLower() == EducationalQualificationAlias.MASTERWITHBACHELOR.ToLower())
			{
				fileTypes = fileTypes.Where(x => x.IsForMaster == true);
			}

			if (qualificationName.ToLower().Contains(EducationalQualificationAlias.DOCTORAL.ToLower()))
			{
				fileTypes = fileTypes.Where(x => x.IsForDoctor == true);
			}

			if (qualificationName.ToLower() == EducationalQualificationAlias.MASTERWITHSECONDARY.ToLower())
			{
				fileTypes = fileTypes.Where(x => x.IsForMasterWithSecondary == true);
			}

			var files = await fileTypes
				.OrderBy(x => x.ViewOrder)
				.Select(x => new ApplicationFileTypeNomenclatureDto {
					Id = x.Id,
					Name = x.Name,
					HasDate = x.HasDate,
					Description = x.Description,
					Alias = x.Alias,
					IsForBachelor = x.IsForBachelor,
					IsForDoctor = x.IsForDoctor,
					IsForMaster = x.IsForMaster,
					IsForMasterWithSecondary = x.IsForMasterWithSecondary
				})
				.ToListAsync();

			return files;
		}

		public async Task<ApplicationFileTypeNomenclatureDto> SelectApplicationFileType(string alias)
		{
			var applicationFile = await this.context.Set<ApplicationFileType>()
				.Where(x => x.Alias == alias)
				.Select(x => new ApplicationFileTypeNomenclatureDto {
					Id = x.Id,
					Name = x.Name,
					HasDate = x.HasDate,
					Description = x.Description
				})
				.SingleOrDefaultAsync();

			return applicationFile;
		}
	}
}
