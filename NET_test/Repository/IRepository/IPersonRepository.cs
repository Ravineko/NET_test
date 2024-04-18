using NET_test.Models;

namespace NET_test.Repository.IRepository
{
    public interface IPersonRepository:IRepository<Person>
    {
        Task<Person> EditAsync(Person person);
    }
}
