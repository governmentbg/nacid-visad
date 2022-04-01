using System;

namespace VisaD.Application.Applications.Dtos
{
	public class ReportItemDto
	{
		public int? ModificationCommitsCount { get; set; }
		public int? UnsignedCommitsCount { get; set; }
		public int? PendingCommitsCount { get; set; }
		public int? CertificateCommitsCount { get; set; }
		public int? RejectedCommitsCount { get; set; }
		public int? AnnulledCommitsCount { get; set; }

		public string Nationality { get; set; }
		public string Country { get; set; }
		public string EducationalQualification { get; set; }
		public string Institution { get; set; }
		public string SchoolYear { get; set; }

		public string CandidateLatinName { get; set; }
		public string CandidateCiryllicName { get; set; }
		public string CandidateNationality { get; set; }
		public string CandidateCountry { get; set; }
		public string CandidateBirthPlace { get; set; }
		public DateTime? CandidateBirthDate { get; set; }
		public int? CandidateCertficatesCount { get; set; }

		//ForExports
		public string FormatedCandidateBirthDate { get; set; }
		public string ConcatenatedCandidateNames { get; set; }
		public string ConcatenatedCandidateBirthInfo { get; set; }
		public string HideHtmlElement { get; set; }
	}
}
