using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LaerlingProject.Models;

namespace LaerlingProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<City> Citys { get; set; }
        public DbSet<Fag> Fag { get; set; }
        public DbSet<LaerlingProfil> LaerlingProfil { get; set; }
        public DbSet<Job> Job { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LaerlingCity>().HasKey(k => new { k.LaerlingID, k.CityID });

            builder.Entity<LaerlingCity>()
                .HasOne(lc => lc.City)
                .WithMany(c => c.LaerlingCity)
                .HasForeignKey(k => k.CityID);

            builder.Entity<LaerlingCity>()
                .HasOne(l => l.Laerling)
                .WithMany(lc => lc.LaerlingCity)
                .HasForeignKey(k => k.LaerlingID); 


            builder.Entity<LaerlingProfil>()
                .HasOne(u => u.ApplicationUser)
                .WithOne(l => l.LaerlingProfil)
                .HasForeignKey<ApplicationUser>(k => k.LaerlingProfilID);

        }
    }
}
