using VisaD.Data.Common.Models;

namespace VisaD.Data.Nomenclatures
{
	public class SchoolYear : Nomenclature
	{
		public int FromYear { get; set; }
		public int ToYear { get; set; }

		public override void Update(Nomenclature newModel)
		{
			var castedModel = newModel as SchoolYear;
			this.FromYear = castedModel.FromYear;
			this.ToYear = castedModel.ToYear;

			base.Update(newModel);
		}
	}
}
