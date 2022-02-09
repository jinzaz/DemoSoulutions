using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PostSharpDemo
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public class TimingAttribute : PostSharp.Aspects.OnMethodBoundaryAspect
    {
        [NonSerialized]
        Stopwatch _StopWatch;

        public override void OnEntry(MethodExecutionArgs args)
        {
            _StopWatch = Stopwatch.StartNew();
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine(string.Format("[{0}] took {1}ms to execute",new StackTrace().GetFrame(1).GetMethod().Name,_StopWatch.ElapsedMilliseconds));
            base.OnExit(args);  
        }
    }
}
