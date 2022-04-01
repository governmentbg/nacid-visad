using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Constants;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Emails;
using VisaD.Data.Emails;
using VisaD.Data.Nomenclatures.Constants;
using VisaD.Data.Users;

namespace VisaD.Application.Applications.Commands
{
	public class SendApplicationEmailCommand : IRequest<Unit>
	{
		public int CreatorUserId { get; set; }

		public string Alias { get; set; }

		public object TemplateData { get; set; }

		public class Handler : IRequestHandler<SendApplicationEmailCommand, Unit>
		{
			private readonly IAppDbContext context;
			private readonly IEmailService emailService;

			public Handler(IAppDbContext context, IEmailService emailService)
			{
				this.context = context;
				this.emailService = emailService;
			}

			public async Task<Unit> Handle(SendApplicationEmailCommand request, CancellationToken cancellationToken)
			{
				var receivers = new List<string>();

				if (request.Alias == EmailTypeAlias.SIGN_APPLICATION)
				{
					receivers = await this.context.Set<User>()
						.Where(x => x.Role.Alias == UserRoleAliases.RESULT_SIGNER_USER)
						.Select(x => x.Email)
						.ToListAsync(cancellationToken);
				}
				else
				{
					receivers = await this.context.Set<User>()
						.Where(x => x.Id == request.CreatorUserId)
						.Select(x => x.Email)
						.ToListAsync(cancellationToken);
				}

				Email email = await this.emailService.ComposeEmailAsync(request.Alias, request.TemplateData, receivers.ToArray());

				this.context.Set<Email>().Add(email);
				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
