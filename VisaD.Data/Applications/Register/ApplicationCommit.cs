using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Models;

namespace VisaD.Data.Applications.Register
{
	public class ApplicationCommit : Commit
	{
		public ApplicationLot Lot { get; set; }

		public ApplicantPart ApplicantPart { get; set; }

		public int CandidateCommitId { get; set; }
		public CandidateCommit CandidateCommit { get; set; }
		
		public EducationPart EducationPart { get; set; }
		public TrainingPart TrainingPart { get; set; }
		public TaxAccountPart TaxAccountPart { get; set; }
		public DocumentPart DocumentPart { get; set; }
		public DiplomaPart DiplomaPart { get; set; }
		public RepresentativePart RepresentativePart { get; set; }
		public PreviousApplicationPart PreviousApplicationPart { get; set; }
		public MedicalCertificatePart MedicalCertificatePart { get; set; }

		public ApplicationCommit()
			:base()
		{

		}

		public ApplicationCommit(ApplicationCommit commit)
			:base(commit)
		{
			this.ApplicantPart = new ApplicantPart(commit.ApplicantPart);

			this.CandidateCommitId = commit.CandidateCommitId;
			this.CandidateCommit = commit.CandidateCommit;

			this.EducationPart = new EducationPart(commit.EducationPart);
			this.TrainingPart = new TrainingPart(commit.TrainingPart);
			this.TaxAccountPart = new TaxAccountPart(commit.TaxAccountPart);
			this.DocumentPart = new DocumentPart(commit.DocumentPart);
			this.DiplomaPart = new DiplomaPart(commit.DiplomaPart);
			this.RepresentativePart = new RepresentativePart(commit.RepresentativePart);
			this.PreviousApplicationPart = new PreviousApplicationPart(commit.PreviousApplicationPart);
			this.MedicalCertificatePart = new MedicalCertificatePart(commit.MedicalCertificatePart);
		}
	}
}
