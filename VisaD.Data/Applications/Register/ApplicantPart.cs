using VisaD.Data.Common.Models;

namespace VisaD.Data.Applications.Register
{
	public class ApplicantPart : Part<Applicant>
	{
		public ApplicantPart()
			:base()
		{

		}

		public ApplicantPart(ApplicantPart part)
			:base(part)
		{

		}
	}
}
