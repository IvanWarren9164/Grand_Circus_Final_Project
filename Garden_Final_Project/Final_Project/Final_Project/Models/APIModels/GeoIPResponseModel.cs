using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models.APIModels
{
    public class GeoIPResponseModel
    {

       
            public string ip { get; set; }
            public string country_code { get; set; }
            public string country_name { get; set; }
            public string region_code { get; set; }
            public string region_name { get; set; }
            public string city { get; set; }
            public string zip_code { get; set; }
            public string time_zone { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int metro_code { get; set; }
        

    }
}
