using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateTaxAccountCommand : IRequest<Unit>
	{
		public TaxAccountDto Model { get; set; }
		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdateTaxAccountCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateTaxAccountCommand request, CancellationToken cancellationToken)
			{
				var part = await context.Set<TaxAccountPart>()
					.Include(e => e.Entity)
						.ThenInclude(e => e.Taxes)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				var taxesForAdd = request.Model.Taxes.Where(e => !part.Entity.Taxes.Select(t => t.Id).Contains(e.Id));
				foreach(var taxForAdd in taxesForAdd)
				{
					part.Entity.AddTax(taxForAdd.Iban, taxForAdd.AccountHolder, taxForAdd.Amount, taxForAdd.CurrencyType?.Id, taxForAdd.AdditionalInfo, taxForAdd.Type, taxForAdd.Bank, taxForAdd.Bic);
				}

				var taxesForUpdate = request.Model.Taxes.Where(e => part.Entity.Taxes.Select(t => t.Id).Contains(e.Id));
				foreach(var taxForUpdate in taxesForUpdate)
				{
					part.Entity.UpdateTax(taxForUpdate.Iban, taxForUpdate.AccountHolder, taxForUpdate.Amount, taxForUpdate.CurrencyType?.Id, taxForUpdate.AdditionalInfo,
						taxForUpdate.Type, taxForUpdate.Id, taxForUpdate.Bank, taxForUpdate.Bic);
				}

				var taxesForRemove = part.Entity.Taxes.Where(e => !request.Model.Taxes.Select(t => t.Id).Contains(e.Id));
				foreach(var taxForRemove in taxesForRemove)
				{
					part.Entity.RemoveTax(taxForRemove.Id);
				}

				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
