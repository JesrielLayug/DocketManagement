using Docket.Client.Pages.UserPage;
using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<DTOUser>> GetAll();
    }
}
