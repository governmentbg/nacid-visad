using VisaD.Data.Common.Models;

namespace VisaD.Data.Nomenclatures
{
	public class ApplicationFileType : Nomenclature
	{
		public bool HasDate { get; set; }

		public bool IsForBachelor { get; set; }

		public bool IsForDoctor { get; set; }

		public bool IsForMaster { get; set; }

		public bool IsForMasterWithSecondary { get; set; }

		public string Description { get; set; }

		public string Alias { get; set; }

		public override void Update(Nomenclature newModel)
		{
			var castedModel = newModel as ApplicationFileType;

			this.HasDate = castedModel.HasDate;
			this.IsForBachelor = castedModel.IsForBachelor;
			this.IsForDoctor = castedModel.IsForDoctor;
			this.IsForMaster = castedModel.IsForMaster;
			this.Description = castedModel.Description;
			this.IsForMasterWithSecondary = castedModel.IsForMasterWithSecondary;

			base.Update(newModel);
		}
	}
}
