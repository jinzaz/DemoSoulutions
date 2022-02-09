using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceProvider
{
    public interface IServiceLocator
    {
        void AddService<T>();
        void AddService<T>(Func<T> Implementation);
        T GetService<T>();
    }
}
