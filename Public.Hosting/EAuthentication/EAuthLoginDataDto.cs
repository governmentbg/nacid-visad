namespace Public.Hosting.EAuthentication
{
	public class EAuthLoginDataDto
	{
		public string Egn { get; set; }
		public string Name { get; set; }
		public string ResponseStatusMessage { get; set; }
		public EAuthResponseStatus ResponseStatus { get; set; }
	}
}
