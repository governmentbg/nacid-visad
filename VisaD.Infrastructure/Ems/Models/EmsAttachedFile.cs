using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisaD.Infrastructure.Ems.Models
{
    public class EmsAttachedFile
	{
		public string Name { get; set; }

		public string MimeType { get; set; }

		public string Key { get; set; }

		public string Hash { get; set; }

		public int Size { get; set; }

		public int DbId { get; set; }

		public string Description { get; set; }
	}
}
