using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Application.Common.Commands
{
	public class GeneratePdfCommand : IRequest<byte[]>
	{
		public object Items { get; set; }
		public string TemplateAlias { get; set; }

		public class Handler : IRequestHandler<GeneratePdfCommand, byte[]>
		{
			private readonly IMediator mediator;
			private readonly ITemplateService templateService;
			private readonly IPdfFileService applicationFileService;

			public Handler(
				IMediator mediator,
				ITemplateService templateService,
				IPdfFileService applicationFileService
			)
			{
				this.mediator = mediator;
				this.templateService = templateService;
				this.applicationFileService = applicationFileService;
			}

			public async Task<byte[]> Handle(GeneratePdfCommand request, CancellationToken cancellationToken)
			{
				var template = await this.templateService.GetTemplateAsync(request.TemplateAlias);
				var stream = await this.applicationFileService.GeneratePdfFile(new { Items = request.Items }, template);
				return stream.ToArray();
			}
		}
	}
}
