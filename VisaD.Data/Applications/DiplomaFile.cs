using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Applications.Diplomas;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications
{
	public class DiplomaFile : IEntity
	{
		public int Id { get; set; }

		public string DiplomaNumber { get; private set; }

		public DateTime IssuedDate { get; private set; }

		public int? CountryId { get; private set; }

		public Country Country { get; private set; }
		public string City { get; private set; }

		public string OrganizationName { get; private set; }

		public int DiplomaId { get; private set; }

		public Diploma Diploma { get; private set; }

		public int DiplomaTypeId { get; private set; }

		public DiplomaType DiplomaType { get; private set; }

		public virtual List<DiplomaDocumentFile> DiplomaDocumentFiles { get; set; } = new List<DiplomaDocumentFile>();

		private DiplomaFile()
		{

		}

		public DiplomaFile(string diplomaNumber, DateTime issuedDate, int? countryId, string city, string organizationName, int diplomaTypeId)
		{
			this.DiplomaNumber = diplomaNumber;
			this.IssuedDate = issuedDate;
			this.CountryId = countryId;
			this.City = city;
			this.OrganizationName = organizationName;
			this.DiplomaTypeId = diplomaTypeId;
		}

		public DiplomaFile(DiplomaFile diploma)
			: this(diploma.DiplomaNumber, diploma.IssuedDate, diploma.CountryId, diploma.City, diploma.OrganizationName, diploma.DiplomaTypeId)
		{
			foreach (var document in diploma.DiplomaDocumentFiles)
			{
				this.AddDiplomaFile(document.Key, document.Hash, document.Size, document.Name,
					document.MimeType, document.DbId, document.Type);
			}
		}

		public void Update(string diplomaNumber, DateTime issuedDate, int? countryId, string city, string organizationName, int diplomaTypeId)
		{
			this.DiplomaNumber = diplomaNumber;
			this.IssuedDate = issuedDate;
			this.CountryId = countryId;
			this.City = city;
			this.OrganizationName = organizationName;
			this.DiplomaTypeId = diplomaTypeId;
		}

		public void UpdateFile(Guid key, string hash, long size, string name, string mimeType, int dbId, int id)
		{
			var file = this.DiplomaDocumentFiles.SingleOrDefault(e => e.Id == id);

			if (file != null)
			{
				file.Update(key, hash, size, name, mimeType, dbId);
			}
		}

		public DiplomaDocumentFile AddDiplomaFile(Guid key, string hash, long size, string name, string mimeType, int dbId, DiplomaDocumentType documentType)
		{
			var document = new DiplomaDocumentFile(key, hash, size, name, mimeType, dbId, documentType);
			this.DiplomaDocumentFiles.Add(document);

			return document;
		}
	}
}
