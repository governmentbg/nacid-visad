using VisaD.Data.Applications.Register;
using VisaD.Infrastructure.Ems.Models;

namespace VisaD.Application.Ems.Converters
{
	public interface IEmsApplicationConverter
    {
        EmsApplication ToEmsApplication(string electornicServiceUri, ApplicantPart model, string regNumber, ApplicationLotResultFile file, bool hasParent);
    }
}
