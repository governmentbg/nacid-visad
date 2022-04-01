using System;
using System.Linq.Expressions;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;
using VisaD.Data.Users.Enums;

namespace VisaD.Application.Users.Dtos
{
	public class UserEditDto
	{
		public int Id { get; set; }
		public string Username { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }
		public string Phone { get; set; }

		public UserStatus Status { get; set; }

		public NomenclatureDto<Institution> Institution { get; set; }

		public int? InstitutionId { get; set; }

		public Role Role { get; set; }

		public string Position { get; set; }

		public static Expression<Func<User, UserEditDto>> SelectExpression => e => new UserEditDto {
			Id = e.Id,
			Username = e.Username,
			Email = e.Email,
			Phone = e.Phone,
			FirstName = e.FirstName,
			MiddleName = e.MiddleName,
			LastName = e.LastName,
			Status = e.Status,
			Role = new Role {
				Id = e.RoleId,
				Alias = e.Role.Alias,
				Name = e.Role.Name
			},
			Institution = e.Institution != null

						 ? new NomenclatureDto<Institution> {
							 Id = e.Institution.Id,
							 Name = e.Institution.Name
						 }
						 : null,
			Position = e.Position
		};

	}
}
