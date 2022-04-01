using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class Document : IEntity, IAuditable
	{
		public int Id { get; set; }

		public int CreatorUserId { get; set; }
		public DateTime CreateDate { get; set; }

		private HashSet<ApplicationFile> _files = new HashSet<ApplicationFile>();
		public IReadOnlyCollection<ApplicationFile> Files => _files.ToList().AsReadOnly();

		public string Description { get; set; }

		public bool AreIdenticalFiles { get; set; }


		public Document()
		{

		}

		public Document(Document document)
			: this()
		{
			this.AreIdenticalFiles = document.AreIdenticalFiles;
			this.Description = document.Description;

			foreach (var item in document.Files)
			{
				AddFile(
					item.TypeId,
					item.Key,
					item.Hash,
					item.Size,
					item.Name,
					item.MimeType,
					item.DbId,
					item.FileDescription
				);
			}
		}

		public ApplicationFile AddFile(int? typeId,  Guid key, string hash, long size, string name, string mimeType, int dbId, string fileDescription)
		{
			var file = new ApplicationFile(typeId, key, hash, size, name, mimeType, dbId, fileDescription);
			this._files.Add(file);

			return file;
		}

		public ApplicationFile UpdateFile(int id, int? typeId, Guid key, string hash, long size, string name, string mimeType, int dbId, string fileDescription)
		{
			var file = this._files.Single(e => e.Id == id);
			file.Update(typeId, key, hash, size, name, mimeType, dbId, fileDescription);

			return file;
		}

		public void RemoveFile(int id)
		{
			var file = this._files.Single(e => e.Id == id);
			this._files.Remove(file);
		}
	}
}
