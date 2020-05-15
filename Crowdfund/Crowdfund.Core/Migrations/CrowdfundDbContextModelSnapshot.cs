﻿// <auto-generated />
using System;
using Crowdfund.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Crowdfund.Core.Migrations
{
    [DbContext(typeof(CrowdfundDbContext))]
    partial class CrowdfundDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Crowdfund.Core.Model.Package", b =>
                {
                    b.Property<int>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("PackageId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Package");
                });

            modelBuilder.Entity("Crowdfund.Core.Model.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FinancialGoal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FinancialProgress")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Crowdfund.Core.Model.ProjectMedia", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("MediaUrl")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProjectId", "MediaUrl");

                    b.ToTable("ProjectMedia");
                });

            modelBuilder.Entity("Crowdfund.Core.Model.ProjectStatusUpdate", b =>
                {
                    b.Property<int>("ProjectStatusUpdateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectStatusUpdateId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectStatusUpdate");
                });

            modelBuilder.Entity("Crowdfund.Core.Model.PurchasedPackage", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "PackageId");

                    b.HasIndex("PackageId");

                    b.ToTable("PurchasedPackage");
                });

            modelBuilder.Entity("Crowdfund.Core.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsProjectCreator")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Crowdfund.Core.Model.Package", b =>
                {
                    b.HasOne("Crowdfund.Core.Model.Project", "Project")
                        .WithMany("Packages")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Crowdfund.Core.Model.Project", b =>
                {
                    b.HasOne("Crowdfund.Core.Model.User", "User")
                        .WithMany("Projects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Crowdfund.Core.Model.ProjectMedia", b =>
                {
                    b.HasOne("Crowdfund.Core.Model.Project", "Project")
                        .WithMany("Media")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Crowdfund.Core.Model.ProjectStatusUpdate", b =>
                {
                    b.HasOne("Crowdfund.Core.Model.Project", "Project")
                        .WithMany("StatusUpdates")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Crowdfund.Core.Model.PurchasedPackage", b =>
                {
                    b.HasOne("Crowdfund.Core.Model.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Crowdfund.Core.Model.User", "User")
                        .WithMany("PurchasedPackages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
