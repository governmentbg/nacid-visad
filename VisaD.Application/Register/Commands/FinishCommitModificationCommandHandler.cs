using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Models;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Register.Commands
{
	public class FinishCommitModificationCommandHandler<TLot, TCommit> : IRequestHandler<FinishCommitModificationCommand<TLot, TCommit>, CommitInfoDto>
		where TLot: Lot<TCommit>
		where TCommit : Commit
	{
		protected readonly IAppDbContext context;

		public FinishCommitModificationCommandHandler(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<CommitInfoDto> Handle(FinishCommitModificationCommand<TLot, TCommit> request, CancellationToken cancellationToken)
		{
			var lot = await context.Set<TLot>()
				.Include(e => e.Commits)
				.SingleAsync(e => e.Id == request.LotId, cancellationToken);

			var currentCommit = lot.Commits.Single(e => e.State == CommitState.Modification || e.State == CommitState.InitialDraft || e.State == CommitState.CommitReady);

			if ((currentCommit.State == CommitState.InitialDraft || currentCommit.State == CommitState.CommitReady) && request.ShouldRegisterLot && string.IsNullOrWhiteSpace(lot.RegisterNumber))
			{
				var registerIndexCounter = await context.Set<RegisterIndexCounter>()
					.AsNoTracking()
					.Include(e => e.RegisterIndex)
					.SingleAsync(e => e.RegisterIndex.Alias == request.RegisterIndexAlias && e.Year == DateTime.Now.Year, cancellationToken);

				string query = $"update {nameof(RegisterIndexCounter).ToLower()} set {nameof(RegisterIndexCounter.Counter).ToLower()} = {nameof(RegisterIndexCounter.Counter).ToLower()} + 1 where id = @id returning {nameof(RegisterIndexCounter.Counter).ToLower()}";
				var queryParams = new Dictionary<string, object>() {
					{"id", registerIndexCounter.Id }
				};
				int registerIndexCount = await context.ExecuteRawSqlScalarAsync<int>(query, queryParams);
				lot.RegisterNumber = string.Format(registerIndexCounter.RegisterIndex.Format, registerIndexCount, DateTime.Now.Date);
			}

			currentCommit.State = CommitState.Actual;

			var previousActualCommit = lot.Commits.SingleOrDefault(e => e.State == CommitState.ActualWithModification);
			if (previousActualCommit != null)
			{
				previousActualCommit.State = CommitState.History;
			}

			currentCommit.Number = previousActualCommit?.Number + 1 ?? 1;

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = currentCommit.LotId,
				CommitId = currentCommit.Id
			};
		}
	}
}
