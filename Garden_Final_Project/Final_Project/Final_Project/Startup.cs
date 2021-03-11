using Final_Project.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<GardenDBContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));


            services.AddDefaultIdentity<IdentityUser>(options =>

                    options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<GardenDBContext>();

            services.Configure<AppConfig>(options => Configuration.GetSection("Secrets").Bind(options));
            //var trefleAPI = Configuration.GetSection("Secrets:TrefleAPIKey").Value;
            //var weatherAPI = Configuration.GetSection("Secrets:WeatherAPIKey").Value;

            services.AddHttpClient<TropicalFruitClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("http://tropicalfruitandveg.com/api/tfvjsonapi.php");
            });

            services.AddHttpClient<TrefleClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://trefle.io");
            });

            services.AddHttpClient<WeatherClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.openweathermap.org");
            });

            services.AddHttpClient<GeoIPClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://freegeoip.app");
            });


            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
