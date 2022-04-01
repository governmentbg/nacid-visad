using FileStorageNetCore.Api;
using System;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Application.Common.Services
{
	public class ImageFileService : IImageFileService
	{
		private readonly BlobStorageService fileStorageRepository;

		public ImageFileService(BlobStorageService fileStorageRepository)
		{
			this.fileStorageRepository = fileStorageRepository;
		}

		public async Task<string> GetBase64ImageUrlAsync(Guid key, int dbId)
		{
			var image = await this.fileStorageRepository.GetBytes(key, dbId);
			string base64ImageUrl;

			if (image != null)
			{
				base64ImageUrl = Convert.ToBase64String(image);
			}
			else
			{
				return null;
			}

			return base64ImageUrl;
		}
	}
}
