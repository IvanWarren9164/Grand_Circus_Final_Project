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
    public class WeatherClient
    {
        private readonly HttpClient _client;
        private readonly IOptions<AppConfig> _config;

        public WeatherClient(HttpClient client,IOptions<AppConfig> config)
        {
            _client = client;
            _config = config;
        }

        //0bb6a2058b1b7e84cad84945812f22d6

        public async Task<WeatherResponseModel> GetWeather(double lat, double lon)
        {
            var APIKey = _config.Value.WeatherAPIKey;
            return await GetAsync<WeatherResponseModel>($"/data/2.5/onecall?lat={lat}&lon={lon}&units=imperial&appid={APIKey}");

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
                throw new HttpRequestException("Weather API returned bad response");
            }
        }
    }
}
