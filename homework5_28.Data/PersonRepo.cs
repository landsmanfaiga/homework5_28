using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faker;

namespace homework5_28.Data
{
    public class PersonRepo
    {
        private readonly string _connectionString;
        public PersonRepo(string connectionString)
        {
             _connectionString = connectionString;
        }

        public List<Person> GetAll()
        {
            var context = new PeopleDataContext(_connectionString);
            return context.People.ToList();
        }

        public void DeleteAll()
        {
            var context = new PeopleDataContext(_connectionString);
            context.People.RemoveRange(context.People);
            context.SaveChanges();
        }

        public void UploadPeople(List<Person> people)
        {            
            var context = new PeopleDataContext(_connectionString);
            context.People.AddRange(people);
            context.SaveChanges();
        }
      
    }

  
}
