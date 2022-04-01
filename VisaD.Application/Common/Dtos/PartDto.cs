using VisaD.Data.Common.Enums;

namespace VisaD.Application.Common.Dtos
{
	public class PartDto<TDto>
		where TDto : class
	{
		public int Id { get; set; }

		public TDto Entity { get; set; }

		public PartState State { get; set; }
	}
}
