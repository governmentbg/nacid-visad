using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Applications.AttachedFiles;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Enums;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class Representative : IEntity, IConcurrency, IAuditable
	{
		public int Id { get; private set; }

		public bool HasRepresentative { get; private set; }
		public RepresentativeType? Type { get; private set; }

		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string Fullname { get; private set; }

		public string IdentificationCode { get; private set; }
		public string Mail { get; private set; }
		public string Phone { get; private set; }

		public string Note { get; private set; }

		public int Version { get; set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		public virtual List<RepresentativeDocumentFile> RepresentativeDocumentFiles { get; set; } = new List<RepresentativeDocumentFile>();

		public DateTime SubmissionDate { get; private set; }

		private Representative()
		{

		}

		public Representative(bool hasRepresentative, RepresentativeType? type, string firstName, string lastName, string identificationCode, string mail, string phone,
			string note, DateTime submissionDate, List<RepresentativeDocumentFile> representativeDocumentFiles)
		{
			this.HasRepresentative = hasRepresentative;
			this.Type = type;

			this.FirstName = firstName;
			this.LastName = lastName;
			this.Fullname = $"{firstName} {lastName}";

			this.IdentificationCode = identificationCode;
			this.Mail = mail;
			this.Phone = phone;
			this.Note = note;
			this.SubmissionDate = submissionDate;


			foreach (var file in representativeDocumentFiles)
			{
				if (file != null)
				{
					this.AddFile(file);
				}
			}
		}

		public Representative(Representative representative)
			: this(representative.HasRepresentative, representative.Type.Value, representative.FirstName, representative.LastName, representative.IdentificationCode,
				  representative.Mail, representative.Phone, representative.Note, representative.SubmissionDate, representative.RepresentativeDocumentFiles)
		{
		}

		public void Update(bool hasRepresentative, RepresentativeType? type, string firstName, string lastName, string identificationCode, string mail, string phone,
			string note, DateTime submissioDate)
		{
			this.HasRepresentative = hasRepresentative;
			this.Type = type;

			this.FirstName = firstName;
			this.LastName = lastName;
			this.Fullname = $"{firstName} {lastName}";

			this.IdentificationCode = identificationCode;
			this.Mail = mail;
			this.Phone = phone;
			this.Note = note;
			this.SubmissionDate = submissioDate;
		}

		public RepresentativeDocumentFile AddFile(RepresentativeDocumentFile file)
		{
			var document = new RepresentativeDocumentFile(file.Key, file.Hash, file.Size, file.Name, file.MimeType, file.DbId, file.Type);
			this.RepresentativeDocumentFiles.Add(document);

			return document;
		}

		public void UpdateFile(RepresentativeDocumentFile file)
		{
			var document = this.RepresentativeDocumentFiles.SingleOrDefault(e => e.Type == file.Type);

			if (document != null)
			{
				document.Update(file.Key, file.Hash, file.Size, file.Name, file.MimeType, file.DbId);
			}
		}
	}
}
