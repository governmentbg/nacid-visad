using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VisaD.Application.Common.Behaviours
{
	public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
			where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> validators;

		public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			this.validators = validators;
		}

		public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var context = new ValidationContext<TRequest>(request);
			var failures = validators
				.Select(e => e.Validate(context))
				.SelectMany(e => e.Errors)
				.Where(e => e != null)
				.ToList();
			if (failures.Any())
			{
				throw new ValidationException(failures);
			}

			return next();
		}
	}
}
