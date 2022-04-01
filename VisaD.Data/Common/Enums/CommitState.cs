using System.ComponentModel;

namespace VisaD.Data.Common.Enums
{
	public enum CommitState
	{
		[Description("първоначална чернова")]
		InitialDraft = 1,

		[Description("върнато за редакция")]
		Modification = 2,

		[Description("изпратено за вписване")]
		Actual = 3,

		[Description("актуално с инициирана промяна")]
		ActualWithModification = 4,

		[Description("предишно състояние")]
		History = 5,

		[Description("Изтрито")]
		Deleted = 6,

		[Description("готов за вписване")]
		CommitReady = 7,

		[Description("заявено за заличаване")]
		ModificationErase = 8,

		[Description("Одобрено")]
		Approved = 9,

		[Description("Анулирано")]
		Annulled = 10,

		[Description("Отказано подписване")]
		RefusedSign = 11
	}
}
