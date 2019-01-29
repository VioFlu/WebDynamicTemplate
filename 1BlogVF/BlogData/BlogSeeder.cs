using BlogData.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BlogData
{
    public class BlogSeeder
    {
        public readonly BlogContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<BlogOwner> _userManager;

        public BlogSeeder(BlogContext ctx,
                          IHostingEnvironment hosting,
                          UserManager<BlogOwner> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }
        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            BlogOwner user = await _userManager.FindByEmailAsync("vf@mail.com");

            if (user == null)
            {
                user = new BlogOwner()
                {
                    FirstName = "Vioexample",
                    LastName = "VLastName",
                    Email = "vf@mail.com",
                    UserName = "vex"
                };
                var result = await _userManager.CreateAsync(user, "Passw0rd_1");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

        }

    }
}
