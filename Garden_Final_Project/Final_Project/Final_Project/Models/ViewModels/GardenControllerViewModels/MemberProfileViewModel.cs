using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models.ViewModels.GardenControllerViewModels
{
    public class MemberProfileViewModel
    {
        public string FirstName { get; set; }
        public string Usage { get; set; }
        public string FavoritePlant { get; set; }

        public string ImgFavPlant { get; set; }

        public string TreflePlantName { get; set; }
        public bool InDatabase { get; set; }

        public bool UserHaveFave { get; set; }
    }
}
