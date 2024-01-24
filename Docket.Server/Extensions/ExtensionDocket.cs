﻿using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Extensions
{
    public static class ExtensionDocket
    {
        public static IEnumerable<DTODocket> 
            Docket (
                this IEnumerable<Models.Docket> dockets,
                IEnumerable<User> users,
                IEnumerable<DocketRate> rates
            )
        {
            return (from docket in dockets
                    join user in users on docket.UserId equals user.id
                    join rate in rates on docket.Id equals rate.DocketId into docketRatings
                    select new DTODocket
                    {
                        Id = docket.Id,
                        Title = docket.Title,
                        Body = docket.Body,
                        Ratings = docketRatings.Select(dr => dr.Rate).ToList(),
                        DateCreated = docket.DateCreated,
                        DateModified = docket.DateModified,
                        IsPublic = docket.IsPublic,
                        UserId = docket.UserId,
                        Username = user.name
                    }).ToList();
        }
    }
}
