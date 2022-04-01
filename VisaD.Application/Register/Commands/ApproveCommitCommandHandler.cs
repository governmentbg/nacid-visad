using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Commands;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Models;
using VisaD.Data.Nomenclatures.Constants;

namespace VisaD.Application.Register.Commands
{
	public class ApproveCommitCommandHandler<TCommit> : IRequestHandler<ApproveCommitCommand<TCommit>, CommitInfoDto>
		where TCommit : Commit
	{
		protected readonly IAppDbContext context;
		private readonly IMediator mediator;
		private readonly AuthConfiguration authConfiguration;

		public ApproveCommitCommandHandler(
			IAppDbContext context,
			IOptions<AuthConfiguration> authOptions,
			IMediator mediator
			)
		{
			this.context = context;
			this.mediator = mediator;
			this.authConfiguration = authOptions.Value;
		}

		public async Task<CommitInfoDto> Handle(ApproveCommitCommand<TCommit> request, CancellationToken cancellationToken)
		{
			var actualCommit = await context.Set<TCommit>()
				.SingleAsync(e => e.LotId == request.LotId && (e.State == CommitState.Actual || e.State == CommitState.RefusedSign), cancellationToken);
			actualCommit.State = CommitState.Approved;
			actualCommit.ChangeStateDescription = null;

			var templateData = new {
				ApplicationLink = $"{this.authConfiguration.Issuer}/application/lot/{actualCommit.LotId}/commit/{actualCommit.Id}"
			};

			await this.mediator.Send(new SendApplicationEmailCommand {
				Alias = EmailTypeAlias.SIGN_APPLICATION,
				TemplateData = templateData
			});

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = actualCommit.LotId,
				CommitId = actualCommit.Id
			};
		}
	}
}
