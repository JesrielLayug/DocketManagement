﻿using Docket.Shared;
using System.Globalization;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketService
    {
        Task<IEnumerable<DTODocket>> GetAll();
        Task<IEnumerable<DTODocket>> GetUserDocket();
        Task<DTODocket> GetDocketById(string  docketId);
        Task<Response> Add(DTODocketCreate docket);
        Task<Response> Update(DTODocketUpdate docket);
        Task<Response> Delete(string docketId);
    }
}
