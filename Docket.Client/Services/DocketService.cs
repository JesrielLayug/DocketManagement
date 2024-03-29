﻿using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class DocketService : IDocketService
    {
        private readonly HttpClient httpClient;

        public DocketService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Response> Add(DTODocketCreate docket)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<DTODocketCreate>("Docket/Create", docket);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return new Response
                    {
                        isSuccess = true,
                        statusCode = response.StatusCode,
                        message = "Successfully added the docket."
                    };
                }

                return new Response
                {
                    isSuccess = false,
                    statusCode = response.StatusCode,
                    message = "Failed to add the docket."
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response
                {
                    isSuccess = false,
                    message = "Internal server error."
                };
            }
        }


        public async Task<IEnumerable<DTODocket>> GetUserDocket()
        {
            try
            {
                 return await httpClient.GetFromJsonAsync<IEnumerable<DTODocket>>("Docket/GetUserDockets");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<DTODocket> GetDocketById(string docketId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<DTODocket>($"Docket/GetById/{docketId}");
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response> Update(DTODocketUpdate docket)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<DTODocketUpdate>($"Docket/Update/{docket.Id}", docket);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new Response
                    {
                        isSuccess = true,
                        statusCode = response.StatusCode,
                        message = "Successfully updated the docket."
                    };
                }

                return new Response
                {
                    isSuccess = false,
                    statusCode = response.StatusCode,
                    message = "Failed to update the docket."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response
                {
                    isSuccess = false,
                    message = "Internal server error."
                };
            }
        }

        public async Task<Response> Delete(string docketId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"Docket/Delete/{docketId}");
                if (response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = true,
                        statusCode = response.StatusCode,
                        message = "Successfully deleted the docket."
                    };
                }
                return new Response
                {
                    isSuccess = false,
                    statusCode = response.StatusCode,
                    message = "Failed to delete the docket"
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response { isSuccess = false, message = "Internal server error" };
            }
        }

        public async Task<IEnumerable<DTODocket>> GetAll()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IEnumerable<DTODocket>>("Docket/GetAllPublic");
            }
            catch
            {
                throw;
            }
        }
    }
}
