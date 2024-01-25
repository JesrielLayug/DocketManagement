using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionDocket
    {
        public static IEnumerable<DTODocket> 
            Convert (
                this IEnumerable<Models.Docket> dockets,
                IEnumerable<User> users,
                IEnumerable<Rate> rates,
                IEnumerable<Favorite> favorites,
                string userId
            )
        {
            return (from docket in dockets
                    join user in users on docket.UserId equals user.id
                    join rate in rates on docket.Id equals rate.DocketId into docketRatings
                    join favorite in favorites on docket.UserId equals favorite.UserId into docketFavorites
                    select new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        Ratings = docketRatings.Select(dr => dr.Rating).ToList(),
                        IsFavorite = docketFavorites.Any(fav => fav.UserId == userId),
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
