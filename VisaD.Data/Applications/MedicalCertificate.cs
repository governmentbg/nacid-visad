using FileStorageNetCore.Models;
using System;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class MedicalCertificate : AttachedFile, IEntity
	{
		public int Id { get; set; }

		public DateTime IssuedDate { get; set; }

		public MedicalCertificate()
		{

		}

		public MedicalCertificate(Guid key, string hash, long size, string name, string mimeType, int dbId, DateTime issuedDate)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{
			this.IssuedDate = issuedDate;
		}

		public MedicalCertificate(MedicalCertificate medicalCertificate)
			: this(medicalCertificate.Key, medicalCertificate.Hash, medicalCertificate.Size, medicalCertificate.Name, medicalCertificate.MimeType, medicalCertificate.DbId,
				  medicalCertificate.IssuedDate)
		{
		}

		public void Update(Guid key, string hash, long size, string name, string mimeType, int dbId, DateTime issuedDate)
		{
			this.Key = key;
			this.Hash = hash;
			this.Size = size;
			this.Name = name;
			this.MimeType = mimeType;
			this.DbId = dbId;
			this.IssuedDate = issuedDate;
		}
	}
}
