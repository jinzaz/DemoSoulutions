using QuartzDemo.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QuartzDemo.Util.common
{
    public class MyValueProcessor : IValueProcessor
    {
        private readonly Dictionary<short, Func<decimal, decimal>> _dic;
        public MyValueProcessor ()
        {
            _dic = new Dictionary<short, Func<decimal, decimal>>();
            foreach (int item in Enum.GetValues(typeof(PolicyEnum)))
            {
                Type type = PolicyEnum.Bazhe.GetType();
                string fold = Enum.GetName(typeof(PolicyEnum),item);
                FieldInfo fieldInfo = typeof(PolicyEnum).GetField(fold);
                DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                _dic.Add((short)item,m => m * decimal.Parse(attr.Description));
            }
        }
        public decimal DaZhe(short policy, decimal orginPrice)
        {
            if (_dic.ContainsKey(policy))
            {
                return _dic[policy].Invoke(orginPrice);
            }
            return orginPrice / 2;
        }
    }

    public enum PolicyEnum
    { 
        [Description("0.5")]
        Wuzhe = 0,
        [Description("0.6")]
        Liuzhe = 1,
        [Description("0.7")]
        Qizhe = 2,
        [Description("0.8")]
        Bazhe = 3,
        [Description("0.9")]
        Jiuzhe = 4
    }
}
