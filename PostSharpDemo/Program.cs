using System;
using System.Threading;

namespace PostSharpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Calc();
            LongRunningCalc();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        [Exception]
        [Log]
        static void Calc()
        {
            throw new DivideByZeroException("A Math Error Occured...");
        }

        [Log,Timing]
        static void LongRunningCalc()
        {
            
            Thread.Sleep(1000);
        }
    }
}
