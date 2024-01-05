using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task Register(User user);
        Task<User> Login(string id);
        void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
        string CreateToken(User user);
    }
}
