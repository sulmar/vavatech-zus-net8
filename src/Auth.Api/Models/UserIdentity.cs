namespace Auth.Api.Models;

public class UserIdentity
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string[] Roles { get; set; }
}
