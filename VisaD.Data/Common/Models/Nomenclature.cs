using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Common.Models
{
	public abstract class Nomenclature : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? ViewOrder { get; set; }
		public bool IsActive { get; set; } = true;

		public virtual void Update(Nomenclature newModel)
		{
			this.Name = newModel.Name;
			this.ViewOrder = newModel.ViewOrder;
			this.IsActive = newModel.IsActive;
		}
	}
}
