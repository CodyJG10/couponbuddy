﻿using System;
using System.Collections.Generic;
using System.Text;
using BrochureBuddy.Entities;
using BrochureBuddy.Web.Areas.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static BrochureBuddy.Web.Areas.Identity.User;

namespace BrochureBuddy.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<AdSettings> AdSettings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationReference> UserLocations { get; set; }
        public DbSet<LocationAd> LocationAds { get; set; }
        public DbSet<VendorMedia> VendorMedia { get; set; }
        public DbSet<VendorAnalytics> VendorAnalytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<VendorMedia>()
            //    .HasOne(x => x.Vendor)
            //    .WithMany(x => x.VendorMedia)
            //    .HasForeignKey(x => x.VendorId);

            //modelBuilder.Entity<Vendor>()
            //    .HasMany(x => x.VendorMedia)
            //    .WithOne(x => x.Vendor)
            //    .HasForeignKey(x => x.VendorId); 
        }
    }
}