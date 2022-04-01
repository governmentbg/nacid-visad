using FileStorageNetCore.Models;
using System;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications.AttachedFiles
{
    public class RepresentativeDocumentFile : AttachedFile, IEntity
    {
        public int Id { get; set; }

        public int RepresentativeId { get; set; }

		public Representative Representative { get; set; }

		public RepresentativeDocumentType Type { get; set; }

		public RepresentativeDocumentFile()
        {

        }

		public RepresentativeDocumentFile(Guid key, string hash, long size, string name, string mimeType, int dbId, RepresentativeDocumentType type)
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
