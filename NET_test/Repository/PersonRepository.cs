using Microsoft.EntityFrameworkCore;
using NET_test.Data;
using NET_test.Models;
using NET_test.Repository.IRepository;
using System;
using System.Linq;

namespace NET_test.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        private readonly PersonDbContext _db;
        public PersonRepository(PersonDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Person> EditAsync(Person person)
        {

            _db.People.Update(person);
            await _db.SaveChangesAsync();

            return person;
        }
    }
}
