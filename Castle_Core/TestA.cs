using System;
using System.Collections.Generic;
using System.Text;

namespace Castle_Core
{
    public class TestA
    {
        public virtual string GetResult(string user)
        {
            string str = DateTime.Now + "TestB-GetResult";
            Console.WriteLine(str);
            return str;
        }

        public virtual string GetResult2()
        {
            string str = DateTime.Now + "TestB-GetResult2";
            Console.WriteLine(str);
            return str;
        }
    }
}
