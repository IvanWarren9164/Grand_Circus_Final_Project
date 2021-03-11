using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models.DALModels
{
    public class GardenDAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GardenID { get; set; }
        public string common_name { get; set; }
        public string scientific_name { get; set; }

        [DataType(DataType.Date)]
        public DateTime plantDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime harvestDate { get; set; }
        public string image_url { get; set; }

        [ForeignKey("AspNetUsers")]
        public string Id { get; set; }

        public IdentityUser User { get; set; }
    }
}
