using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Candidates.Commands
{
	public class FinishApplicationCandidateCommitModificationCommand : IRequest<CandidateCommitDto>
	{
		public int CommitId { get; set; }

		public CandidateDto CandidateDto { get; set; }

		public int ApplicationCommitId { get; set; }

		public class Handler : IRequestHandler<FinishApplicationCandidateCommitModificationCommand, CandidateCommitDto>
		{
			protected readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<CandidateCommitDto> Handle(FinishApplicationCandidateCommitModificationCommand request, CancellationToken cancellationToken)
			{
				var actualCommit = await context.Set<CandidateCommit>()
					.Include(e => e.CandidatePart)
						.ThenInclude(p => p.Entity)
					.Include(e => e.CandidatePart)
						.ThenInclude(p => p.Entity)
							.ThenInclude(e => e.Country)
					.SingleAsync(e => e.Id == request.CommitId, cancellationToken);
				actualCommit.State = CommitState.History;

				var modificationCommit = new CandidateCommit(actualCommit);
				modificationCommit.State = CommitState.Actual;
				modificationCommit.Number = actualCommit.Number + 1;
				context.Set<CandidateCommit>().Add(modificationCommit);

				var entity = new Candidate(
					request.CandidateDto.FirstName,
					request.CandidateDto.LastName,
					request.CandidateDto.BirthDate,
					request.CandidateDto.BirthPlace,
					request.CandidateDto.Nationality.Id,
					request.CandidateDto.PassportNumber,
					request.CandidateDto.PassportValidUntil,
					request.CandidateDto.Country.Id,
					request.CandidateDto.Phone,
					request.CandidateDto.Mail,
					request.CandidateDto.ImgFile.Key,
					request.CandidateDto.ImgFile.Hash,
					request.CandidateDto.ImgFile.Size,
					request.CandidateDto.ImgFile.Name,
					request.CandidateDto.ImgFile.MimeType,
					request.CandidateDto.ImgFile.DbId,
					request.CandidateDto.OtherNames,
					request.CandidateDto.FirstNameCyrillic,
					request.CandidateDto.LastNameCyrillic,
					request.CandidateDto.OtherNamesCyrillic
				);

				foreach (var item in request.CandidateDto.OtherNationalities)
				{
					entity.AddNationality(item.Id);
				}

				entity.AddFile(
					request.CandidateDto.Document.AttachedFile.Key,
					request.CandidateDto.Document.AttachedFile.Hash,
					request.CandidateDto.Document.AttachedFile.Size,
					request.CandidateDto.Document.AttachedFile.Name,
					request.CandidateDto.Document.AttachedFile.MimeType,
					request.CandidateDto.Document.AttachedFile.DbId
					);

				context.Set<Candidate>().Add(entity);
				await context.SaveChangesAsync(cancellationToken);

				modificationCommit.CandidatePart.Entity = entity;
				modificationCommit.CandidatePart.EntityId = entity.Id;
				modificationCommit.CandidatePart.State = PartState.Modified;

				var applicationCommit = await this.context.Set<ApplicationCommit>()
					.SingleOrDefaultAsync(a => a.Id == request.ApplicationCommitId);

				if (applicationCommit != null)
				{
					applicationCommit.CandidateCommitId = modificationCommit.Id;
				}

				await context.SaveChangesAsync(cancellationToken);

				return await context.Set<CandidateCommit>()
					.Select(CandidateCommitDto.SelectExpression)
					.SingleAsync(e => e.Id == modificationCommit.Id, cancellationToken);
			}
		}
	}
}
