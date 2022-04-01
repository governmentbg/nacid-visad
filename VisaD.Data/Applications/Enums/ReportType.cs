using System.ComponentModel;

namespace VisaD.Data.Applications.Enums
{
	public enum ReportType
	{
		[Description("Общ брой заявления")]
		DefaultReport = 1,

		[Description("По висше училище")]
		ReportByInstitution = 2,

		[Description("По гражданство")]
		ReportByNationality = 3,

		[Description("По месторождение")]
		ReportByCountry = 4,

		[Description("По ОКС")]
		ReportByEducationalQualification = 5,

		[Description("С повече от 1 издадено удостоверение")]
		ReportByCandidateWithMoreThanOneCertificate = 6,
	}
}
