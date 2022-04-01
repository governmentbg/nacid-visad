using FileStorageNetCore;
using FileStorageNetCore.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Hosting.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FilesStorageController : FileStorageController
	{
		private readonly IImageFileService imageFileService;

		public FilesStorageController(BlobStorageService service, IImageFileService imageFileService)
			: base(service)
		{
			this.imageFileService = imageFileService;
		}

		[HttpGet("image")]
		public async Task<string> GetImage([FromQuery] Guid key, [FromQuery] int dbId)
		{
			return await this.imageFileService.GetBase64ImageUrlAsync(key, dbId);
		}
	}
}
