using Crowdfund.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Data
{
    public class CrowdfundDbContext : DbContext
    {

        //private readonly string connectionString2 =
        //    "Server =localhost; " +
        //    "Database =master; " +
        //    "User Id =sa; " +
        //    "Password =admin!@#123;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Server=localhost; Database=crowdfund; User Id=sa; Password=admin!@#123;");
        //optionsbuilder.usesqlserver(this.connectionstring2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>()
                .ToTable("User");

            modelBuilder
                .Entity<Project>()
                .ToTable("Project");

            modelBuilder
                .Entity<Package>()
                .ToTable("Package");


            modelBuilder
                .Entity<PurchasedPackage>()
                .ToTable("PurchasedPackage");

            modelBuilder
                .Entity<PurchasedPackage>()
                .HasKey(pp => new { pp.UserId, pp.PackageId });

            modelBuilder
                .Entity<PurchasedPackage>()
                .HasOne(pp => pp.User)
                .WithMany(u => u.PurchasedPackages).HasForeignKey(pp => pp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<ProjectMedia>()
                .ToTable("ProjectMedia");

            modelBuilder
                .Entity<ProjectMedia>()
                .HasKey(pm => new { pm.ProjectId, pm.MediaUrl });

            modelBuilder
                .Entity<ProjectStatusUpdate>()
                .ToTable("ProjectStatusUpdate");

            

        }
    }
}
