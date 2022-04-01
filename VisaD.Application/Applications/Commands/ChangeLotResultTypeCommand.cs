using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands
{
	public class ChangeLotResultTypeCommand : IRequest<Unit>
	{
		public int LotId { get; set; }

		public ApplicationLotResultType Type { get; set; }

		public class Handler : IRequestHandler<ChangeLotResultTypeCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(ChangeLotResultTypeCommand request, CancellationToken cancellationToken)
			{
				var lot = await this.context.Set<ApplicationLot>()
					.Include(x => x.Result)
					.SingleOrDefaultAsync(x => x.Id == request.LotId, cancellationToken);

                try
                {
                    lot.Result.Type = request.Type;
                }
				catch(Exception)
                {
                    throw;
                }

                await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
