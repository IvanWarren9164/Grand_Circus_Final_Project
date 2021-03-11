using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models.DALModels
{
    public class GardenerProfileDAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GardenerID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string homeLat { get; set; }
        public string homeLon { get; set; }
        public string usage { get; set; }
        public string favoriteplant { get; set; }
        [ForeignKey("AspNetUsers")]
        public string Id { get; set; }

        public IdentityUser User { get; set; }
    }
}
