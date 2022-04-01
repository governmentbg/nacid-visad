namespace VisaD.Application.Common.Interfaces
{
	public interface IUserContext
	{
		int UserId { get; }
		string Username { get; }
		string OrganizationName { get; }
		string Role { get; }
	}
}
