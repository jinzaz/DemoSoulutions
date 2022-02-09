using System;

namespace ServiceProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceLocator locator =  ServiceLocator.GetInstance();
            //locator.AddService<ServiceA>();
            //locator.AddService<ServiceB>();
            //locator.AddService<ServiceC>();

            //locator.GetService<ServiceA>().SayHello();
            //locator.GetService<ServiceB>().SayHello();
            //locator.GetService<ServiceC>().SayHello();

            //locator.AddService(() => new ServiceD(locator.GetService<ServiceA>()));
            //locator.GetService<ServiceD>().SayHello();
            locator.AddService<ServiceD>();
            locator.AddService<ServiceA>();
            locator.GetService<ServiceD>().SayHello();
            locator.GetService<ServiceA>().SayHello();
            Console.ReadKey();
        }
    }
}
