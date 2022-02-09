using PostSharp.Aspects;
using PostSharp.Patterns.Diagnostics.Audit;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostSharpDemo
{
    
    [Serializable]
    public class LogAttribute : OnMethodBoundaryAspect
    {
        
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Entering [ {0} ] ...",args.Method);
            Console.WriteLine("HHHHHAAAAA");
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("Leaving [ {0} ] ...",args.Method);
            base.OnExit(args);
        }
    }
}
