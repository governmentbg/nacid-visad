using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Commands
{
    public class CancelModificationCommand : IRequest<CommitInfoDto>
    {
        public int LotId { get; set; }

        public class Handler : IRequestHandler<CancelModificationCommand, CommitInfoDto>
        {
            private readonly IAppDbContext context;
            private readonly IMediator mediator;

            public Handler(IAppDbContext context, IMediator mediator)
            {
                this.context = context;
                this.mediator = mediator;
            }

            public async Task<CommitInfoDto> Handle(CancelModificationCommand request, CancellationToken cancellationToken)
            {
                var modificationCommit = await context.Set<ApplicationCommit>()
                    .Include(e => e.ApplicantPart)
                            .ThenInclude(a => a.Entity)
                    .Include(e => e.EducationPart)
                            .ThenInclude(ed => ed.Entity)
                                .ThenInclude(el => el.EducationSpecialityLanguages)
                    .Include(e => e.TrainingPart)
                            .ThenInclude(t => t.Entity)
                                .ThenInclude(te => te.Proficiencies)
                    .Include(e => e.TaxAccountPart)
                            .ThenInclude(t => t.Entity)
                                .ThenInclude(te => te.Taxes)
                    .Include(e => e.DiplomaPart)
                            .ThenInclude(t => t.Entity)
                                .ThenInclude(te => te.DiplomaFiles)
                    .Include(e => e.MedicalCertificatePart)
                            .ThenInclude(t => t.Entity)
                    .SingleAsync(e => e.LotId == request.LotId && e.State == CommitState.Modification, cancellationToken);

                context.Set<ApplicationCommit>().Remove(modificationCommit);

                if (modificationCommit.ApplicantPart.State == PartState.Modified)
                {
                    context.Set<Applicant>().Remove(modificationCommit.ApplicantPart.Entity);
                }
                context.Set<ApplicantPart>().Remove(modificationCommit.ApplicantPart);

                if (modificationCommit.EducationPart.State == PartState.Modified)
                {
                    context.Set<Education>().Remove(modificationCommit.EducationPart.Entity);
                }
                context.Set<EducationPart>().Remove(modificationCommit.EducationPart);

                if (modificationCommit.TrainingPart.State == PartState.Modified)
                {
                    context.Set<Training>().Remove(modificationCommit.TrainingPart.Entity);
                }
                context.Set<TrainingPart>().Remove(modificationCommit.TrainingPart);

                if (modificationCommit.TaxAccountPart.State == PartState.Modified)
                {
                    context.Set<TaxAccount>().Remove(modificationCommit.TaxAccountPart.Entity);
                }
                context.Set<TaxAccountPart>().Remove(modificationCommit.TaxAccountPart);

                if (modificationCommit.DiplomaPart.State == PartState.Modified)
                {
                    context.Set<Diploma>().Remove(modificationCommit.DiplomaPart.Entity);
                }
                context.Set<DiplomaPart>().Remove(modificationCommit.DiplomaPart);

                if (modificationCommit.MedicalCertificatePart.State == PartState.Modified)
                {
                    context.Set<MedicalCertificate>().Remove(modificationCommit.MedicalCertificatePart.Entity);
                }
                context.Set<MedicalCertificatePart>().Remove(modificationCommit.MedicalCertificatePart);

                var actualWithModificationCommit = await context.Set<ApplicationCommit>()
                    .SingleAsync(e => e.LotId == request.LotId && e.State == CommitState.ActualWithModification);

                actualWithModificationCommit.State = CommitState.Actual;
                await this.mediator.Send(new ChangeLotResultTypeCommand { LotId = actualWithModificationCommit.LotId, Type = ApplicationLotResultType.Actual });

                await context.SaveChangesAsync(cancellationToken);

                return new CommitInfoDto {
                    LotId = actualWithModificationCommit.LotId,
                    CommitId = actualWithModificationCommit.Id
                };
            }
        }
    }
}
