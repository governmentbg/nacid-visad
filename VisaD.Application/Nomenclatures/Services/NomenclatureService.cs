using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Extensions;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Application.Nomenclatures.Extensions;
using VisaD.Data.Common.Models;

namespace VisaD.Application.Nomenclatures.Services
{
    public class NomenclatureService<TNomenclature> : INomenclatureService<TNomenclature>
        where TNomenclature : Nomenclature
    {
        private readonly IAppDbContext context;
        private readonly DomainValidationService validation;

        public NomenclatureService(IAppDbContext context, DomainValidationService validation)
        {
            this.context = context;
            this.validation = validation;
        }

        public async Task<IEnumerable<TNomenclature>> GetNomenclaturesAsync<TFilter>(TFilter filter)
            where TFilter : BaseNomenclatureFilterDto<TNomenclature>
        {
            IQueryable<TNomenclature> query = this.context.Set<TNomenclature>()
                .AsNoTracking()
                .GetFiltered(filter)
                .ApplyOrder(filter.Orders)
            ;

            //if (filter.Limit.HasValue && filter.Offset.HasValue)
            //{
            //    query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
            //}

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TDto>> SelectNomenclaturesAsync<TFilter, TDto>(TFilter filter)
            where TFilter : BaseNomenclatureFilterDto<TNomenclature>
            where TDto : IMapping<TNomenclature, TDto>, new()
        {
            IQueryable<TNomenclature> query = this.context.Set<TNomenclature>()
                .AsNoTracking()
                .GetFiltered(filter)
                .ApplyOrder(filter.Orders)
            ;

            if (filter.Limit.HasValue && filter.Offset.HasValue)
            {
                query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
            }

            var result = await query.Select(new TDto().Map())
                .ToListAsync();
            return result;
        }

        public async Task<TNomenclature> GetSingleOrDefaultNomenclatureAsync(Expression<Func<TNomenclature, bool>> predicate)
        {
            var nomenclature = await context.Set<TNomenclature>()
                .SingleOrDefaultAsync(predicate);

            return nomenclature;
        }

        public async Task<TNomenclature> InsertNomenclatureAsync(TNomenclature model)
        {
            var maxViewOrder = await context.Set<TNomenclature>()
                    .MaxAsync(e => e.ViewOrder);
            model.ViewOrder = maxViewOrder.HasValue ? maxViewOrder.Value + 1 : 1;
            context.Set<TNomenclature>().Add(model);
            await context.SaveChangesAsync();

            return model;
        }

        public async Task<TNomenclature> UpdateNomenclatureAsync(int id, TNomenclature model)
        {
            var original = await context.Set<TNomenclature>()
                .SingleOrDefaultAsync(e => e.Id == id);
            if (original == null)
            {
                return null;
            }

            original.Update(model);
            await context.SaveChangesAsync();

            return model;
        }

        public async Task DeleteNomenclatureAsync(int id)
        {
            var forDelete = await context.Set<TNomenclature>()
                .SingleOrDefaultAsync(e => e.Id == id);
            if (forDelete != null)
            {
                try
                {
                    context.Set<TNomenclature>().Remove(forDelete);
                    await context.SaveChangesAsync();
                }
                catch(DbUpdateException ex)
                {
                    this.validation.ThrowErrorMessage(NomenclatureErrorCode.Nomenclature_CannotDelete);
                }
            }
        }
    }
}
