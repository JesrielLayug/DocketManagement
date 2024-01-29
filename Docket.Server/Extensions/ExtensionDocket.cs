using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionDocket
    {
        public static IEnumerable<DTODocket>
        Convert(
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
                    join favorite in favorites on new { DocketId = docket.Id, UserId = userId } equals new { favorite.DocketId, favorite.UserId } into docketFavorites
                    from favorite in docketFavorites.DefaultIfEmpty() 
                    select new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        Ratings = docketRatings.Select(dr => dr.Rating).ToList(),
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
