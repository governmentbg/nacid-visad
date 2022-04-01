using System.ComponentModel;

namespace VisaD.Data.Applications.Enums
{
	public enum TaxType
	{
		[Description("Такса за обучение")]
		EducationTax = 1,

		[Description("Такса за подготовка")]
		TrainingTax = 2,
	}
}
