using FileStorageNetCore.Models;
using System;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class TrainingLanguageDocumentFile : AttachedFile, IEntity
	{
		public int Id { get; set; }

		public int TrainingId { get; set; }

		public Training Training { get; set; }

		public TrainingLanguageDocumentFile()
		{

		}

		public TrainingLanguageDocumentFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{
		}

		public void Update(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			this.Key = key;
			this.Hash = hash;
			this.Size = size;
			this.Name = name;
			this.MimeType = mimeType;
			this.DbId = dbId;
		}
	}
}
