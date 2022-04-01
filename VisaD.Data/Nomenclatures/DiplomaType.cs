using VisaD.Data.Common.Models;

namespace VisaD.Data.Nomenclatures
{
	public class DiplomaType : Nomenclature
	{
		public bool IsNacidVerificationRequired { get; set; }

		public string Alias { get; set; }

		public override void Update(Nomenclature newModel)
		{
			var castedModel = newModel as DiplomaType;
			this.IsNacidVerificationRequired = castedModel.IsNacidVerificationRequired;

			base.Update(newModel);
		}
	}
}
