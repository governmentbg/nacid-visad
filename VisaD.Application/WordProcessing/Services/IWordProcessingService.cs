using System.IO;

namespace VisaD.Application.WordProcessing.Services
{
    public interface IWordProcessingService
    {
        MemoryStream PopulateTemplate(byte[] content, object data);
    }
}
