using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Hosting.Controllers.Common
{
	public class BaseEntityController<TEntity> : ControllerBase
		where TEntity : class, IEntity
	{
		private readonly IAppDbContext context;

		public BaseEntityController(
			IAppDbContext context)
		{
			this.context = context;
		}

		[HttpGet]
		public async Task<IEnumerable<TEntity>> GetAll()
		{
			var result = await context.Set<TEntity>()
				.AsNoTracking()
				.OrderBy(e => e.Id)
				.ToListAsync();

			return result;
		}

		[HttpPost]
		public async Task<TEntity> Post([FromBody] TEntity model)
		{
			context.Entry(model).State = EntityState.Added;
			await context.SaveChangesAsync();

			return model;
		}

		[HttpPut]
		public async Task<TEntity> Put([FromBody] TEntity model)
		{
			context.Entry(model).State = EntityState.Modified;
			await context.SaveChangesAsync();

			return model;
		}

		[HttpDelete("{id:int}")]
		public async Task Delete([FromRoute] int id)
		{
			var model = this.context.Set<TEntity>()
				.Where(x => x.Id == id)
				.SingleOrDefault();

			if (model != null)
			{
				context.Entry(model).State = EntityState.Deleted;
				await context.SaveChangesAsync();
			}
		}
	}
}
