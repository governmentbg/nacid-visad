using System;
using System.ComponentModel.DataAnnotations;

namespace VisaD.Data.Users
{
	public class PasswordToken
	{
		[Key]
		public string Value { get; set; }

		public DateTime ExpirationTime { get; set; }

		public bool IsUsed { get; set; }

		public int UserId { get; set; }

		public User User { get; set; }

		public PasswordToken()
		{

		}

		public PasswordToken(int userId, int expirationMinutes)
		{
			this.Value = Guid.NewGuid().ToString();
			this.ExpirationTime = DateTime.UtcNow.AddMinutes(expirationMinutes);
			this.IsUsed = false;
			this.UserId = userId;
		}

		public void Use()
		{
			this.IsUsed = true;
		}
	}
}
