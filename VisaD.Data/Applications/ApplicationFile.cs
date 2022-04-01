using FileStorageNetCore.Models;
using System;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications
{
	public class ApplicationFile : AttachedFile, IEntity
	{
		public int Id { get; private set; }

		public int DocumentId { get; private set; }
		public Document Document { get; private set; }

		public int? TypeId { get; private set; }
		public ApplicationFileType Type { get; private set; }

		public string FileDescription { get; private set; }

		private ApplicationFile()
		{

		}

		public ApplicationFile(int? typeId, Guid key, string hash, long size, string name, string mimeType, int dbId, string fileDescription)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{
			this.TypeId = typeId;
			this.FileDescription = fileDescription;
		}

		public void Update(int? typeId, Guid key, string hash, long size, string name, string mimeType, int dbId, string fileDescription)
		{
			this.TypeId = typeId;
			this.Key = key;
			this.Hash = hash;
			this.Size = size;
			this.Name = name;
			this.MimeType = mimeType;
			this.DbId = dbId;
			this.FileDescription = fileDescription;
		}
	}
}
