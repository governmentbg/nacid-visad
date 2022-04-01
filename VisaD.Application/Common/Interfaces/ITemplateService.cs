using System.Threading.Tasks;

namespace VisaD.Application.Common.Interfaces
{
	public interface ITemplateService
    {
        Task<byte[]> GetTemplateAsync(string alias);
    }
}
