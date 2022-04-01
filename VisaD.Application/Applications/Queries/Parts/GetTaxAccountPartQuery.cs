using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Queries.Parts
{
	public class GetTaxAccountPartQuery : IRequest<PartDto<TaxAccountDto>>
	{
		public int PartId { get; set; }

		public class Handler : IRequestHandler<GetTaxAccountPartQuery, PartDto<TaxAccountDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<PartDto<TaxAccountDto>> Handle(GetTaxAccountPartQuery request, CancellationToken cancellationToken)
			{
				var result = await context.Set<TaxAccountPart>()
					.AsNoTracking()
					.Select(e => new PartDto<TaxAccountDto> {
						Id = e.Id,
						Entity = new TaxAccountDto {
							Taxes = e.Entity.Taxes
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
						State = e.State
					})
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				return result;
			}
		}
	}
}
