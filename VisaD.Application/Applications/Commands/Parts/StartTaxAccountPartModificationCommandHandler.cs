using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
	public class StartTaxAccountPartModificationCommandHandler : StartPartModificationCommandHandler<TaxAccountPart, TaxAccount>
	{
		public StartTaxAccountPartModificationCommandHandler(IAppDbContext context)
			:base(context)
		{
		}

		protected override IQueryable<TaxAccountPart> LoadPart()
		{
			return context.Set<TaxAccountPart>()
				.Include(e => e.Entity)
					.ThenInclude(e => e.Taxes);
		}
	}
}
