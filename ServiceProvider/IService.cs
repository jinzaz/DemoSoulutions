using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceProvider
{
    public interface IService
    {
        void SayHello();
    }

    public class ServiceA : IService
    {
        public void SayHello()
        {
            Console.WriteLine("Hello,I'm from A");
        }
    }
    public class ServiceB : IService
    {
        public void SayHello()
        {
            Console.WriteLine("Hello,I'm from B");
        }
    }
    public class ServiceC : IService
    {
        public void SayHello()
        {
            Console.WriteLine("Hello,I'm from C");
        }
    }
    public class ServiceD : IService
    {
        private readonly ServiceA _service;
        public ServiceD(ServiceA service)
        {
            _service = service;
        }
        public void SayHello()
        {
            Console.WriteLine("-------------");
            _service.SayHello();
            Console.WriteLine("Hello,I'm from D");
        }
    }
}
