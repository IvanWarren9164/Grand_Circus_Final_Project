using Final_Project.Models.APIModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Final_Project.Services
{
    public class TrefleClient
    {
        private readonly HttpClient _client;
        private readonly IOptions<AppConfig> _config;

        public TrefleClient(HttpClient client,IOptions<AppConfig> config)
        {
            _client = client;
            _config = config;
        }

        public async Task<TrefleResponseModel> GetPlants(string keyword)
        {
            var APIKey = _config.Value.TrefleAPIKey;
            return await GetAsync<TrefleResponseModel>($"/api/v1/plants/search?token={APIKey}&q={keyword}");

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
                throw new HttpRequestException("Trefle API returned bad response");
            }
        }
    }
}
