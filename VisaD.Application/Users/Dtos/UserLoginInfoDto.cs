using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Users.Dtos
{
	public class UserLoginInfoDto
	{
		public string Fullname { get; set; }
		public string Token { get; set; }
		public NomenclatureDto<Institution> Institution { get; set; }
		public string RoleAlias { get; set; }
	}
}
