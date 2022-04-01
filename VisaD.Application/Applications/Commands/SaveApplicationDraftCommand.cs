using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands
{
	public class SaveApplicationDraftCommand : IRequest<Unit>
	{
		public int DraftId { get; set; }

		public ApplicationDraftDto Draft { get; set; }

		public class Handler : IRequestHandler<SaveApplicationDraftCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly DomainValidationService validationService;

			public Handler(IAppDbContext context, DomainValidationService validationService)
			{
				this.context = context;
				this.validationService = validationService;
			}

			public async Task<Unit> Handle(SaveApplicationDraftCommand request, CancellationToken cancellationToken)
			{
				var draft = await this.context.Set<ApplicationDraft>()
					.SingleOrDefaultAsync(e => e.Id == request.DraftId, cancellationToken);

				if (draft == null)
				{
					this.validationService.ThrowErrorMessage(ApplicationErrorCode.Application_NotFound);
				}

				draft.Content = request.Draft.Content;
				draft.ModificationDate = DateTime.Now;

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
