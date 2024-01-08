
using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<Response> Register(DTOUserRegister request);
        Task<Response> Login(DTOUserLogin request);
    }
}
