using StudentWebAPI.Identity;

namespace StudentWebAPI.Service
{
    public interface ITokenService
    {
        string CreateToken(
            ApplicationUser user,
            IList<string> roles);
    }
}
