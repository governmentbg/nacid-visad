using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VisaD.Application.Common.Extensions
{
	public class ExpressionHelper
	{
		public static Expression<Func<T, bool>> BuildOrStringExpression<T>(string propertyName, List<string> options)
		{
			Expression<Func<T, bool>> resultExpression = null;
			ParameterExpression param = null;

			foreach (var option in options)
			{
				var expression = ExpressionHelper.GetStringContainsExpression<T>(propertyName, option);
				if (resultExpression is null)
				{
					resultExpression = Expression.Lambda<Func<T, bool>>(expression.Body, expression.Parameters);
					param = resultExpression.Parameters[0];
					continue;
				}

				// Works only for 1 param
				if (ReferenceEquals(param, expression.Parameters[0]))
				{
					resultExpression = Expression.Lambda<Func<T, bool>>(Expression.Or(resultExpression.Body, expression.Body), param);
				}
				else
				{
					resultExpression = Expression.Lambda<Func<T, bool>>(Expression.Or(resultExpression.Body, Expression.Invoke(expression, param)), param);
				}
			}

			return resultExpression;
		}

		private static Expression<Func<T, bool>> GetStringContainsExpression<T>(string propertyName, string propertyValue)
		{
			var parameterExp = Expression.Parameter(typeof(T), propertyName.ToLower().First().ToString());

			var propertyExp = Expression.Property(parameterExp, propertyName);

			var someValue = Expression.Constant(propertyValue, typeof(string));

			MethodInfo trimMethod = typeof(string).GetMethod(nameof(string.Trim), new Type[] { });
			var trimMethodExp = Expression.Call(propertyExp, trimMethod);

			MethodInfo toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), new Type[] { });
			var toLowerMethodExp = Expression.Call(trimMethodExp, toLowerMethod);

			MethodInfo containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
			var containsMethodExp = Expression.Call(toLowerMethodExp, containsMethod, someValue);

			return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
		}
	}
}
