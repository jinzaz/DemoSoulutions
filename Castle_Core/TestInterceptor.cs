using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Castle_Core
{
   public class TestInterceptor : StandardInterceptor
   {
        /// <summary>
        /// 创建代理类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateProxy<T>() where T : class
        {
            ProxyGenerator generator = new ProxyGenerator();
            var testa = generator.CreateClassProxy<T>(new TestInterceptor());
            return testa;
        }

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PreProceed(IInvocation invocation)
        {
            Console.WriteLine(invocation.Method.Name + "执行前，入参：" +string.Join(",",invocation.Arguments));
        }

        /// <summary>
        /// 执行中
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PerformProceed(IInvocation invocation)
        {
            Console.WriteLine(invocation.Method.Name + "执行中");
            try
            {
                base.PerformProceed(invocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine(invocation.Method.Name + "执行后,返回值：" + invocation.ReturnValue);
        }
    }
}
