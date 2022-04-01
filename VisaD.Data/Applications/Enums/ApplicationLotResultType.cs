using System.ComponentModel;

namespace VisaD.Data.Applications.Enums
{
	public enum ApplicationLotResultType
	{
		[Description("Издадено удостоверение")]
		Certificate = 1,

		[Description("Издаден отказ")]
		Rejection = 2,

		[Description("Изпратено за вписване")]
		Actual = 3,

		[Description("Върнато за редакция")]
		Modification = 4,

		[Description("Изтрито")]
		Deleted = 5,

		[Description("Одобрено")]
		Approved = 6,

		[Description("Анулирано")]
		Annulled = 7,

		[Description("Отказано подписване")]
		RefusedSign = 8
	}
}
