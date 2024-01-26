using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionRate
    {
        public static IEnumerable<DTODocket>
            WithRate(
                this IEnumerable<DTODocket> dto_dockets,
                IEnumerable<Rate> rates
            )
        {
            return (from user_rate in rates
                    join docket in dto_dockets on user_rate.DocketId equals docket.Id
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
