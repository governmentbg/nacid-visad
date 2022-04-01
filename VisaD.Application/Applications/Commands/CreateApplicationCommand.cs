using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Configurations;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Emails;
using VisaD.Application.Ems.Converters;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Nomenclatures.Constants;
using VisaD.Infrastructure.Ems;
using VisaD.Infrastructure.Ems.Enums;
using VisaD.Infrastructure.Ems.Models;

namespace VisaD.Application.Applications.Commands
{
	public class CreateApplicationCommand : IRequest<CommitInfoDto>
	{
		public ApplicantDto Applicant { get; set; }

		public int CandidateCommitId { get; set; }

		public EducationDto Education { get; set; }
		public TrainingDto Training { get; set; }
		public TaxAccountDto TaxAccount { get; set; }
		public DocumentDto Document { get; set; }
		public DiplomaDto Diploma { get; set; }
		public RepresentativeDto Representative { get; set; }
		public PreviousApplicationDto PreviousApplication { get; set; }
		public MedicalCertificateDto MedicalCertificate { get; set; }

		public CommitState State { get; set; }
		public int? DraftId { get; set; }

		public class Handler : IRequestHandler<CreateApplicationCommand, CommitInfoDto>
		{
			const string RegisterIndexAlias = "ApplicationRegisterIndex";
			const string ElectornicServiceUri = "VisaApplication";

			private readonly IAppDbContext context;
			private readonly IMediator mediator;
			private readonly AuthConfiguration authConfiguration;
			private readonly EmsService emsService;
			private readonly IEmsApplicationConverter converter;

			public Handler(
				IAppDbContext context,
				IMediator mediator,
				IOptions<AuthConfiguration> authOptions,
				IEmsApplicationConverter converter,
				EmsService emsService
				)
			{
				this.context = context;
				this.mediator = mediator;
				this.authConfiguration = authOptions.Value;
				this.converter = converter;
				this.emsService = emsService;
			}

			public async Task<CommitInfoDto> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
			{
				if (request.State == CommitState.InitialDraft && request.DraftId != null)
				{
					await this.mediator.Send(new DeleteApplicationDraftCommand { DraftId = request.DraftId.Value });
				}

				int? lastLotNumber = await this.context.Set<ApplicationLot>()
					.MaxAsync(e => (int?)e.LotNumber, cancellationToken);
				var lot = new ApplicationLot {
					LotNumber = (lastLotNumber ?? 0) + 1
				};

				if (string.IsNullOrWhiteSpace(lot.RegisterNumber))
				{
					var registerIndexCounter = await this.context.Set<RegisterIndexCounter>()
						.AsNoTracking()
						.Include(e => e.RegisterIndex)
						.SingleAsync(e => e.RegisterIndex.Alias == RegisterIndexAlias && e.Year == DateTime.Now.Year, cancellationToken);

					string query = $"update {nameof(RegisterIndexCounter).ToLower()} set {nameof(RegisterIndexCounter.Counter).ToLower()} = {nameof(RegisterIndexCounter.Counter).ToLower()} + 1 where id = @id returning {nameof(RegisterIndexCounter.Counter).ToLower()}";
					var queryParams = new Dictionary<string, object>() {
					{"id", registerIndexCounter.Id }
				};
					int registerIndexCount = await context.ExecuteRawSqlScalarAsync<int>(query, queryParams);
					lot.RegisterNumber = string.Format(registerIndexCounter.RegisterIndex.Format, registerIndexCount, DateTime.Now.Date);
				}

				this.context.Set<ApplicationLot>().Add(lot);
				await this.context.SaveChangesAsync(cancellationToken);

				lot.AddResult(ApplicationLotResultType.Actual, null, lot.RegisterNumber, null, null);

				var commit = new ApplicationCommit {
					LotId = lot.Id,
					State = CommitState.Actual,
					Number = 1,
					ApplicantPart = new ApplicantPart {
						Entity = request.Applicant?.ToModel()
					},
					CandidateCommitId = request.CandidateCommitId,
					EducationPart = new EducationPart {
						Entity = request.Education?.ToModel()
					},
					TrainingPart = new TrainingPart {
						Entity = request.Training?.ToModel()
					},
					TaxAccountPart = new TaxAccountPart {
						Entity = request.TaxAccount?.ToModel()
					},
					DocumentPart = new DocumentPart {
						Entity = request.Document?.ToModel()
					},
					DiplomaPart = new DiplomaPart {
						Entity = request.Diploma?.ToModel()
					},
					RepresentativePart = new RepresentativePart {
						Entity = request.Representative?.ToModel()
					},
					PreviousApplicationPart = new PreviousApplicationPart {
						Entity = request.PreviousApplication?.ToModel()
					},
					MedicalCertificatePart = new MedicalCertificatePart {
						Entity = request.MedicalCertificate?.ToModel()
					}
				};

				this.context.Set<ApplicationCommit>().Add(commit);

				EmsApplication emsApplication = this.converter.ToEmsApplication(ElectornicServiceUri, commit.ApplicantPart, lot.RegisterNumber, null, false);
				var pendingEmsApplication = new {
					Application = JsonConvert.SerializeObject(emsApplication),
					//Signature = application.Signature,
					Status = EmsIncomingDocStatus.Pending
				};

				var docGuid = await emsService.SubmitApplicationAsync(JsonConvert.SerializeObject(pendingEmsApplication));
				var response = await Policy
					.HandleResult<EmsDocStatusResponse>(e => e.Status == EmsIncomingDocStatus.Pending)
					.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
					.ExecuteAsync(async () => await emsService.GetEmsApplicationStatus(docGuid))
				;

				var candidate = await this.context.Set<CandidateCommit>()
					.Where(x => x.Id == request.CandidateCommitId)
					.Include(x => x.CandidatePart.Entity)
						.ThenInclude(x => x.Country)
					.SingleAsync(cancellationToken);

				var templateData = new {
					RegisterNumber = commit.Lot.RegisterNumber,
					FullName = commit.ApplicantPart.Entity.FullName,
					CandidateFullname = candidate.CandidatePart.Entity.Fullname
				};

				await mediator.Send(new SendApplicationEmailCommand {
					Alias = EmailTypeAlias.NEW_APPLICATION,
					CreatorUserId = lot.CreatorUserId,
					TemplateData = templateData
				}, cancellationToken);

				await this.context.SaveChangesAsync(cancellationToken);

				await mediator.Send(new AddApplicationStatusHistoryCommand {
					LotId = commit.LotId,
					CommitId = commit.Id,
					State = commit.State,
					ChangeStateDescription = commit.ChangeStateDescription,
					CandidateName = candidate.CandidatePart.Entity.Fullname,
					CandidateBirthDate = candidate.CandidatePart.Entity.BirthDate,
					CandidateCountry = candidate.CandidatePart.Entity.Country.Name,
					RegisterNumber = commit.Lot.RegisterNumber
				}, cancellationToken);

				return new CommitInfoDto {
					LotId = lot.Id,
					CommitId = commit.Id
				};
			}
		}
	}
}
