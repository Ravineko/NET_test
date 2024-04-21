using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using NET_test.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NET_test.Data
{
    public class PersonDbContext :IdentityDbContext<ApplicationUser>
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
