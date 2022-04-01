using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VisaD.Application.Applications.Commands.Parts;
using VisaD.Application.Candidates.Commands;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;

namespace VisaD.Application.Applications.Commands.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationPartCommands(this IServiceCollection services)
		{
			services
				// Application commits
				.AddTransient(typeof(IRequestHandler<StartCommitModificationCommand<ApplicationCommit>, CommitInfoDto>), typeof(StartApplicationCommitModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<FinishCommitModificationCommand<ApplicationLot, ApplicationCommit>, CommitInfoDto>), typeof(FinishCommitModificationCommandHandler<ApplicationLot, ApplicationCommit>))
				.AddTransient(typeof(IRequestHandler<CommitReadyModificationCommand<ApplicationCommit>, CommitInfoDto>), typeof(CommitReadyModificationCommandHandler<ApplicationCommit>))
				.AddTransient(typeof(IRequestHandler<RevertErasedCommitCommand<ApplicationCommit>, CommitInfoDto>), typeof(RevertErasedCommitCommandHandler<ApplicationCommit>))
				.AddTransient(typeof(IRequestHandler<EraseCommitCommand<ApplicationCommit>, CommitInfoDto>), typeof(EraseCommitCommandHandler<ApplicationCommit>))
				.AddTransient(typeof(IRequestHandler<AnnulCommitCommand<ApplicationCommit>, CommitInfoDto>), typeof(AnnulCommitCommandHandler<ApplicationCommit>))
				.AddTransient(typeof(IRequestHandler<ApproveCommitCommand<ApplicationCommit>, CommitInfoDto>), typeof(ApproveCommitCommandHandler<ApplicationCommit>))
				.AddTransient(typeof(IRequestHandler<RefuseSignCommand<ApplicationCommit>, CommitInfoDto>), typeof(RefuseSignCommandHandler<ApplicationCommit>))

				// Candidate commits
				.AddTransient(typeof(IRequestHandler<StartCommitModificationCommand<CandidateCommit>, CommitInfoDto>), typeof(StartCandidateCommitModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<FinishCommitModificationCommand<CandidateLot, CandidateCommit>, CommitInfoDto>), typeof(FinishCommitModificationCommandHandler<CandidateLot, CandidateCommit>))
				.AddTransient(typeof(IRequestHandler<CommitReadyModificationCommand<CandidateCommit>, CommitInfoDto>), typeof(CommitReadyModificationCommandHandler<CandidateCommit>))
				.AddTransient(typeof(IRequestHandler<RevertErasedCommitCommand<CandidateCommit>, CommitInfoDto>), typeof(RevertErasedCommitCommandHandler<CandidateCommit>))
				.AddTransient(typeof(IRequestHandler<EraseCommitCommand<CandidateCommit>, CommitInfoDto>), typeof(EraseCommitCommandHandler<CandidateCommit>))

				// Application parts
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<ApplicantPart, Applicant>, int>), typeof(StartPartModificationCommandHandler<ApplicantPart, Applicant>))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<EducationPart, Education>, int>), typeof(StartEducationPartModificationCommand))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<TrainingPart, Training>, int>), typeof(StartTrainingPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<TaxAccountPart, TaxAccount>, int>), typeof(StartTaxAccountPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<DocumentPart, Document>, int>), typeof(StartDocumentPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<DiplomaPart, Diploma>, int>), typeof(StartDiplomaPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<RepresentativePart, Representative>, int>), typeof(StartRepresentativePartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<PreviousApplicationPart, PreviousApplication>, int>), typeof(StartPartModificationCommandHandler<PreviousApplicationPart, PreviousApplication>))
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<MedicalCertificatePart, MedicalCertificate>, int>), typeof(StartMedicalCertificatePartModificationCommandHandler))

				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, ApplicantPart, Applicant>, int>), typeof(CancelPartModificationCommandHandler<ApplicationCommit, ApplicantPart, Applicant>))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, EducationPart, Education>, int>), typeof(CancelEducationPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, TrainingPart, Training>, int>), typeof(CancelTrainingPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, TaxAccountPart, TaxAccount>, int>), typeof(CancelTaxAccountPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, DocumentPart, Document>, int>), typeof(CancelDocumentPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, DiplomaPart, Diploma>, int>), typeof(CancelDiplomaPartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, RepresentativePart, Representative>, int>), typeof(CancelRepresentativePartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, PreviousApplicationPart, PreviousApplication>, int>), typeof(CancelPartModificationCommandHandler<ApplicationCommit, PreviousApplicationPart, PreviousApplication>))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<ApplicationCommit, MedicalCertificatePart, MedicalCertificate>, int>), typeof(CancelMedicalCertificatePartModificationCommandHandler))

				// Candidate parts
				.AddTransient(typeof(IRequestHandler<StartPartModificationCommand<CandidatePart, Candidate>, int>), typeof(StartCandidatePartModificationCommandHandler))
				.AddTransient(typeof(IRequestHandler<CancelPartModificationCommand<CandidateCommit, CandidatePart, Candidate>, int>), typeof(CancelCandidatePartModificationCommandHandler))
			;

			return services;
		}
	}
}
