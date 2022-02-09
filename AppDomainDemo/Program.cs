using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppDomainDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //string CallingDomainName = AppDomain.CurrentDomain.FriendlyName;
            //Console.WriteLine(CallingDomainName);
            //AppDomain ad = AppDomain.CreateDomain("DLL Unload test");
            //ProxyObject obj = (ProxyObject)ad.CreateInstanceFromAndUnwrap(@"UnloadDll.exe","UnloadDll.ProxyObject");
            //obj.LoadAssembly();
            //obj.Invoke("TestDll.Class1","Test","It's a test");
            //AppDomain.Unload(ad);
            //obj = null;
            //Console.ReadLine();
            //IDCreateHelper.GetID(IDCreateHelper.IDType0.定量,IDCreateHelper.IDType1.tpye1);
            //string ss = IDCreateHelper.GetID(IDCreateHelper.IDType0.定量, IDCreateHelper.IDType1.tpye1);
            List<string> list  = new   List<string>();
            try
            {
                list.Select(s => s).ToList().ForEach(x => { 
                    
                });
            }
            catch (Exception ex)
            {

                throw;
            }
            Console.ReadKey();
        }
        
    }
    class ProxyObject : MarshalByRefObject
    {
        Assembly assembly = null;
        string curPath;
        public ProxyObject()
        {
            curPath = System.Environment.CurrentDirectory;

        }
        public void LoadAssembly()
        {
            assembly = Assembly.LoadFile(curPath + @"\TestDLL.dll");
        }
        public bool Invoke(string fullClassName,string methodName,params object[] args)
        {
            if (assembly == null)
                return false;
            Type tp = assembly.GetType(fullClassName);
            if (tp == null)
                return false;
            MethodInfo method = tp.GetMethod(methodName);
            if (method == null)
                return false;
            Object obj = Activator.CreateInstance(tp);
            method.Invoke(obj,args);
            return true;
        }
    }
}
