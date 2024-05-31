using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ST10058357_PROG7311_POE2.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // If you have existing tables, configure them here
         builder.Entity<User>().ToTable("User");
    }

    public DbSet<ST10058357_PROG7311_POE2.Models.User> User { get; set; } = default!;

public DbSet<ST10058357_PROG7311_POE2.Models.Product> Product { get; set; } = default!;

public DbSet<ST10058357_PROG7311_POE2.Models.ProductCategory> ProductCategory { get; set; } = default!;

public DbSet<ST10058357_PROG7311_POE2.Models.ProductSubCategory> ProductSubCategory { get; set; } = default!;
    }
