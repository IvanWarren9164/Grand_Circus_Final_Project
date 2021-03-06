using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models.ViewModels
{
    public class Plants
    {
       //public Datum[] data { get; set; }
        //public Links links { get; set; }
        //public Meta meta { get; set; }

        public int id { get; set; }
        public string common_name { get; set; }
        public string slug { get; set; }
        public string scientific_name { get; set; }
        public int year { get; set; }
        public string bibliography { get; set; }
        public string author { get; set; }
        public string status { get; set; }
        public string rank { get; set; }
        public string family_common_name { get; set; }
        public int genus_id { get; set; }
        public string image_url { get; set; }
        public string[] synonyms { get; set; }
        public string genus { get; set; }
        public string family { get; set; }
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime plantDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime harvestDate { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public string PlantNote { get; set; }
        //public Links1 links { get; set; }
    }

    //public class Links
    //{
    //    public string self { get; set; }
    //    public string first { get; set; }
    //    public string last { get; set; }
    //}

    //public class Meta
    //{
    //    public int total { get; set; }
    //}

    //public class Datum
    //{
    //    public int id { get; set; }
    //    public string common_name { get; set; }
    //    public string slug { get; set; }
    //    public string scientific_name { get; set; }
    //    public int year { get; set; }
    //    public string bibliography { get; set; }
    //    public string author { get; set; }
    //    public string status { get; set; }
    //    public string rank { get; set; }
    //    public string family_common_name { get; set; }
    //    public int genus_id { get; set; }
    //    public string image_url { get; set; }
    //    public string[] synonyms { get; set; }
    //    public string genus { get; set; }
    //    public string family { get; set; }
    //    public Links1 links { get; set; }
    //}

    //public class Links1
    //{
    //    public string self { get; set; }
    //    public string plant { get; set; }
    //    public string genus { get; set; }
    //}


}
