using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VisaD.Application.Users.Commands
{
	public class GetUnauthorizedUserInformationCommand : IRequest<UnauthorizedUserInfoDto>
	{
		public class Handler : IRequestHandler<GetUnauthorizedUserInformationCommand, UnauthorizedUserInfoDto>
		{
			private readonly IHttpContextAccessor httpContextAccessor;

			public Handler(IHttpContextAccessor httpContextAccessor)
			{
				this.httpContextAccessor = httpContextAccessor;
			}

			public Task<UnauthorizedUserInfoDto> Handle(GetUnauthorizedUserInformationCommand request, CancellationToken cancellationToken)
			{
				var result = new UnauthorizedUserInfoDto {
					Ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
					Date = DateTime.Now
				};

				return Task.FromResult(result);
			}
		}
	}
}
