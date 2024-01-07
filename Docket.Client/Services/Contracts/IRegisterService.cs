
using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IRegisterService
    {
        Task<Response> Register(DTOUserRegister request);
    }
}
