﻿using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class DocketFavoriteService : IDocketFavoriteService
    {
        private readonly HttpClient httpClient;

        public DocketFavoriteService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Response> Add(DTOFeatureFavorite favorite)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/Favorite/Add", favorite);
                if (response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = true,
                        message = "Docket successfully added to favorite"
                    };
                }
                return new Response
                {
                    isSuccess = false,
                    message = "Failed adding the docket to favorite"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response { isSuccess = false, message = e.Message };
            }
        }

        public async Task<Response> Remove(string docketId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Favorite/Remove{docketId}");
                if (response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = true,
                        message = "Docket successfully added to favorite"
                    };
                }
                return new Response
                {
                    isSuccess = false,
                    message = "Failed adding the docket to favorite"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response { isSuccess = false, message = e.Message };
            }
        }
    }
}