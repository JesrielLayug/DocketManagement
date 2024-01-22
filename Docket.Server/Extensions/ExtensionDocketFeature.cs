using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionDocketFeature
    {
        public static IEnumerable<DTODocketFeature>
            Convert (
                this IEnumerable<Models.Docket> dockets, 
                IEnumerable<User> users, 
                IEnumerable<DocketRate> rates, 
                IEnumerable<DocketFavorite> favorites
            )
        {
            return (from docket in dockets
                    join user in users on docket.UserId equals user.id
                    join rate in rates on docket.UserId equals rate.UserId
                    join favorite in favorites on docket.UserId equals favorite.UserId
                    select new DTODocketFeature
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        Rates = (rate.Rate != null) ? rate.Rate : 0,
                        IsFavorite = favorite.IsFavorite,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
