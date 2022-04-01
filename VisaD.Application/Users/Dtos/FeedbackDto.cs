using VisaD.Data.Users.Enums;

namespace VisaD.Application.Users.Dtos
{
	public class FeedbackDto
	{
		public string FullName { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string Description { get; set; }

		public FeedbackSubject FeedbackSubject { get; set; }
	}
}
