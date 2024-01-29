using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionRate
    {
        public static IEnumerable<DTODocket>
            WithRates(
                this IEnumerable<Models.Docket> dockets,
                IEnumerable<Rate> rates,
                IEnumerable<Favorite> favorites,
                IEnumerable<User> users,
                string userId
            )
        {
            return (from rate in rates.Where(r => r.UserId == userId)
                    join docket in dockets on rate.DocketId equals docket.Id 
                    join user in users on userId equals user.id
                    join favorite in favorites on new { DocketId = docket.Id, UserId = userId } equals new { favorite.DocketId, favorite.UserId } into docketFavorites
                    from favorite in docketFavorites.DefaultIfEmpty()
                    select new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        Ratings = rates.Where(r => r.DocketId == docket.Id).Select(r => r.Rating).Distinct().ToList(),
                        IsFavorite = favorite != null && favorite.IsFavorite,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
