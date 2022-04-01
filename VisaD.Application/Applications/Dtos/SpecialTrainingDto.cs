using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Dtos
{
	public class SpecialTrainingDto
	{
		public int Id { get; set; }

		public string Department { get; set; }

		public NomenclatureDto<TrainingType> Type { get; set; }

		public double Duration { get; set; }
		public NomenclatureDto<DurationType> DurationType { get; set; }
	}
}
