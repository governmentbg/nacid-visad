using System.Collections.Generic;
using VisaD.Data.Common.Models;

namespace VisaD.Data.Nomenclatures
{
	public class RegisterIndex : Nomenclature
	{
		public string Alias { get; set; }

		public string Format { get; set; }

		public ICollection<RegisterIndexCounter> Counters { get; set; }
	}
}
