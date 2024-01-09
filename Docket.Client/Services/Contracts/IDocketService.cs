using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketService
    {
        Task<IEnumerable<DTODocket>> GetAll();
    }
}
