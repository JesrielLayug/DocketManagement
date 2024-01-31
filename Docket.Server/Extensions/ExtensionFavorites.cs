using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionFavorites
    {
        public static IEnumerable<DTODocket>
            UserFavorites(
                this IEnumerable<Models.Docket> dockets,
                IEnumerable<Favorite> favorites,
                IEnumerable<Rate> rates,
                IEnumerable<User> users,
                string userId
            )
        {
            return (from favorite in favorites.Where(f => f.UserId == userId)
                    join docket in dockets on favorite.DocketId equals docket.Id
                    join rate in rates on docket.Id equals rate.DocketId into docketRatings
                    join user in users on userId equals user.id
                    select new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        Ratings = docketRatings.Select(dr => dr.Rating).ToList(),
                        IsFavorite = favorite.IsFavorite,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
