using System.Collections.Generic;
using VisaD.Data.Applications;

namespace VisaD.Application.Applications.Dtos
{
	public class DocumentDto
	{
		public IEnumerable<ApplicationFileDto> Files { get; set; } = new List<ApplicationFileDto>();

		public bool AreIdenticalFiles { get; set; }

		public string Description { get; set; }
		public Document ToModel()
		{
			var document = new Document();
			foreach (var item in this.Files)
			{
				if (item.AttachedFile != null)
				{
					document.AddFile(
					item.Type?.Id,
					item.AttachedFile.Key,
					item.AttachedFile.Hash,
					item.AttachedFile.Size,
					item.AttachedFile.Name,
					item.AttachedFile.MimeType,
					item.AttachedFile.DbId,
					item.FileDescription
				);
				}
			}

			document.AreIdenticalFiles = this.AreIdenticalFiles;
			document.Description = this.Description;

			return document;
		}
	}
}
