using FileStorageNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Candidates
{
	public class Candidate : AttachedFile, IEntity, IAuditable, IConcurrency
	{
		public int Id { get; private set; }

		public string PassportNumber { get; private set; }
		public DateTime PassportValidUntil { get; private set; }

		public string FirstName { get; set; }
		public string LastName { get; private set; }
		public string Fullname { get; private set; }
		public string OtherNames { get; private set; }

		public string FirstNameCyrillic { get; private set; }
		public string LastNameCyrillic { get; private set; }
		public string FullNameCyrillic { get; private set; }
		public string OtherNamesCyrillic { get; private set; }

		public DateTime BirthDate { get; private set; }
		public string BirthPlace { get; private set; }

		public int CountryId { get; private set; }
		public Country Country { get; private set; }
		
		public int NationalityId { get; private set; }
		public Country Nationality { get; private set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		public string Phone { get; private set; }
		public string Mail { get; private set; }

		public int Version { get; set; }

		private HashSet<CandidateNationality> _otherNationalities = new HashSet<CandidateNationality>();
		public IReadOnlyCollection<CandidateNationality> OtherNationalities => _otherNationalities.ToList().AsReadOnly();

		public CandidatePassportDocument CandidatePassportDocument { get; set; }

		private Candidate()
		{
		}

		public Candidate(string firstName, string lastName, DateTime birthDate, string birthPlace, int nationalityId, string passportNumber, DateTime passportValidUntil, int countryId, string phone, string mail,
			Guid key, string hash, long size, string name, string mimeType, int dbId, string otherNames, string firstNameCyrillic,
			string lastNameCyrillic, string otherNamesCyrillic)
			: base(new BlobDescriptor {
				Key = key,
				Hash = hash,
				Size = size,
				Name = name,
				MimeType = mimeType
			}, dbId)
		{
			this.PassportNumber = passportNumber;
			this.PassportValidUntil = passportValidUntil;

			this.FirstName = firstName;
			this.LastName = lastName;
			this.OtherNames = otherNames;
			this.Fullname = $"{firstName} {lastName}";
			this.BirthDate = birthDate;
			this.BirthPlace = birthPlace;

			this.CountryId = countryId;
			this.NationalityId = nationalityId;

			this.Phone = phone;
			this.Mail = mail;

			this.FirstNameCyrillic = firstNameCyrillic;
			this.LastNameCyrillic = lastNameCyrillic;
			this.OtherNamesCyrillic = otherNamesCyrillic;
			this.FullNameCyrillic = $"{firstNameCyrillic} {lastNameCyrillic}";
		}

		public Candidate(Candidate candidate)
			:this(candidate.FirstName, candidate.LastName, candidate.BirthDate, candidate.BirthPlace, candidate.NationalityId, candidate.PassportNumber, candidate.PassportValidUntil, candidate.CountryId, candidate.Phone, candidate.Mail,
				 candidate.Key, candidate.Hash, candidate.Size, candidate.Name, candidate.MimeType, candidate.DbId, candidate.OtherNames,
				 candidate.FirstNameCyrillic, candidate.LastNameCyrillic, candidate.OtherNamesCyrillic)
		{
			foreach (var item in candidate.OtherNationalities)
			{
				this.AddNationality(item.NationalityId);
			}

			this.AddFile(
				candidate.CandidatePassportDocument.Key,
				candidate.CandidatePassportDocument.Hash,
				candidate.CandidatePassportDocument.Size,
				candidate.CandidatePassportDocument.Name,
				candidate.CandidatePassportDocument.MimeType,
				candidate.CandidatePassportDocument.DbId
				);
		}

		public void AddNationality(int nationalityId)
		{
			var existingNationality = this._otherNationalities.FirstOrDefault(x => x.NationalityId == nationalityId);

			if (existingNationality == null)
			{
				var nationality = new CandidateNationality(nationalityId);

				this._otherNationalities.Add(nationality);
			}
		}

		public void RemoveNationality(int nationalityId)
		{
			var nationality = this._otherNationalities.Single(e => e.NationalityId == nationalityId);
			this._otherNationalities.Remove(nationality);
		}

		public void Update(string firstName, string lastName, DateTime birthDate, string birthPlace, int nationalityId,
			string passportNumber, DateTime passportValidUntil, int countryId, string phone, string mail, Guid key, string hash, long size, string name, string mimeType, int dbId,
			string otherNames, string firstNameCyrillic, string lastNameCyrillic, string otherNamesCyrillic)
		{
			this.PassportNumber = passportNumber;
			this.PassportValidUntil = passportValidUntil;

			this.FirstName = firstName;
			this.LastName = lastName;
			this.OtherNames = otherNames;
			this.Fullname = $"{firstName} {lastName}";
			this.BirthDate = birthDate;
			this.BirthPlace = birthPlace;

			this.CountryId = countryId;
			this.NationalityId = nationalityId;

			this.Phone = phone;
			this.Mail = mail;

			this.Key = key;
			this.Hash = hash;
			this.Size = size;
			this.Name = name;
			this.MimeType = mimeType;
			this.DbId = dbId;

			this.FirstNameCyrillic = firstNameCyrillic;
			this.LastNameCyrillic = lastNameCyrillic;
			this.OtherNamesCyrillic = otherNamesCyrillic;
		}

		public CandidatePassportDocument AddFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			var document = new CandidatePassportDocument(key, hash, size, name, mimeType, dbId);
			this.CandidatePassportDocument = document;

			return document;
		}

		public void UpdateFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			this.CandidatePassportDocument.Update(key, hash, size, name, mimeType, dbId);
		}
	}
}
