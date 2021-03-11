using Final_Project.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Final_Project
{
    public class TropicalFruitClient
    {

        private readonly HttpClient _client;

        public TropicalFruitClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<TropicalFruitResponseModel> GetFruit(string keyword)
        {
            return await GetAsync<TropicalFruitResponseModel>($"?search={keyword}");
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
                throw new HttpRequestException("Tropical Fruit API returned bad response");
            }
        }
    }
}
