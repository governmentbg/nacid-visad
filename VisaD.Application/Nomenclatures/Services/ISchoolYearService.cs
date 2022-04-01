using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Services
{
	public interface ISchoolYearService
	{
		Task<IEnumerable<SchoolYear>> SelectSchoolYearsAsync(CancellationToken cancellationToken);

		Task<SchoolYear> GetDefaultSchoolYearsAsync(CancellationToken cancellationToken);
	}
}
