using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace NET_test.Models
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
    }
}
