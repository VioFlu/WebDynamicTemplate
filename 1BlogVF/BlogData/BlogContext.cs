using BlogData.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogData
{
    public class BlogContext : IdentityDbContext<BlogOwner>
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        public DbSet<NavBarEntity> NavBarEntities { get; set; }

        public DbSet<NavBarEntityItem> NavBarEntityItems { get; set; }
    }
}
