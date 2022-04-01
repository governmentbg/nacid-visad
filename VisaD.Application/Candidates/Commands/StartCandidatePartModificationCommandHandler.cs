using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Candidates;
using VisaD.Data.Candidates.Register;

namespace VisaD.Application.Candidates.Commands
{
		public class StartCandidatePartModificationCommandHandler : StartPartModificationCommandHandler<CandidatePart, Candidate>
		{
			public StartCandidatePartModificationCommandHandler(IAppDbContext context)
				: base(context)
			{
			}

			protected override IQueryable<CandidatePart> LoadPart()
			{
				return context.Set<CandidatePart>()
					.Include(e => e.Entity)
						.ThenInclude(e => e.CandidatePassportDocument)
					.Include(e => e.Entity.OtherNationalities);
			}
		}
}
