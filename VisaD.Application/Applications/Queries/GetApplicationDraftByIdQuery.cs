using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries
{
	public class GetApplicationDraftByIdQuery : IRequest<ApplicationDraftDto>
	{
		public int DraftId { get; set; }

		public class Handler : IRequestHandler<GetApplicationDraftByIdQuery, ApplicationDraftDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<ApplicationDraftDto> Handle(GetApplicationDraftByIdQuery request, CancellationToken cancellationToken)
			{
				var draft = await this.context.Set<ApplicationDraft>()
					.Where(x => x.Id == request.DraftId)
					.Select(x => new ApplicationDraftDto { 
						Id = x.Id,
						Content = x.Content
					})
					.SingleOrDefaultAsync();

				return draft;
			}
		}
	}
}
