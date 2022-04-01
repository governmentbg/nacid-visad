using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Application.Users.Dtos;
using VisaD.Data.Users;

namespace VisaD.Application.Users.Queries
{
	public class GetUserByIdQuery : IRequest<UserEditDto>
	{
		public int Id { get; set; }

		public class Handler : IRequestHandler<GetUserByIdQuery, UserEditDto>
		{
			private readonly IAppDbContext context;
			private readonly DomainValidationService validationService;

			public Handler(IAppDbContext context, DomainValidationService validationService)
			{
				this.context = context;
				this.validationService = validationService;
			}

			public async Task<UserEditDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
			{
				var user = await this.context.Set<User>()
					.Where(e => e.Id == request.Id)
					.Select(UserEditDto.SelectExpression)
					.SingleOrDefaultAsync();

				if (user == null)
				{
					this.validationService.ThrowErrorMessage(UserErrorCode.User_NotFound);
				}

				return user;
			}
		}
	}
}
