using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using VisaD.Application.Applications.Queries;

namespace VisaD.Application.Common.Interfaces
{
	public interface IExcelProcessor
    {
        MemoryStream Export<T, TResult>(IEnumerable<T> list, params Expression<Func<T, TResult>>[] expr);

        MemoryStream ExportReports<T, TResult>(GenerateReportCommitQuery filter, IEnumerable<T> list, params Expression<Func<T, TResult>>[] expr);
    }
}
