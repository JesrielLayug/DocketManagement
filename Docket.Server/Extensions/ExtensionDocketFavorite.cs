using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionDocketFavorite
    {
        public static IEnumerable<DTODocketWithRateAndFavorite>
            Favorite (
                this IEnumerable<Models.Docket> dockets, 
                IEnumerable<User> users, 
                IEnumerable<DocketFavorite> favorites,
                IEnumerable<DocketRate> rates
            )
        {
            return (from docket in dockets
                    join user in users on docket.UserId equals user.id
                    join favorite in favorites on docket.UserId equals favorite.UserId
                    join rate in rates on docket.Id equals rate.DocketId
                    select new DTODocketWithRateAndFavorite
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        Rates = rate.Rate,
                        IsFavorite = favorite.IsFavorite,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
