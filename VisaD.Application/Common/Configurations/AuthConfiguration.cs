namespace VisaD.Application.Common.Configurations
{
	public class AuthConfiguration
	{
		public string SecretKey { get; set; }

		public string Issuer { get; set; }

		public string Audience { get; set; }

		public int ValidHours { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }
	}
}
