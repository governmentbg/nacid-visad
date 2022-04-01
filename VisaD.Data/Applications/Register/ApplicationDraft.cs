using System;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications.Register
{
	public class ApplicationDraft : IEntity
	{
		public int Id { get; set; }

		public string Content { get; set; }

		public int UserId { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime? ModificationDate { get; set; }
	}
}
