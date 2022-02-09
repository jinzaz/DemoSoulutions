using System;

namespace Castle_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            TestInterceptor.CreateProxy<TestA>().GetResult("admin");
            Console.ReadKey();
        }
    }
}
