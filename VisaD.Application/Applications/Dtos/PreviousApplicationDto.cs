using VisaD.Data.Applications;

namespace VisaD.Application.Applications.Dtos
{
	public class PreviousApplicationDto
	{
		public int Id { get; set; }

		public bool HasPreviousApplication { get; set; }

		public string PreviousApplicationRegisterNumber { get; set; }

		public int? PreviousApplicationYear { get; set; }

		public int? PreviousApplicationLotId { get; set; }

		public int? PreviousApplicationCommitId { get; set; }

		public PreviousApplication ToModel()
		{
			var previousApplication = new PreviousApplication(this.HasPreviousApplication, this.PreviousApplicationRegisterNumber, this.PreviousApplicationYear, this.PreviousApplicationLotId, this.PreviousApplicationCommitId);
			
			return previousApplication;
		}
	}
}
