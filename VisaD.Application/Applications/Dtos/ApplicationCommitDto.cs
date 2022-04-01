using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Dtos;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicationCommitDto : CommitDto
	{
		public PartDto<ApplicantDto> ApplicantPart { get; set; }

		public int CandidateCommitId { get; set; }
		public CandidateCommitDto CandidateCommit { get; set; }

		public PartDto<EducationDto> EducationPart { get; set; }
		public PartDto<TrainingDto> TrainingPart { get; set; }
		public PartDto<DiplomaDto> DiplomaPart { get; set; }
		public PartDto<TaxAccountDto> TaxAccountPart { get; set; }
		public PartDto<DocumentDto> DocumentPart { get; set; }
		public PartDto<RepresentativeDto> RepresentativePart { get; set; }
		public PartDto<PreviousApplicationDto> PreviousApplicationPart { get; set; }
		public PartDto<MedicalCertificateDto> MedicalCertificatePart { get; set; }

		public string RegisterNumber { get; set; }
		public int CreatorUserId { get; set; }

		public bool HasResult { get; set; }
		public ApplicationLotResultDto LotResult { get; set; }

		public string ChangeStateDescription { get; set; }

		public bool HasOtherCommits { get; set; }
	}
}
