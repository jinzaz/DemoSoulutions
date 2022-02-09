using QuartzDemo.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzDemo.Repository
{
    public class PersonRepository :IPersonRepository
    {
        public string Eat()
        {
            return "吃饭";
        }
    }
}
