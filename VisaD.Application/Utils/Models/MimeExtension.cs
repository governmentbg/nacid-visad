namespace VisaD.Application.Utils.Models
{
	public class MimeExtension
	{
		public string Extension { get; }
		public string MimeType { get; }

		public MimeExtension(string extension, string mimeType)
		{
			Extension = extension;
			MimeType = mimeType;
		}
	}
}
