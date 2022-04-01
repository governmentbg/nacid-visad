using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VisaD.Application.Common.Behaviours
{
	public class CommandTransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
			where TRequest : IRequest<TResponse>
	{
		//private readonly DbContext context;

		//public CommandTransactionBehaviour(DbContext context)
		//{
		//	this.context = context;
		//}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			if (!request.GetType().Name.Contains("Command")) 
				//|| context.Database.CurrentTransaction != null)
			{
				return await next();
			}

			//using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
			//{
				try
				{
					var response = await next();

					//transaction.Commit();

					return response;
				}
				catch (Exception)
				{
					//transaction.Rollback();
					throw;
				}
			//}
		}
	}
}
