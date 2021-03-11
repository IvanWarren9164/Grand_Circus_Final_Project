using Final_Project.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Final_Project.Services
{
    public class GeoIPClient
    {
        private readonly HttpClient _client;

        public GeoIPClient(HttpClient client)

            
        {
            _client = client;
        }

        public async Task<GeoIPResponseModel> GetLocation()
        {
            return await GetAsync<GeoIPResponseModel>("/json/");
        }


        private async Task<T> GetAsync<T>(string endPoint)
        {
            var response = await _client.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer.DeserializeAsync<T>(content);

                return model;
            }
            else
            {
                throw new HttpRequestException("FreeGeoIP API returned bad response");
            }
        }
    }
}
