using System;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications.Register
{
	public class ApplicationLotResult : IEntity, IAuditable
	{
		public int Id { get; set; }

		public int LotId { get; private set; }
		public ApplicationLot Lot { get; private set; }

		public ApplicationLotResultType Type { get; set; }

		public ApplicationLotResultFile File { get; private set; }
		public string CertificateNumber { get; private set; }
		public string Note { get; private set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }
		
		public string AccessCode { get; private set; }

		public bool IsSigned { get; private set; } = false;
		public DateTime? SigningDate { get; private set; }
		public int? SignerUserId { get; private set; }

		public int? RegulationId { get; private set; }
		public Regulation Regulation { get; private set; }

		private ApplicationLotResult()
		{

		}

		public ApplicationLotResult(ApplicationLotResultType type, string note, string certificateNumber, string accessCode, int? regulationId)
		{
			this.Type = type;
			this.Note = note;
			this.CertificateNumber = certificateNumber;
			this.AccessCode = accessCode;
			this.RegulationId = regulationId;
		}

		public void AddFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			if (this.File != null)
			{
				this.IsSigned = false;
				this.SignerUserId = null;
				this.SigningDate = null;

				this.UpdateFile(key, hash, size, name, mimeType, dbId);
			}
			else
			{
				var file = new ApplicationLotResultFile(key, hash, size, name, mimeType, dbId);
				this.File = file;
			}
		}

		public void Update(ApplicationLotResultType type, string note, string certificateNumber, string accessCode, int regulationId)
		{
			this.Type = type;
			this.Note = note;
			this.CertificateNumber = certificateNumber;
			this.AccessCode = accessCode;
			this.RegulationId = regulationId;
		}

		public void UpdateFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			this.File.Key = key;
			this.File.Hash = hash;
			this.File.Size = size;
			this.File.Name = name;
			this.File.MimeType = mimeType;
			this.File.DbId = dbId;
		}

		public void Sign(int userId)
		{
			this.IsSigned = true;
			this.SigningDate = DateTime.UtcNow;
			this.SignerUserId = userId;
		}
	}
}
