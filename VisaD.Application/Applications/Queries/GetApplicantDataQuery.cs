using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Users;

namespace VisaD.Application.Applications.Queries
{
	public class GetApplicantDataQuery : IRequest<ApplicantDto>
	{
		public int UserId { get; set; }

		public class Handler : IRequestHandler<GetApplicantDataQuery, ApplicantDto>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<ApplicantDto> Handle(GetApplicantDataQuery request, CancellationToken cancellationToken)
			{
				var applicant = await this.context.Set<User>()
					.Where(e => e.Id == request.UserId)
					.Select(ApplicantDto.SelectUserExpression)
					.SingleOrDefaultAsync(cancellationToken);

				return applicant;
			}
		}
	}
}
