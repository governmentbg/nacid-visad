using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Commands
{
	public class CreateDraftCommand : IRequest<Unit>
	{
		public ApplicationDraftDto Draft { get; set; }

		public class Handler : IRequestHandler<CreateDraftCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly IUserContext userContext;

			public Handler(IAppDbContext context, IUserContext userContext)
			{
				this.context = context;
				this.userContext = userContext;
			}

			public async Task<Unit> Handle(CreateDraftCommand request, CancellationToken cancellationToken)
			{
				var applicationDraft = new ApplicationDraft {
					Content = request.Draft.Content,
					UserId = this.userContext.UserId,
					CreationDate = DateTime.Now
				};

				this.context.Set<ApplicationDraft>().Add(applicationDraft);

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
