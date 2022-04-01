using System;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications
{
	public class SpecialTraining : IEntity, IAuditable
	{
		public int Id { get; private set; }

		public int TrainingId { get; private set; }
		public virtual Training Training { get; private set; }

		public string Department { get; private set; }

		public int TypeId { get; private set; }
		public TrainingType Type { get; private set; }

		public double Duration { get; private set; }
		public int? DurationTypeId { get; private set; }
		public DurationType DurationType { get; private set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		private SpecialTraining()
		{

		}

		public SpecialTraining(string department, int typeId, double duration, int? durationTypeId)
		{
			this.Department = department;
			this.TypeId = typeId;
			this.Duration = duration;
			this.DurationTypeId = durationTypeId;
		}

		public void Update(string department, int typeId, double duration, int? durationTypeId)
		{
			this.Department = department;
			this.TypeId = typeId;
			this.Duration = duration;
			this.DurationTypeId = durationTypeId;
		}
	}
}
