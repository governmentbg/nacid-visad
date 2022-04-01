using System.ComponentModel;

namespace VisaD.Data.Users.Enums
{
	public enum FeedbackSubject
	{
		[Description("Въпрос")]
		Question = 1,

		[Description("Предложение")]
		Suggestion = 2,

		[Description("Технически проблем")]
		TechnicalIssue = 3
	}
}
