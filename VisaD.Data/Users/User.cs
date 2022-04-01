using System;
using System.Text.Json.Serialization;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users.Enums;

namespace VisaD.Data.Users
{
	public class User : IEntity
	{
		public int Id { get; set; }
		public string Username { get; set; }

		[JsonIgnore]
		public string Password { get; set; }

		[JsonIgnore]
		public string PasswordSalt { get; set; }

		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Position { get; set; }


		public int RoleId { get; set; }
		public Role Role { get; set; }

		public int? InstitutionId { get; private set; }
		public Institution Institution { get; private set; }

		public UserStatus Status { get; set; }
		public bool IsLocked { get; set; }

		public DateTime? CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		public User(string username, string firstName, string middleName, string lastName, string email, string phone, int roleId, string position, int? institutionId)
		{
			this.Username = username;
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.Email = email?.Trim().ToLower();
			this.Phone = phone;
			this.RoleId = roleId;
			this.Position = position;
			this.InstitutionId = institutionId;

			this.IsLocked = true;
			this.CreateDate = DateTime.UtcNow;
			this.Status = UserStatus.Inactive;
		}

		public void Activate(string passwordHash, string passwordSalt)
		{
			this.Password = passwordHash;
			this.PasswordSalt = passwordSalt;
			this.IsLocked = false;
			this.UpdateDate = DateTime.UtcNow;
			this.Status = UserStatus.Active;
		}

		public void ChangePassword(string passwordHash, string passwordSalt)
		{
			this.Password = passwordHash;
			this.PasswordSalt = passwordSalt;
			this.UpdateDate = DateTime.UtcNow;
		}

		public void Update(string username, string email, string phone, string firstName, string middleName, string lastName, int? institutionId, int roleId, string position)
		{
			this.Username = username;
			this.Email = email?.Trim().ToLower();
			this.Phone = phone;
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.InstitutionId = institutionId;
			this.RoleId = roleId;
			this.Position = position;
		}
	}
}
