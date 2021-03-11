using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models.ViewModels.GardenControllerViewModels
{
    public class PlantInfoUpdateViewModel
    {
        public string common_name { get; set; }
        public string scientific_name { get; set; }
        public string image_url { get; set; }

        [DataType(DataType.Date)]
        public DateTime plantDate { get { return this.dateCreated.HasValue ? this.dateCreated.Value : DateTime.Now; } set { } }

        [DataType(DataType.Date)]
        public DateTime harvestDate { get { return this.dateCreated.HasValue ? this.dateCreated.Value : DateTime.Now; } set { } }

        public List<Plants> Results { get; set; }

        private DateTime? dateCreated = null;
    }
}
