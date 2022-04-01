using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications.Register;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;

namespace VisaD.Application.Candidates.Commands
{
	public class CancelCandidatePartModificationCommandHandler : CancelPartModificationCommandHandler<CandidateCommit, CandidatePart, Candidate>
	{
		public CancelCandidatePartModificationCommandHandler(IAppDbContext context)
			: base(context)
		{
		}

		protected override IQueryable<CandidatePart> LoadPart()
		{
			return context.Set<CandidatePart>()
				.Include(e => e.Entity)
					.ThenInclude(e => e.OtherNationalities)
				.Include(e => e.Entity)
					.ThenInclude(x => x.CandidatePassportDocument);
		}
	}
}
