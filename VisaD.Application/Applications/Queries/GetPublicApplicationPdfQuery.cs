using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries
{
	public class GetPublicApplicationPdfQuery : IRequest<string>
	{
		public string AccessCode { get; set; }

		public class Handler : IRequestHandler<GetPublicApplicationPdfQuery, string>
		{
			private readonly IAppDbContext context;
			private readonly DomainValidationService validation;
			private readonly AuthConfiguration authConfiguration;

			public Handler(
				IAppDbContext context, 
				DomainValidationService validation, 
				IOptions<AuthConfiguration> options)
			{
				this.context = context;
				this.validation = validation;
				this.authConfiguration = options.Value;
			}
			public async Task<string> Handle(GetPublicApplicationPdfQuery request, CancellationToken cancellationToken)
			{
				var applicationResult = await this.context.Set<ApplicationLotResult>()
					.Include(a => a.File)
					.SingleOrDefaultAsync(a => a.AccessCode == request.AccessCode, cancellationToken);

				if (applicationResult == null)
				{
					this.validation.ThrowErrorMessage(ApplicationErrorCode.Application_InvalidAccessCode);
				}

				if(applicationResult.Type == Data.Applications.Enums.ApplicationLotResultType.Annulled)
                {
					this.validation.ThrowErrorMessage(ApplicationErrorCode.Application_Annulled);
				}

				var pdfUrl = $"{this.authConfiguration.Issuer}/api/FilesStorage?key={applicationResult.File.Key}&fileName={applicationResult.File.Name}&dbId={applicationResult.File.DbId}";

				return pdfUrl;
			}
		}
	}
}
