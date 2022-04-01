using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications
{
	public class Applicant : IEntity, IAuditable, IConcurrency
	{
		public int Id { get; private set; }
		public int InstitutionId { get; private set; }
		public Institution Institution { get; private set; }
		public string FirstName { get; private set; }
		public string MiddleName { get; private set; }
		public string LastName { get; private set; }
		public string FullName { get; private set; }

		public string Position { get; private set; }

		public string Phone { get; private set; }
		public string Mail { get; private set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		public int Version { get; set; }

		private Applicant()
		{

		}

		public Applicant(int institutionId, string firstName, string middleName, string lastName, string position, string phone, string mail)
		{
			this.InstitutionId = institutionId;
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.Position = position;
			this.Phone = phone;
			this.Mail = mail;

			this.SetFullname(firstName, middleName, lastName);
		}

		public Applicant(Applicant applicant)
			: this(applicant.InstitutionId, applicant.FirstName, applicant.MiddleName, applicant.LastName, applicant.Position, applicant.Phone, applicant.Mail)
		{

		}

		public void Update(int institutionId, string firstName, string middleName, string lastName, string position, string phone, string mail)
		{
			this.InstitutionId = institutionId;
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.Position = position;
			this.Phone = phone;
			this.Mail = mail;

			this.SetFullname(firstName, middleName, lastName);
		}

		private void SetFullname(string firstName, string middleName, string lastName)
		{
			this.FullName = string.Join(" ", new List<string> { firstName, middleName, lastName }
					.Where(e => !string.IsNullOrWhiteSpace(e))
					.Select(e => e));
		}
	}
}
