using System.Collections.Generic;
using System.Threading.Tasks;
using VisaD.Application.Nomenclatures.Dtos;

namespace VisaD.Application.Nomenclatures.Services
{
	public interface IApplicationFileTypeService
	{
		Task<IEnumerable<ApplicationFileTypeNomenclatureDto>> GetApplicationFileTypes(string qualificationName);

		Task<ApplicationFileTypeNomenclatureDto> SelectApplicationFileType(string alias);
	}
}
