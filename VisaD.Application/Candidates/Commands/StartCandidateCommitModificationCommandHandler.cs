using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Candidates.Register;

namespace VisaD.Application.Candidates.Commands
{
	public class StartCandidateCommitModificationCommandHandler : StartCommitModificationCommandHandler<CandidateCommit>
	{
		public StartCandidateCommitModificationCommandHandler(IAppDbContext context)
			:base(context)
		{

		}

		protected override IQueryable<CandidateCommit> LoadRelatedData(IQueryable<CandidateCommit> query)
		{
			return query
				.Include(e => e.CandidatePart)
			;
		}
	}
}
