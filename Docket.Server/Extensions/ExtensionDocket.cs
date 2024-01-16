using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionDocket
    {
        public static IEnumerable<DTODocket> Convert(this IEnumerable<Models.Docket> dockets, IEnumerable<User> users)
        {
            return (from docket in dockets
                    join user in users 
                    on docket.UserId equals user.id
                    select new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
