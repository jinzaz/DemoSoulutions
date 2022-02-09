using System;
using System.Collections.Generic;

namespace Console_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            //yield y = new yield();
            //y.TestMethod();

            iteration iter = new iteration().GetFiles("E:\\开源\\HzyAdmin-Mvc5");
            foreach (var item in iter)
            {
                Console.WriteLine(item);
            }
            IEnumerable<string> files = iter.yieldGetFiles("D:\\站点部");
            foreach (String item in iter.yieldGetFiles("D:\\站点部署"))
            {
                Console.WriteLine(item);
            }
            Console.Read();
        }
    }
}
