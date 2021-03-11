using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models.ViewModels.GardenControllerViewModels
{
    public class SearchResultViewModel
    {
        public string common_name { get; set; }
        public string scientific_name { get; set; }
        public string image_url { get; set; }
        public string plantDate { get; set; }
        public string harvestDate { get; set; }
        public IEnumerable<SearchResultViewModel> plants { get; set; }

        public List<Plants> results { get; set; }

        public string search { get; set; }

        public bool userLoggedIn { get; set; }


    }
}
