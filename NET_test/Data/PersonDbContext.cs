using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using NET_test.Models;

namespace NET_test.Data
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
    }
}
