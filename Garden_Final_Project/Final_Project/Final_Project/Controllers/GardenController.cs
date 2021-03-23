using Final_Project.Models.DALModels;
using Final_Project.Models.ViewModels;
using Final_Project.Models.ViewModels.GardenControllerViewModels;
using Final_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Controllers
{
    public class GardenController : Controller
    {
        private readonly GardenDBContext _gardenDBContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GeoIPClient _geoIPClient;
        private readonly TrefleClient _trefleClient;
        private readonly WeatherClient _weatherClient;

        public GardenController(GardenDBContext gardenDBContext,
            UserManager<IdentityUser> userManager,
             GeoIPClient geoIPClient,
            TrefleClient trefleClient,
            WeatherClient weatherClient
            )
        {
            _gardenDBContext = gardenDBContext;
            _userManager = userManager;
            _geoIPClient = geoIPClient;
            _trefleClient = trefleClient;
            _weatherClient = weatherClient;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemberProfileForm()
        {
            return View();
        }

        public async Task<IActionResult> MakeFavorite(string search, int index)
        {
            var gardener = await _userManager.GetUserAsync(User);
            var memberAlreadyExist = new GardenerProfileDAL();
            memberAlreadyExist = _gardenDBContext.Gardener.Where(user => user.Id == gardener.Id).FirstOrDefault();
            var response = await _trefleClient.GetPlants(search);
            memberAlreadyExist.favoriteplant = response.data[index].common_name;
            _gardenDBContext.SaveChanges();

            var viewModel = new MemberProfileViewModel();
            viewModel.ImgFavPlant = response.data[index].image_url;
            viewModel.TreflePlantName = response.data[index].common_name;
            viewModel.Usage = memberAlreadyExist.usage;
            viewModel.UserHaveFave = true;
            viewModel.InDatabase = true;
            viewModel.FirstName = memberAlreadyExist.firstName;
            viewModel.FavoritePlant = viewModel.TreflePlantName;

            return View("MemberProfile", viewModel);
        }

        public async Task<IActionResult> StoreGardenerData(MemberProfileFormViewModel model)
        {
            var gardener = await _userManager.GetUserAsync(User);
            var profileDAL = new GardenerProfileDAL();
            var response = await _geoIPClient.GetLocation();
            var memberAlreadyExist = new GardenerProfileDAL();
            memberAlreadyExist = _gardenDBContext.Gardener.Where(user => user.Id == gardener.Id).FirstOrDefault();

            bool memberExists = false;
            var viewModel = new MemberProfileFormViewModel();

            if (memberAlreadyExist != null)
            {
                memberExists = true;
            }

            if (!memberExists)
            {
                if (Validation.CheckName(model.FirstName)
                && Validation.CheckName(model.LastName))
                {
                    profileDAL.Id = gardener.Id;
                    profileDAL.firstName = model.FirstName;
                    profileDAL.lastName = model.LastName;
                    profileDAL.usage = model.Usage;

                    profileDAL.homeLat = response.latitude.ToString();
                    profileDAL.homeLon = response.longitude.ToString();

                    _gardenDBContext.Gardener.Add(profileDAL);
                    _gardenDBContext.SaveChanges();

                    return View("MemberPortal");
                }
                else
                {
                    return View("MemberProfileForm");
                }
            }
            else
            {
                if (Validation.CheckName(model.FirstName)
                && Validation.CheckName(model.LastName))
                {
                    memberAlreadyExist.firstName = model.FirstName;
                    memberAlreadyExist.lastName = model.LastName;

                    memberAlreadyExist.usage = model.Usage;

                    _gardenDBContext.SaveChanges();

                    return View("MemberPortal");
                }
                return View("MemberProfileForm");
            }
        }

        public async Task<IActionResult> MemberProfile()
        {
            var gardener = await _userManager.GetUserAsync(User);
            var viewModel = new MemberProfileViewModel();
            var memberAlreadyExist = new GardenerProfileDAL();
            memberAlreadyExist = _gardenDBContext.Gardener.Where(user => user.Id == gardener.Id).FirstOrDefault();

            if (memberAlreadyExist != null)
            {
                viewModel.InDatabase = true;

                var gardenerProfile = _gardenDBContext.Gardener.Where(member => member.Id == gardener.Id).FirstOrDefault();

                viewModel.FirstName = gardenerProfile.firstName;
                viewModel.Usage = gardenerProfile.usage;

                if (gardenerProfile.favoriteplant != null)
                {
                    viewModel.FavoritePlant = gardenerProfile.favoriteplant;
                    viewModel.UserHaveFave = true;
                    var response = await _trefleClient.GetPlants(viewModel.FavoritePlant);

                    if (response != null)
                    {
                        viewModel.ImgFavPlant = response.data[0].image_url;
                        viewModel.TreflePlantName = response.data[0].common_name;
                    }
                    else
                    {
                        viewModel.ImgFavPlant = "https://dummyimage.com/100/fff/0011ff.png&text=Image+Not+Found";
                        viewModel.TreflePlantName = "Looks like your favorite plant cannot be found";
                    }
                }
                else
                {
                    viewModel.UserHaveFave = false;
                }
            }
            else
            {
                viewModel.InDatabase = false;
            }
            return View(viewModel);
        }

        public IActionResult Weather()
        {
            return View();
        }

        public async Task<IActionResult> HomeWeather()
        {
            var response = await _geoIPClient.GetLocation();
            var weather = await _weatherClient.GetWeather(response.latitude, response.longitude);

            var viewModel = new HomeWeatherViewModel();

            viewModel.Temperature = weather.current.temp;
            var icon = weather.current.weather[0].icon;
            viewModel.ImgUrl = $"http://openweathermap.org/img/wn/{icon}@2x.png";

            return View(viewModel);
        }

        public async Task<IActionResult> VacationWeather()
        {
            var gardener = await _userManager.GetUserAsync(User);
            var viewModel = new VacationWeatherViewModel();

            var gardenerProfile = _gardenDBContext.Gardener.Where(member => member.Id == gardener.Id).FirstOrDefault();

            if (gardenerProfile != null)
            {
                viewModel.InDatabase = true;

                double.TryParse(gardenerProfile.homeLat, out double latitude);
                double.TryParse(gardenerProfile.homeLon, out double longitude);


                var weather = await _weatherClient.GetWeather(latitude, longitude);

                viewModel.Temperature = weather.current.temp;
                var icon = weather.current.weather[0].icon;
                viewModel.ImgUrl = $"http://openweathermap.org/img/wn/{icon}@2x.png";
            }
            else
            {
                viewModel.InDatabase = false;
            }

          

            return View(viewModel);
        }
        public async Task<IActionResult> MemberGarden()
        {
            var gardener = await _userManager.GetUserAsync(User);
            var viewModel = new MemberGardenViewModel();
            
            var gardenerView = _gardenDBContext.Garden.Where(member => member.Id == gardener.Id).ToList();
            viewModel.plants = new List<Plants>();

            viewModel.plants = gardenerView
                .Select(plant => new Plants()
                {
                    common_name = plant.common_name,
                    scientific_name = plant.scientific_name,
                    plantDate = plant.plantDate,
                    harvestDate = plant.harvestDate,
                    image_url = plant.image_url,
                    Id = plant.GardenID,
                    Quantity = plant.Quantity,
                    Location = plant.Location,
                    PlantNote = plant.PlantNote
                })
                .ToList();
            if(viewModel.plants.Count == 0)
            {
                return View("EmptyGarden");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> UpdateNote(int Id,string PlantNote)
        {
            var plantDB = new GardenDAL();
            var noteupdate = new UpdateNoteViewModel();
            var gardener = await _userManager.GetUserAsync(User);
            var viewModel = new MemberGardenViewModel();


            plantDB = _gardenDBContext.Garden
                .Where(garden => garden.GardenID == Id)
                .FirstOrDefault();

            noteupdate.PlantNote = PlantNote;

            plantDB.PlantNote = noteupdate.PlantNote;
            _gardenDBContext.SaveChanges();

            var gardenerView = _gardenDBContext.Garden.Where(member => member.Id == gardener.Id).ToList();
            viewModel.garden = new List<MemberGardenViewModel>();

            viewModel.garden = gardenerView
            .Select(plant => new MemberGardenViewModel()
            {
                common_name = plant.common_name,
                scientific_name = plant.scientific_name,
                plantDate = plant.plantDate,
                harvestDate = plant.harvestDate,
                image_url = plant.image_url,
                Id = plant.GardenID,
                Quantity = plant.Quantity,
                Location = plant.Location,
                PlantNote = plant.PlantNote
            })
            .ToList();

            return View("MemberGarden", viewModel);
        }


        public async Task<IActionResult> SearchResult(string search)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var searchresult = await _trefleClient.GetPlants(search);
            var viewModel = new SearchResultViewModel();
            viewModel.results = new List<Plants>();
            viewModel.results = searchresult.data
                .Select(plant => new Plants()
                {
                    common_name = plant.common_name,
                    scientific_name = plant.scientific_name,
                    image_url = plant.image_url
                })
                .ToList();

            viewModel.search = search;

            if (loggedInUser != null)
            {
                viewModel.userLoggedIn = true;
            }
            else
            {
                viewModel.userLoggedIn = false;
            }

            if(viewModel.results.Count == 0)
            {
                return View("NoResults");
            }
            return View(viewModel);
        }
        public async Task<IActionResult> AddPlant(PlantInfoUpdateViewModel model)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var plantDB = new GardenDAL();

            plantDB.common_name = model.common_name;
            plantDB.scientific_name = model.scientific_name;
            plantDB.image_url = model.image_url;
            plantDB.harvestDate = model.harvestDate;
            plantDB.plantDate = model.plantDate;
            plantDB.Id = loggedInUser.Id;
            plantDB.Location = model.Location;
            plantDB.Quantity = model.Quantity;
       
            _gardenDBContext.Garden.Add(plantDB);
            _gardenDBContext.SaveChanges();

            var gardenerView = _gardenDBContext.Garden.Where(member => member.Id == loggedInUser.Id).ToList();

            var viewGarden = new MemberGardenViewModel();
            viewGarden.garden = new List<MemberGardenViewModel>();
            viewGarden.garden = gardenerView.Select(plant => new MemberGardenViewModel
            {
                common_name = plant.common_name,
                scientific_name = plant.scientific_name,
                harvestDate = plant.harvestDate,
                plantDate = plant.plantDate,
                Id = plant.GardenID,
                Location = plant.Location,
                Quantity = plant.Quantity
            }).ToList();

            return View("MemberGarden", viewGarden);
        }

        public async Task<IActionResult> DeletePlant(int Id)
        {
            var plantDB = new GardenDAL();

            plantDB = _gardenDBContext.Garden
                .Where(garden => garden.GardenID == Id)
                .FirstOrDefault();

            _gardenDBContext.Garden.Remove(plantDB);
            _gardenDBContext.SaveChanges();

            var loggedInUser = await _userManager.GetUserAsync(User);
            var gardenerView = _gardenDBContext.Garden.Where(member => member.Id == loggedInUser.Id).ToList();

            var viewGarden = new MemberGardenViewModel();
            viewGarden.garden = new List<MemberGardenViewModel>();
            viewGarden.garden = gardenerView.Select(plant => new MemberGardenViewModel
            {
                common_name = plant.common_name,
                scientific_name = plant.scientific_name,
                harvestDate = plant.harvestDate,
                plantDate = plant.plantDate,
                Id = plant.GardenID

            }).ToList();

            return View("MemberGarden", viewGarden);
        }

        public async Task<IActionResult> PlantInfoUpdate(string search, int index)
        {
            var searchResult = await _trefleClient.GetPlants(search);

            var viewModel = new PlantInfoUpdateViewModel();
            viewModel.Results = searchResult.data.Select(plant => new Plants()
            {
                common_name = plant.common_name,
                scientific_name = plant.scientific_name,
                image_url = plant.image_url
            }).ToList();

            var plant = new Plants();
            plant = viewModel.Results[index];

            viewModel.common_name = plant.common_name;
            viewModel.scientific_name = plant.scientific_name;
            viewModel.image_url = plant.image_url;

            return View(viewModel);
        }

        public IActionResult MemberPortal()
        {
            return View();
        }
    }
}
