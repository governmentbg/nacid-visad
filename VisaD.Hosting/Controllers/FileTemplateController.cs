using Microsoft.AspNetCore.Mvc;
using VisaD.Application.Common.Interfaces;
using VisaD.Data;
using VisaD.Hosting.Controllers.Common;

namespace VisaD.Hosting.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FileTemplateController : BaseEntityController<FileTemplate>
	{
		public FileTemplateController(IAppDbContext context)
			: base(context)
		{

		}
	}
}
