using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task Register(User user);
    }
}
