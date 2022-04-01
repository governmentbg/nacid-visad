using FileStorageNetCore.Models;
using System;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Candidates
{
	public class CandidatePassportDocument : AttachedFile, IEntity
	{
		public int Id { get; set; }

		public int CandidateId { get; set; }
		public Candidate Candidate { get; set; }

		public CandidatePassportDocument()
		{

		}

		public CandidatePassportDocument(Guid key, string hash, long size, string name, string mimeType, int dbId)
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
