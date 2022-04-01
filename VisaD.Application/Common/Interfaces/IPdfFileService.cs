using System.IO;
using System.Threading.Tasks;
using VisaD.Application.Common.Models;

namespace VisaD.Application.Common.Interfaces
{
	public interface IPdfFileService
	{
		Task<MemoryStream> GeneratePdfFile<T>(T payload, byte[] content, bool closeStream = true);
		Task<byte[]> GenerateSignedPdfFile<T>(T payload, byte[] content, PdfSignFieldSettings signFieldSettings);
	}
}
