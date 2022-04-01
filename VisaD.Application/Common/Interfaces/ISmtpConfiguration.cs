namespace VisaD.Application.Common.Interfaces
{
	public interface ISmtpConfiguration
	{
		string SmtpHost { get; set; }
		int SmtpPort { get; set; }
		bool SmtpUseSsl { get; set; }
		bool SmtpShouldAuthenticate { get; set; }
		string SmtpUsername { get; set; }
		string SmtpPassword { get; set; }
	}
}
