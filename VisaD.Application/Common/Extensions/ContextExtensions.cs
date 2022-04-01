using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Application.Common.Extensions
{
	public static class ContextExtensions
	{
		public static async Task<T> ExecuteRawSqlScalarAsync<T>(this IAppDbContext dbContext, string sql, IDictionary<string, object> sqlParams = null)
		where T : struct
		{
			var context = dbContext as DbContext;

			var connection = context.Database.GetDbConnection();
			if (connection == null)
			{
				throw new ArgumentNullException("connection", "Invalid db connection");
			}


			using (var command = connection.CreateCommand())
			{
				command.CommandText = sql;
				command.CommandType = CommandType.Text;
				command.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
				if (command.Transaction == null)
				{
					command.Connection.Open();
				}

				if (sqlParams != null)
				{
					foreach (var sqlParam in sqlParams)
					{
						var param = command.CreateParameter();
						param.ParameterName = sqlParam.Key;
						param.Value = sqlParam.Value;

						command.Parameters.Add(param);
					}
				}


				var result = await command.ExecuteScalarAsync();
				return result != DBNull.Value ? (T)result : (T)default;
			}
		}
	}
}
