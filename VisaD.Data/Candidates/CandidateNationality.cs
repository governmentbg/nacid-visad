using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Candidates
{
	public class CandidateNationality
	{
		public CandidateNationality(int nationalityId)
		{
			this.NationalityId = nationalityId;
		}

		public int Id { get; set; }

		public int CandidateId { get; set; }

		public Candidate Candidate { get; set; }

		public int NationalityId { get; set; }

		public Country Nationality { get; set; }
	}
}
