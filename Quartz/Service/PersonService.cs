using QuartzDemo.Repository.IRepository;
using QuartzDemo.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzDemo.Service
{
    public class PersonService :IPersonService
    {
        private IPersonRepository _personRespository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRespository = personRepository;
        }

        public string Eat()
        {
            return _personRespository.Eat();
        }
    }
}
