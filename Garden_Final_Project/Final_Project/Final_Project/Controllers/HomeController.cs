using Final_Project.Models;
using Final_Project.Models.ViewModels;
using Final_Project.Models.ViewModels.HomeControllerViewModels;
using Final_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoIPClient _geoIPClient;
        private readonly ILogger<HomeController> _logger;
        private readonly TrefleClient _trefleClient;
        private readonly WeatherClient _weatherClient;
        private readonly TropicalFruitClient _tropicalFruitClient;

        public HomeController(ILogger<HomeController> logger,
            GeoIPClient geoIPClient,
            TrefleClient trefleClient,
            TropicalFruitClient tropicalFruitClient,
            WeatherClient weatherClient)
        {
            _geoIPClient = geoIPClient;
            _logger = logger;
            _trefleClient = trefleClient;
            _weatherClient = weatherClient;
            _tropicalFruitClient = tropicalFruitClient;
        }

        public async Task<IActionResult> Index()
        {
            var SearchString = "Rosa pendulina";

            var viewModel = new IndexViewModel();
            var response = await _trefleClient.GetPlants(SearchString);
            viewModel.Plants = response.data
                .Select(response => new PlantVM() { Name = response.common_name, ScientificName = response.scientific_name, ImageURL = response.image_url, PlantID = response.id })
                .ToList();

            return View(viewModel);
        }

        public IActionResult About()
        {

            return View();
        }

        //public async Task<IActionResult> TestAPIs(string SearchString)
        //{

        //    var response = await _geoIPClient.GetLocation();

        //    var viewModel = new TestViewModel();

        //    viewModel.lon = response.longitude;
        //    viewModel.lat = response.latitude;

        //    var weatherResponse = await _weatherClient.GetWeather(response.latitude, response.longitude);

        //    viewModel.temp = weatherResponse.current.temp;

        //    var plantResults = await _trefleClient.GetPlants(SearchString);

        //    viewModel.plantSearch = plantResults.data
        //        .Select(result => new Plants()
        //        {id = result.id,
        //        common_name = result.common_name
        //        })
        //        .ToList();


        //    return View("Test", viewModel);
        //}
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
