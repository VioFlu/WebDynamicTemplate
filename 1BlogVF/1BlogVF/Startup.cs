using AutoMapper;
using BlogData;
using BlogData.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace _1BlogVF
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add service for the Identity 
            services.AddIdentity<BlogOwner, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = true;
                cfg.Password.RequireUppercase = true;
                cfg.Password.RequireLowercase = true;
            }
            ).AddEntityFrameworkStores<BlogContext>();

            services.AddAuthentication()
                    .AddCookie()
                    .AddJwtBearer(cfg =>
                    {
                        cfg.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidIssuer = _config["Tokens:Issuer"],
                            ValidAudience = _config["Tokens:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                        };
                    });
            // add service to talk to DB context and configure the database connection
            services.AddDbContext<BlogContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("BlogConnectionString"));
            }

            );

            services.AddDbContext<BlogContext>();

            services.AddAutoMapper();

            services.AddTransient<BlogSeeder>();
            //Register repository to the service
            services.AddScoped<IBlogRepository, BlogRepository>();
            // Add MVC
            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //use the error page
                app.UseExceptionHandler("/error");
            }
            //We need to configure application builder to use static and load our css js files
            app.UseStaticFiles();

            //add authenticatiom
            app.UseAuthentication();
            app.UseNodeModules(env);

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default", "/{controller}/{action}/{id?}",
                            new { controller = "App", Action = "Index" });
            });
        }
    }
}
