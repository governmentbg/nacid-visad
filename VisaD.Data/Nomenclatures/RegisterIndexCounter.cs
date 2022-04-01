using System;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Nomenclatures
{
	public class RegisterIndexCounter : IEntity
	{
		public int Id { get; set; }

		public int RegisterIndexId { get; set; }
		public RegisterIndex RegisterIndex { get; set; }

		public int Counter { get; set; }
		public int Year { get; set; } = DateTime.UtcNow.Year;
	}
}
