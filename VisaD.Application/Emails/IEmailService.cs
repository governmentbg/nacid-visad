using System.Collections.Generic;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Emails;

namespace VisaD.Application.Emails
{
	public interface IEmailService
	{
		IEnumerable<Email> GetPendingEmails(int limit);

		Task<Email> ComposeEmailAsync(string alias, object templateData, params string[] recipients);

		bool SendEmail(Email email, IEmailConfiguration emailConfiguration);
	}
}
