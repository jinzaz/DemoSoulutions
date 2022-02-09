using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace PostSharpDemo
{
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After,typeof(LogAttribute))]
    public class RunTnTransactionAttribute : OnMethodBoundaryAspect
    {
        [NonSerialized]
        TransactionScope TransactionScope;

        public override void OnEntry(MethodExecutionArgs args)
        {
            
            this.TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            this.TransactionScope.Complete();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            Transaction.Current.Rollback();
            Console.WriteLine("Transaction Was unsuccessful!");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            this.TransactionScope.Dispose();
        }
    }
}
