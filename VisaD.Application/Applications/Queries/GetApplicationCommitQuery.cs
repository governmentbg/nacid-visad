using FileStorageNetCore.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.AttachedFiles;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Queries
{
	public class GetApplicationCommitQuery : IRequest<ApplicationCommitDto>
	{
		public int LotId { get; set; }
		public int CommitId { get; set; }

		public class Handler : IRequestHandler<GetApplicationCommitQuery, ApplicationCommitDto>
		{
			private readonly IAppDbContext context;
			private readonly IMediator mediator;

			public Handler(IAppDbContext context, IMediator mediator)
			{
				this.context = context;
				this.mediator = mediator;
			}
			public async Task<ApplicationCommitDto> Handle(GetApplicationCommitQuery request, CancellationToken cancellationToken)
			{
				var commit = await context.Set<ApplicationCommit>()
					.AsNoTracking()
					.Where(e => e.LotId == request.LotId && e.Id == request.CommitId)
					.Select(e => new ApplicationCommitDto {
						Id = e.Id,
						RegisterNumber = e.Lot.RegisterNumber,
						CreatorUserId = e.Lot.CreatorUserId,
						LotId = e.LotId,
						State = e.State,
						ChangeStateDescription = e.ChangeStateDescription,
						HasResult = e.Lot.Result != null,
						LotResult = e.Lot.Result != null
							? new ApplicationLotResultDto {
								Id = e.Lot.Result.Id,
								Type = e.Lot.Result.Type,
								Note = e.Lot.Result.Note,
								AttachedFilePath = $"api/FilesStorage?key={e.Lot.Result.File.Key}&fileName={e.Lot.Result.File.Name}&dbId={e.Lot.Result.File.DbId}",
								CertificateNumber = e.Lot.Result.CertificateNumber,
								IsSigned = e.Lot.Result.IsSigned
							}
							: null,
						ApplicantPart = new PartDto<ApplicantDto> {
							Id = e.ApplicantPart.Id,
							Entity = new ApplicantDto {
								Institution = e.ApplicantPart.Entity.Institution != null
											? new NomenclatureDto<Institution> {
												Id = e.ApplicantPart.Entity.Institution.Id,
												Name = e.ApplicantPart.Entity.Institution.Name
											}
											: null,
								FirstName = e.ApplicantPart.Entity.FirstName,
								MiddleName = e.ApplicantPart.Entity.MiddleName,
								LastName = e.ApplicantPart.Entity.LastName,
								Position = e.ApplicantPart.Entity.Position,
								Phone = e.ApplicantPart.Entity.Phone,
								Mail = e.ApplicantPart.Entity.Mail
							},
							State = e.ApplicantPart.State
						},
						CandidateCommitId = e.CandidateCommitId,
						CandidateCommit = new CandidateCommitDto {
							Id = e.CandidateCommitId,
							LotId = e.CandidateCommit.LotId,
							State = e.CandidateCommit.State,
							CandidatePart = new PartDto<CandidateDto> {
								Id = e.CandidateCommit.CandidatePart.Id,
								Entity = new CandidateDto {
									FirstName = e.CandidateCommit.CandidatePart.Entity.FirstName,
									LastName = e.CandidateCommit.CandidatePart.Entity.LastName,
									OtherNames = e.CandidateCommit.CandidatePart.Entity.OtherNames,
									BirthDate = e.CandidateCommit.CandidatePart.Entity.BirthDate,
									BirthPlace = e.CandidateCommit.CandidatePart.Entity.BirthPlace,
									Nationality = e.CandidateCommit.CandidatePart.Entity.Nationality != null
											? new NomenclatureDto<Country> {
												Id = e.CandidateCommit.CandidatePart.Entity.Nationality.Id,
												Name = e.CandidateCommit.CandidatePart.Entity.Nationality.Name
											}
											: null,
									Country = e.CandidateCommit.CandidatePart.Entity.Country != null
											? new NomenclatureDto<Country> {
												Id = e.CandidateCommit.CandidatePart.Entity.Country.Id,
												Name = e.CandidateCommit.CandidatePart.Entity.Country.Name
											}
											: null,
									PassportNumber = e.CandidateCommit.CandidatePart.Entity.PassportNumber,
									PassportValidUntil = e.CandidateCommit.CandidatePart.Entity.PassportValidUntil,
									Phone = e.CandidateCommit.CandidatePart.Entity.Phone,
									Mail = e.CandidateCommit.CandidatePart.Entity.Mail,
									ImgFile = new AttachedFile {
										Key = e.CandidateCommit.CandidatePart.Entity.Key,
										Hash = e.CandidateCommit.CandidatePart.Entity.Hash,
										Size = e.CandidateCommit.CandidatePart.Entity.Size,
										Name = e.CandidateCommit.CandidatePart.Entity.Name,
										MimeType = e.CandidateCommit.CandidatePart.Entity.Name,
										DbId = e.CandidateCommit.CandidatePart.Entity.DbId
									},
									FirstNameCyrillic = e.CandidateCommit.CandidatePart.Entity.FirstNameCyrillic,
									LastNameCyrillic = e.CandidateCommit.CandidatePart.Entity.LastNameCyrillic,
									OtherNamesCyrillic = e.CandidateCommit.CandidatePart.Entity.OtherNamesCyrillic,
									OtherNationalities = e.CandidateCommit.CandidatePart.Entity.OtherNationalities != null
											? e.CandidateCommit.CandidatePart.Entity.OtherNationalities.Select(x => new Country { Id = x.NationalityId, Name = x.Nationality.Name }).ToList()
											: null,
									Document = new CandidatePassportDocumentDto {
										Id = e.CandidateCommit.CandidatePart.Entity.CandidatePassportDocument.Id,
										AttachedFile = new AttachedFile {
											Key = e.CandidateCommit.CandidatePart.Entity.CandidatePassportDocument.Key,
											Hash = e.CandidateCommit.CandidatePart.Entity.CandidatePassportDocument.Hash,
											Size = e.CandidateCommit.CandidatePart.Entity.CandidatePassportDocument.Size,
											Name = e.CandidateCommit.CandidatePart.Entity.CandidatePassportDocument.Name,
											MimeType = e.CandidateCommit.CandidatePart.Entity.CandidatePassportDocument.MimeType,
											DbId = e.CandidateCommit.CandidatePart.Entity.CandidatePassportDocument.DbId,
										}
									}
								},
								State = e.CandidateCommit.CandidatePart.State
							}
						},
						DiplomaPart = new PartDto<DiplomaDto> {
							Id = e.DiplomaPart.Id,
							Entity = new DiplomaDto {
								Id = e.DiplomaPart.Entity.Id,
								Description = e.DiplomaPart.Entity.Description,
								RectorDecisionDocumentFile = e.DiplomaPart.Entity.AttachedFiles.SingleOrDefault(x => x.Type == DiplomaDocumentType.RectorDecision),
								NacidRecommendation = e.DiplomaPart.Entity.AttachedFiles.SingleOrDefault(x => x.Type == DiplomaDocumentType.NacidRecommendation),
								DiplomaFiles = e.DiplomaPart.Entity.DiplomaFiles
									.Select(df => new DiplomaFileDto {
										Id = df.Id,
										DiplomaNumber = df.DiplomaNumber,
										IssuedDate = df.IssuedDate,
										Country = df.Country != null
											? new NomenclatureDto<Country> {
												Id = df.Country.Id,
												Name = df.Country.Name
											}
											: null,
										City = df.City,
										OrganizationName = df.OrganizationName,
										Type = df.DiplomaType != null
											? new DiplomaTypeNomenclatureDto {
												Id = df.DiplomaType.Id,
												Name = df.DiplomaType.Name,
												IsNacidVerificationRequired = df.DiplomaType.IsNacidVerificationRequired,
												Alias = df.DiplomaType.Alias
											}
											: null,
										DiplomaDocumentFile = df.DiplomaDocumentFiles.SingleOrDefault(x => x.Type == DiplomaDocumentType.Diploma),
										AttachedFiles = df.DiplomaDocumentFiles.Where(x => x.Type == DiplomaDocumentType.OtherDocument).ToList()
									})
							},
							State = e.DiplomaPart.State
						},
						EducationPart = new PartDto<EducationDto> {
							Id = e.EducationPart.Id,
							Entity = new EducationDto {
								Speciality = e.EducationPart.Entity.Speciality != null
											? new NomenclatureDto<Speciality> {
												Id = e.EducationPart.Entity.Speciality.Id,
												Name = e.EducationPart.Entity.Speciality.Name
											}
											: null,
								EducationalQualification = e.EducationPart.Entity.EducationalQualification != null
											? new NomenclatureDto<EducationalQualification> {
												Id = e.EducationPart.Entity.EducationalQualification.Id,
												Name = e.EducationPart.Entity.EducationalQualification.Name
											}
											: null,
								Form = e.EducationPart.Entity.Form != null
											? new NomenclatureDto<EducationFormType> {
												Id = e.EducationPart.Entity.Form.Id,
												Name = e.EducationPart.Entity.Form.Name
											}
											: null,
								SchoolYear = e.EducationPart.Entity.SchoolYear != null
											? new NomenclatureDto<SchoolYear> {
												Id = e.EducationPart.Entity.SchoolYear.Id,
												Name = e.EducationPart.Entity.SchoolYear.Name
											}
											: null,
								Duration = e.EducationPart.Entity.Duration,
								Faculty = e.EducationPart.Entity.Institution != null
											? new NomenclatureDto<Institution> {
												Id = e.EducationPart.Entity.Institution.Id,
												Name = e.EducationPart.Entity.Institution.Name
											}
											: null,
								EducationSpecialityLanguages = e.EducationPart.Entity.EducationSpecialityLanguages.Select(x => new NomenclatureDto<Language> 
								{
									Id = x.LanguageId,
									Name = x.Language.Name
								}).ToList(),
								Specialization = e.EducationPart.Entity.Specialization,
								TraineeDuration = e.EducationPart.Entity.TraineeDuration
							},
							State = e.EducationPart.State
						},
						TrainingPart = new PartDto<TrainingDto> {
							Id = e.TrainingPart.Id,
							Entity = new TrainingDto {
								LanguageDepartment = e.TrainingPart.Entity.LanguageDepartment,
								LanguageTrainingDuration = e.TrainingPart.Entity.LanguageTrainingDuration,
								TrainingLanguageDocumentFile = e.TrainingPart.Entity.TrainingLanguageDocument,
								LanguageProficiencies = e.TrainingPart.Entity.Proficiencies
									.Select(lpe => new LanguageProficiencyDto {
										Id = lpe.Id,
										Language = lpe.Language != null
										? new NomenclatureDto<Language> {
											Id = lpe.Language.Id,
											Name = lpe.Language.Name
										}
										: null,
										Reading = lpe.Reading != null
										? new NomenclatureDto<LanguageDegree> {
											Id = lpe.Reading.Id,
											Name = lpe.Reading.Name
										}
										: null,
										Writing = lpe.Writing != null
										? new NomenclatureDto<LanguageDegree> {
											Id = lpe.Writing.Id,
											Name = lpe.Writing.Name
										}
										: null,
										Speaking = lpe.Speaking != null
										? new NomenclatureDto<LanguageDegree> {
											Id = lpe.Speaking.Id,
											Name = lpe.Speaking.Name
										}
										: null
									})
							},
							State = e.TrainingPart.State
						},
						TaxAccountPart = new PartDto<TaxAccountDto> {
							Id = e.TaxAccountPart.Id,
							Entity = new TaxAccountDto {
								Taxes = e.TaxAccountPart.Entity.Taxes
									.Select(t => new TaxDto {
										Id = t.Id,
										Type = t.Type,
										Iban = t.Iban,
										AccountHolder = t.AccountHolder,
										Amount = t.Amount,
										Bank = t.Bank,
										Bic = t.Bic,
										CurrencyType = t.CurrencyType != null
											? new NomenclatureDto<CurrencyType> {
												Id = t.CurrencyType.Id,
												Name = t.CurrencyType.Name
											}
											: null,
										AdditionalInfo = t.AdditionalInfo
									})
							},
							State = e.TaxAccountPart.State
						},
						DocumentPart = new PartDto<DocumentDto> {
							Id = e.DocumentPart.Id,
							Entity = new DocumentDto {
								AreIdenticalFiles = e.DocumentPart.Entity.AreIdenticalFiles,
								Description = e.DocumentPart.Entity.Description,
								Files = e.DocumentPart.Entity.Files
									.Select(af => new ApplicationFileDto {
										Id = af.Id,
										FileDescription = af.FileDescription,
										Type = af.Type != null
											? new ApplicationFileTypeNomenclatureDto {
												Id = af.Type.Id,
												Name = af.Type.Name,
												HasDate = af.Type.HasDate,
												Description = af.Type.Description
											}
											: null,
										AttachedFile = new AttachedFile {
											Key = af.Key,
											Hash = af.Hash,
											Size = af.Size,
											Name = af.Name,
											MimeType = af.MimeType,
											DbId = af.DbId
										}
									})
							},
							State = e.DocumentPart.State
						},
						RepresentativePart = new PartDto<RepresentativeDto> {
							Id = e.RepresentativePart.Id,
							Entity = new RepresentativeDto {
								HasRepresentative = e.RepresentativePart.Entity.HasRepresentative,
								Type = e.RepresentativePart.Entity.Type.Value,
								FirstName = e.RepresentativePart.Entity.FirstName,
								LastName = e.RepresentativePart.Entity.LastName,
								IdentificationCode = e.RepresentativePart.Entity.IdentificationCode,
								Mail = e.RepresentativePart.Entity.Mail,
								Phone = e.RepresentativePart.Entity.Phone,
								Note = e.RepresentativePart.Entity.Note,
								SubmissionDate = e.RepresentativePart.Entity.SubmissionDate,
								ApplicationForCertificate = e.RepresentativePart.Entity.RepresentativeDocumentFiles.SingleOrDefault(x => x.Type == RepresentativeDocumentType.ApplicationForCertificate),
								LetterOfAttorney = e.RepresentativePart.Entity.RepresentativeDocumentFiles.SingleOrDefault(x => x.Type == RepresentativeDocumentType.LetterOfAttorney)
							},
							State = e.RepresentativePart.State
						},
						PreviousApplicationPart = new PartDto<PreviousApplicationDto> {
							Id = e.PreviousApplicationPart.Id,
							Entity = new PreviousApplicationDto {
								HasPreviousApplication = e.PreviousApplicationPart.Entity.HasPreviousApplication,
								PreviousApplicationRegisterNumber = e.PreviousApplicationPart.Entity.PreviousApplicationRegisterNumber,
								PreviousApplicationYear = e.PreviousApplicationPart.Entity.PreviousApplicationYear,
								PreviousApplicationLotId = e.PreviousApplicationPart.Entity.PreviousApplicationLotId,
								PreviousApplicationCommitId = e.PreviousApplicationPart.Entity.PreviousApplicationCommitId
							},
							State = e.PreviousApplicationPart.State
						},
						MedicalCertificatePart = new PartDto<MedicalCertificateDto> {
							Id = e.MedicalCertificatePart.Id,
							State = e.MedicalCertificatePart.State,
							Entity = new MedicalCertificateDto {
								Id = e.MedicalCertificatePart.Entity.Id,
								File = new AttachedFile {
									Key = e.MedicalCertificatePart.Entity.Key,
									Hash = e.MedicalCertificatePart.Entity.Hash,
									Size = e.MedicalCertificatePart.Entity.Size,
									Name = e.MedicalCertificatePart.Entity.Name,
									MimeType = e.MedicalCertificatePart.Entity.MimeType,
									DbId = e.MedicalCertificatePart.Entity.DbId
								},
								IssuedDate = e.MedicalCertificatePart.Entity.IssuedDate
							}
						}
					})
					.SingleOrDefaultAsync(cancellationToken);

				commit.HasOtherCommits = await mediator.Send(new GetApplicationCommitCountQuery { LotId = request.LotId }, cancellationToken);

				return commit;
			}
		}
	}
}