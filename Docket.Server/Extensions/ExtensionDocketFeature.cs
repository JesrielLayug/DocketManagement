using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionDocketFeature
    {
        public static IEnumerable<DTODocketFeature>
            ConvertWithFeatures (
                this IEnumerable<Models.Docket> dockets, 
                IEnumerable<User> users, 
                IEnumerable<DocketFavorite> favorites
            )
        {
            return (from docket in dockets
                    join user in users on docket.UserId equals user.id
                    join favorite in favorites on docket.Id equals favorite.DocketId
                    select new DTODocketFeature
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        Rates = docket.Rate,
                        IsFavorite = favorite.IsFavorite,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
