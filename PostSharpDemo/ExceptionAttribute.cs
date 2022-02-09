using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostSharpDemo
{
    [Serializable]
    public  class ExceptionAttribute : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine(string.Format("Exception in :[{0}] , Message:[{1}]",args.Method,args.Exception.Message));
            args.FlowBehavior = FlowBehavior.Continue;
            base.OnException(args);
        }
    }
}
