using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionFavorite
    {
        public static IEnumerable<DTODocket>
            WithFavorite(
                this IEnumerable<DTODocket> dto_dockets,
                IEnumerable<Favorite> user_favorites,
                IEnumerable<Rate> user_rates
            )
        {
            return (from user_favorite in user_favorites
                    join docket in dto_dockets on user_favorite.DocketId equals docket.Id
                    join user_rate in user_rates on docket.Id equals user_rate.DocketId
                    select new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        Ratings = docket.Ratings,
                        CurrentUserRate = user_rate.Rating,
                        IsFavorite = docket.IsFavorite,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        UserId = docket.UserId,
                        Username = docket.Username
                    }).ToList();
        }
    }
}
