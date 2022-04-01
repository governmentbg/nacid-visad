using System;

namespace VisaD.Application.Ems.Models
{
	public class FileRequestDto
	{
		public string Content { get; set; }

		public Guid Key { get; set; }
		public int DbId { get; set; }
		public string Name { get; set; }

		public bool ManuallyInserted { get; set; }
	}
}
