using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Console_Core
{
    public class yield
    {
        static private List<MyClass> _numArray;
        class MyClass
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
        }
        public yield()
        {
            _numArray = new List<MyClass>();
            for (int i = 0; i <=10000000; i++)
            {
                _numArray.Add(new MyClass() { ID =i});
            }

        }
         public void TestMethod()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("yield start ");
            List<MyClass> lists = new List<MyClass>();
            foreach (var item in yieldGetAllEvenNumber())
            {
                lists.Add(item);
            }
            Console.WriteLine("yieldMyClasslist");
            stopwatch.Stop();
            Console.WriteLine("yield stop");
            Console.WriteLine("yield runing time :" + stopwatch.ElapsedMilliseconds + "毫秒");
            stopwatch.Start();
            Console.WriteLine("yieldlist  start ");
            IEnumerable<MyClass> yieldMyClasslist = yieldGetAllEvenNumber();
            stopwatch.Stop();
            Console.WriteLine("yieldlist stop");
            Console.WriteLine("yield runing time :" + stopwatch.ElapsedMilliseconds + "毫秒  listCount:" +yieldMyClasslist.Count());
            lists.Clear();

            Console.WriteLine("----------------------------------------");
            stopwatch.Start();
            Console.WriteLine("start ");
            foreach (var item in GetAllEvenNumber())
            {
                lists.Add(item);
            }
            //IEnumerable<MyClass>  MyClasslist = GetAllEvenNumber();
            stopwatch.Stop();
            Console.WriteLine("stop");
            Console.WriteLine("runing time :" + stopwatch.ElapsedMilliseconds + "毫秒");
            stopwatch.Start();
            Console.WriteLine(" list start ");
            IEnumerable<MyClass> MyClasslist = GetAllEvenNumber();
            stopwatch.Stop();
            Console.WriteLine("list stop");
            Console.WriteLine("list runing time :" + stopwatch.ElapsedMilliseconds + "毫秒  listCount:" + MyClasslist.Count());

        }

        static IEnumerable<MyClass> GetAllEvenNumber()
        {
            List<MyClass> result = new List<MyClass>();
            foreach (var item in _numArray)
            {
                if (item.ID%2==0)
                {
                    item.Name = "xxx" + item.ID;
                    item.Sex = "女";
                    result.Add(item);
                }
            }
            return result;
        }

        static IEnumerable<MyClass> yieldGetAllEvenNumber()
        {
            foreach (var item in _numArray)
            {
                if (item.ID%2==0)
                {
                    item.Name = "xxx" + item.ID;
                    item.Sex = "男";
                    yield return item;
                }
            }
            yield break;
        }

    }
}
