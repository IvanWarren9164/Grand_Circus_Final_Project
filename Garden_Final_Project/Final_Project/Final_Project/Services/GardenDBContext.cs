using Final_Project.Models.DALModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Services
{
    public class GardenDBContext : IdentityDbContext
    {
        public GardenDBContext(DbContextOptions<GardenDBContext> options) : base(options)
        {

        }

        public DbSet<GardenDAL> Garden { get; set; }

        public DbSet<GardenerProfileDAL> Gardener { get; set; }
    }
}
