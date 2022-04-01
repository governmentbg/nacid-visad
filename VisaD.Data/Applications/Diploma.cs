using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Applications.Diplomas;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class Diploma : IEntity, IAuditable
	{
		public int Id { get; private set; }

		public DateTime CreateDate { get; set; }

		public int CreatorUserId { get; set; }

		private HashSet<DiplomaFile> _diplomaFiles = new HashSet<DiplomaFile>();

		public IReadOnlyCollection<DiplomaFile> DiplomaFiles => _diplomaFiles.ToList().AsReadOnly();

		public virtual List<DiplomaDocumentFile> AttachedFiles { get; set; } = new List<DiplomaDocumentFile>();

		public string Description { get; set; }


		public Diploma()
		{

		}

		public Diploma(Diploma diploma)
			: this()
		{
			this.Description = diploma.Description;

			foreach (var attachedFile in diploma.AttachedFiles)
			{
				this.AddAttachedFile(attachedFile.Key, attachedFile.Hash, attachedFile.Size, attachedFile.Name, attachedFile.MimeType, attachedFile.DbId, attachedFile.Type);
			}

			foreach (var item in diploma.DiplomaFiles)
			{
				this.AddFile(
					item.DiplomaNumber,
					item.IssuedDate,
					item.CountryId,
					item.City,
					item.OrganizationName,
					item.DiplomaTypeId,
					item.DiplomaDocumentFiles
				);
			}
		}

		public DiplomaFile AddFile(string diplomaNumber, DateTime issuedDate, int? countryId, string city,
			string organizationName, int diplomaTypeId, List<DiplomaDocumentFile> documents)
		{
			var file = new DiplomaFile(diplomaNumber, issuedDate, countryId, city, organizationName, diplomaTypeId);

			foreach (var document in documents)
			{
				file.AddDiplomaFile(document.Key, document.Hash, document.Size, document.Name, document.MimeType, document.DbId, document.Type);
			}

			this._diplomaFiles.Add(file);
			return file;
		}

		public DiplomaFile UpdateFile(int id, string diplomaNumber, DateTime issuedDate, int? countryId, string city, string organizationName, int diplomaTypeId)
		{
			var diplomaFile = this._diplomaFiles.Single(e => e.Id == id);

			diplomaFile.Update(diplomaNumber, issuedDate, countryId, city, organizationName, diplomaTypeId);
			
			return diplomaFile;
		}

		public void RemoveFile(int id)
		{
			var diplomaFile = this._diplomaFiles.Single(e => e.Id == id);
			this._diplomaFiles.Remove(diplomaFile);
		}

		//Add attached file to Diploma model
		public DiplomaDocumentFile AddAttachedFile(Guid key, string hash, long size, string name, string mimeType, int dbId, DiplomaDocumentType documentType)
		{
			var document = new DiplomaDocumentFile(key, hash, size, name, mimeType, dbId, documentType);
			this.AttachedFiles.Add(document);

			return document;
		}

		//Update attached file in Diploma model
		public void UpdateAttachedFile(Guid key, string hash, long size, string name, string mimeType, int dbId, DiplomaDocumentType type)
		{
			var file = this.AttachedFiles.SingleOrDefault(e => e.Type == type);

			if (file != null)
			{
				file.Update(key, hash, size, name, mimeType, dbId);
			}
		}
	}
}
