using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries
{
	public class GetApplicationDraftsQuery : IRequest<IEnumerable<ApplicationDraftDto>>
	{
		public class Handler : IRequestHandler<GetApplicationDraftsQuery, IEnumerable<ApplicationDraftDto>>
		{
			private readonly IAppDbContext context;
			private readonly IUserContext userContext;

			public Handler(IAppDbContext context, IUserContext userContext)
			{
				this.context = context;
				this.userContext = userContext;
			}

			public async Task<IEnumerable<ApplicationDraftDto>> Handle(GetApplicationDraftsQuery request, CancellationToken cancellationToken)
			{
				var drafts = await this.context.Set<ApplicationDraft>()
					.Where(e => e.UserId == this.userContext.UserId)
					.Select(e => new ApplicationDraftDto { 
						Content = e.Content,
						Id = e.Id
					})
					.ToListAsync();

				return drafts;
			}
		}
	}
}
