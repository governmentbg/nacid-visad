using FileStorageNetCore.Models;
using System;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications.Diplomas
{
	public class DiplomaDocumentFile : AttachedFile, IEntity
	{
		public int Id { get; set; }

		public int? DiplomaFileId { get; set; }

		public DiplomaFile DiplomaFile { get; set; }

		public int? DiplomaId { get; set; }

		public Diploma Diploma { get; set; }

		public DiplomaDocumentType Type { get; set; }

		public DiplomaDocumentFile()
		{

		}

		public DiplomaDocumentFile(Guid key, string hash, long size, string name, string mimeType, int dbId, DiplomaDocumentType type)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{
			this.Type = type;
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
