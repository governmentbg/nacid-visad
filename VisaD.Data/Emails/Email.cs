using System.Collections.Generic;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Emails
{
	public class Email : IEntity
	{
		public int Id { get; set; }

		public int TypeId { get; set; }

		public EmailType Type { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public IList<EmailAddressee> Addressees { get; set; } = new List<EmailAddressee>();

		private Email()
		{

		}

		public Email(int typeId, string subject, string body)
		{
			this.TypeId = typeId;
			this.Subject = subject;
			this.Body = body;
		}
	}
}
