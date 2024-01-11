namespace Docket.Server.Services.Contracts
{
    public interface IDocketService
    {
        Task<IEnumerable<Models.Docket>> GetAll();
        Task<Models.Docket> GetById(string id);
        Task<IEnumerable<Models.Docket>> GetByUserId(string userId);
        Task Add(Models.Docket docket);
        Task Update(string id, Models.Docket docket);
        Task Delete(string id);
        Task<IEnumerable<Models.Docket>> GetAllPublic();
    }
}
