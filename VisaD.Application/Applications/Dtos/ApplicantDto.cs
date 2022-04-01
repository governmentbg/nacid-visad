using System;
using System.Linq.Expressions;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicantDto
	{
		public NomenclatureDto<Institution> Institution { get; set; }

		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public string Position { get; set; }

		public string Phone { get; set; }
		public string Mail { get; set; }

		public static Expression<Func<User, ApplicantDto>> SelectUserExpression
			=> user => new ApplicantDto {
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Mail = user.Email,
				Phone = user.Phone,
				Position = user.Position,
				Institution = user.Institution != null ? new NomenclatureDto<Institution> {
					Id = user.Institution.Id,
					Name = user.Institution.Name
				} : null
			};

		public Applicant ToModel()
			=> new Applicant(this.Institution.Id, this.FirstName, this.MiddleName, this.LastName, this.Position, this.Phone, this.Mail);
	}
}
