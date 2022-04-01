using System;
using System.Threading.Tasks;
using VisaD.Data;

namespace VisaD.Application.Common.Interfaces
{
	public interface IImageFileService
	{
		Task<string> GetBase64ImageUrlAsync(Guid key, int dbId);
	}
}
