using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSSB.Data.Dtos.Auth;
using SSSB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data
{
    public class SSSBRestContext : IdentityDbContext<SSSBUser>
    {
        public SSSBRestContext(DbContextOptions<SSSBRestContext> options) : base(options)
        {
        }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // !!! DON'T STORE THE REAL CONNECTION STRING THE IN PUBLIC REPO !!!
            // Use secret managers provided by your chosen cloud provider
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=RestDemo30");
        }
    }
}
