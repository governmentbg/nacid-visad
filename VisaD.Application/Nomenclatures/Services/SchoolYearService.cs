using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Application.Nomenclatures.Extensions;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Nomenclatures.Services
{
	public class SchoolYearService : ISchoolYearService
	{
		private readonly IAppDbContext context;

		public SchoolYearService(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<SchoolYear> GetDefaultSchoolYearsAsync(CancellationToken cancellationToken)
		{
            var schoolYear = await this.context.Set<SchoolYear>()
                .SingleOrDefaultAsync(x => x.FromYear == 0, cancellationToken);

            return schoolYear;
        }

		public async Task<IEnumerable<SchoolYear>> SelectSchoolYearsAsync(CancellationToken cancellationToken)
        {
            var currentYear = DateTime.UtcNow.Year;

            var result = await this.context.Set<SchoolYear>()
                .AsNoTracking()
                .Where(e => e.ToYear == currentYear || e.FromYear == currentYear)
                .OrderBy(e => e.ViewOrder)
                .ToListAsync(cancellationToken);

            return result;
        }
	}
}
