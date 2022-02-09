using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSControl
{
    class Program
    {
        static string orgcode = "CRH";
        static string workpath = @"D:\CASystem\CRH\";
        static void Main(string[] args)
        {

            ConfigurationHelper.AddJS(orgcode, workpath);
            ConfigurationHelper.AddHtml(orgcode, workpath);
            //List<student> student = new List<student>() { new student { data1 = "1", data2 = "2", data3 = "3" }, new student { data1 = "1", data2 = "2", data3 = "3" }, new student { data1 = "1", data2 = "2", data3 = "3" } };
            //List<object> list = student.ConvertAll(s => (object)s);
            //var data = CloneObject(list, student.GetType());
            Console.WriteLine("加入完成");
            Console.ReadKey();
        }
        public class student
        {
            public string data1 { get; set; }
            public string data2 { get; set; }
            public string data3 { get; set; }
        }

        public static object ConvertToObject(object obj, Type type)
        {

            if (type == null) return obj;
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType())) // 如果待转换对象的类型与目标类型兼容，则无需转换
            {
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum) // 如果待转换的对象的基类型为枚举
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString())) // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值
                {
                    return null;
                }
                else
                {
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
                }
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return converter.ConvertFrom(obj);
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ConvertToObject(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }

        public static object CloneObject(object objSource ,Type type)
        {
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(type);  //创建目标对象

            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);  //获取所有属性

            foreach (PropertyInfo property in propertyInfo)  //遍历属性
            {
                if (property.CanWrite)
                {
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))  //直接赋属性值
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, CloneObject(objPropertyValue, property.PropertyType), null);   //引用类型  递归
                        }
                    }
                }
            }
            return objTarget;
        }
    }
}
