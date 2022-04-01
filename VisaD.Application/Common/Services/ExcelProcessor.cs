using EnumsNET;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using VisaD.Application.Applications.Queries;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Application.Common.Services
{
	public class ExcelProcessor : IExcelProcessor
    {
        private readonly IEnumUtility utility;

        public ExcelProcessor(IEnumUtility utility)
        {
            this.utility = utility;
        }
        public MemoryStream Export<T, TResult>(IEnumerable<T> list, params Expression<Func<T, TResult>>[] expr)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                var headers = new List<string>();
                var memberExpressions = new List<MemberExpression>();
                GetHeadersAndMembers(out headers, out memberExpressions, expr);

                bool[] isFormatedMaxCols = new bool[headers.Count];

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("GeneratedWorksheet");

                int col = 1, row = 1;
                foreach (var header in headers)
                {
                    worksheet.Cells[row, col].Value = header;
                    worksheet.Cells[row, col].Style.Font.Bold = true;
                    col++;
                }

                foreach (var item in list)
                {
                    col = 1;
                    row++;
                    foreach (var memberExpression in memberExpressions)
                    {
                        object value = null;
                        if (memberExpression.Expression.Type == typeof(T))
                        {
                            value = item.GetType().GetProperty(memberExpression.Member.Name).GetValue(item, null);
                        }
                        else
                        {
                            var resultValue = GetNestedProperties(item, memberExpression.Expression.ToString().Substring(2));
                            if (resultValue == null)
                            {
                                value = null;
                            }
                            else
                            {
                                value = resultValue.GetType().GetProperty(memberExpression.Member.Name).GetValue(resultValue, null);
                            }
                        }

                        if (value != null
                            && value.GetType().BaseType == typeof(Enum))
                        {
                            worksheet.Cells[row, col].Value = utility.GetDescription(value);
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = value;
                        }

                        var fieldType = memberExpression.Type;
                        if (fieldType == typeof(Boolean)
                            || fieldType == typeof(Boolean?))
                        {
                            var obj = (bool)worksheet.Cells[row, col].Value;
                            string boolValue = "Не";
                            if (obj)
                            {
                                boolValue = "Да";
                            }
                            worksheet.Cells[row, col].Value = boolValue;
                        }

                        worksheet.Cells[row, col].Style.Numberformat.Format = GetCellFormatting(fieldType);

                        if (!isFormatedMaxCols[col - 1]
                            && worksheet.Cells[row, col].Value != null)
                        {
                            int cellSize = worksheet.Cells[row, col].Value.ToString().Length;
                            if (cellSize > 80)
                            {
                                worksheet.Column(col).Width = 80;
                                worksheet.Column(col).Style.WrapText = true;
                                isFormatedMaxCols[col - 1] = true;
                            }
                        }
                        col++;
                    }

                }

                for (int i = 0; i <= headers.Count - 1; i++)
                {
                    if (!isFormatedMaxCols[i])
                    {
                        worksheet.Column(i + 1).AutoFit();
                    }
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }

        private object GetNestedProperties(object original, string properties)
        {

            string[] namesOfProperties = properties.Split('.');
            int size = namesOfProperties.Length - 1;

            PropertyInfo property = original.GetType().GetProperty(namesOfProperties[0]);
            object propValue = property.GetValue(original, null);

            for (int i = 1; i <= size; i++)
            {
                property = propValue.GetType().GetProperty(namesOfProperties[i]);
                propValue = property.GetValue(propValue, null);
            }

            return propValue;
        }

        private void GetHeadersAndMembers<T, TResult>(out List<string> headers, out List<MemberExpression> memberExpressions, params Expression<Func<T, TResult>>[] expressions)
        {
            headers = new List<string>();
            memberExpressions = new List<MemberExpression>();

            foreach (var item in expressions)
            {
                var expression = item.Body as MemberInitExpression;
                var bindings = expression.Bindings;

                foreach (var binding in bindings)
                {
                    dynamic obj = binding;

                    var member = obj.Expression as MemberExpression;
                    var unary = obj.Expression as UnaryExpression;
                    var result = member ?? (unary != null ? unary.Operand as MemberExpression : null);

                    if (result == null)
                    {
                        headers.Add(obj.Expression.Value);
                    }
                    else
                    {
                        memberExpressions.Add(result);
                    }
                }
            }
        }

        private string GetCellFormatting(Type fieldType)
        {
            if (fieldType == typeof(DateTime)
                || fieldType == typeof(DateTime?))
            {
                return "dd-mm-yyyy";
            }
            else if (fieldType == typeof(Double)
                || fieldType == typeof(Double?)
                || fieldType == typeof(Decimal)
                || fieldType == typeof(Decimal?))
            {
                return "0.00";
            }

            return null;
        }

		public MemoryStream ExportReports<T, TResult>(GenerateReportCommitQuery filter, IEnumerable<T> list, params Expression<Func<T, TResult>>[] expr)
		{
			using (ExcelPackage package = new ExcelPackage())
            {
                var headers = new List<string>();
                var memberExpressions = new List<MemberExpression>();
                GetHeadersAndMembers(out headers, out memberExpressions, expr);

                bool[] isFormatedMaxCols = new bool[headers.Count];

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("GeneratedWorksheet");

                worksheet.Cells["A1:B1"].Merge = true;
                worksheet.Cells[1, 1].Value = "Учебна година: ";
                worksheet.Cells[1, 1].Style.Font.Bold = true;

                worksheet.Cells["C1:E1"].Merge = true;
                worksheet.Cells[1, 3].Value = filter.SchoolYearName ?? "Всички";

                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells[2, 1].Value = "Вид справка: ";
                worksheet.Cells[2, 1].Style.Font.Bold = true;

                worksheet.Cells["C2:E2"].Merge = true;
                worksheet.Cells[2, 3].Value = filter.ReportType.AsString(EnumFormat.Description);

                worksheet.Cells["A3:B3"].Merge = true;
                worksheet.Cells[3, 1].Value = "Дата на справката: ";
                worksheet.Cells[3, 1].Style.Font.Bold = true;

                worksheet.Cells["C3:E3"].Merge = true;
                worksheet.Cells[3, 3].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "ч.";

                worksheet.Cells["A4:B4"].Merge = true;
                worksheet.Cells[4, 1].Value = "Висше училище: ";
                worksheet.Cells[4, 1].Style.Font.Bold = true;

                worksheet.Cells["C4:E4"].Merge = true;
                worksheet.Cells[4, 3].Value = filter.InstitutionName ?? "Всички";

                worksheet.Cells["A5:B5"].Merge = true;
                worksheet.Cells[5, 1].Value = "Гражданство: ";
                worksheet.Cells[5, 1].Style.Font.Bold = true;

                worksheet.Cells["C5:E5"].Merge = true;
                worksheet.Cells[5, 3].Value = filter.NationalityName ?? "Всички";

                worksheet.Cells["A6:B6"].Merge = true;
                worksheet.Cells[6, 1].Value = "Държава на раждане: ";
                worksheet.Cells[6, 1].Style.Font.Bold = true;

                worksheet.Cells["C6:E6"].Merge = true;
                worksheet.Cells[6, 3].Value = filter.CountryName ?? "Всички";

                worksheet.Cells["A7:B7"].Merge = true;
                worksheet.Cells[7, 1].Value = "ОКС: ";
                worksheet.Cells[7, 1].Style.Font.Bold = true;

                worksheet.Cells["C7:E7"].Merge = true;
                worksheet.Cells[7, 3].Value = filter.EducationalQualificationName ?? "Всички";

                worksheet.Cells["A8:E8"].Merge = true;

                int col = 1, row = 9;
                foreach (var header in headers)
                {
                    worksheet.Cells[row, col].Value = header;
                    worksheet.Cells[row, col].Style.Font.Bold = true;
                    col++;
                }

                foreach (var item in list)
                {
                    col = 1;
                    row++;
                    foreach (var memberExpression in memberExpressions)
                    {
                        object value = null;
                        if (memberExpression.Expression.Type == typeof(T))
                        {
                            value = item.GetType().GetProperty(memberExpression.Member.Name).GetValue(item, null);
                        }
                        else
                        {
                            var resultValue = GetNestedProperties(item, memberExpression.Expression.ToString().Substring(2));
                            if (resultValue == null)
                            {
                                value = null;
                            }
                            else
                            {
                                value = resultValue.GetType().GetProperty(memberExpression.Member.Name).GetValue(resultValue, null);
                            }
                        }

                        if (value != null
                            && value.GetType().BaseType == typeof(Enum))
                        {
                            worksheet.Cells[row, col].Value = utility.GetDescription(value);
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = value;
                        }

                        var fieldType = memberExpression.Type;
                        if (fieldType == typeof(Boolean)
                            || fieldType == typeof(Boolean?))
                        {
                            var obj = (bool)worksheet.Cells[row, col].Value;
                            string boolValue = "Не";
                            if (obj)
                            {
                                boolValue = "Да";
                            }
                            worksheet.Cells[row, col].Value = boolValue;
                        }

                        worksheet.Cells[row, col].Style.Numberformat.Format = GetCellFormatting(fieldType);

                        if (!isFormatedMaxCols[col - 1]
                            && worksheet.Cells[row, col].Value != null)
                        {
                            int cellSize = worksheet.Cells[row, col].Value.ToString().Length;
                            if (cellSize > 80)
                            {
                                worksheet.Column(col).Width = 80;
                                worksheet.Column(col).Style.WrapText = true;
                                isFormatedMaxCols[col - 1] = true;
                            }
                        }
                        col++;
                    }

                }

                for (int i = 0; i <= headers.Count - 1; i++)
                {
                    if (!isFormatedMaxCols[i])
                    {
                        worksheet.Column(i + 1).AutoFit();
                    }
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
		}
	}
}
