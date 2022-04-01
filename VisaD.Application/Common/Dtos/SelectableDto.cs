using System;
using System.Linq.Expressions;

namespace VisaD.Application.Common.Dtos
{
	public interface IMapping<TFrom, TTo>
	{
		public abstract Expression<Func<TFrom, TTo>> Map();
	}
}
