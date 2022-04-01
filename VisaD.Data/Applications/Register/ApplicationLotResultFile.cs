using FileStorageNetCore.Models;
using System;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications.Register
{
	public class ApplicationLotResultFile : AttachedFile, IEntity
	{
		public int Id { get; set; }

		public int ApplicationLotResultId { get; set; }

		private ApplicationLotResultFile()
		{

		}

		public ApplicationLotResultFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{ }
	}
}
