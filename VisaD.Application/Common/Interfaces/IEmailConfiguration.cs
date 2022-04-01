namespace VisaD.Application.Common.Interfaces
{
	public interface IEmailConfiguration : ISmtpConfiguration
	{
		string FromAddress { get; set; }
		string FromName { get; set; }
		bool JobEnabled { get; set; }
		int JobPeriod { get; set; }
		int JobLimit { get; set; }
	}
}
