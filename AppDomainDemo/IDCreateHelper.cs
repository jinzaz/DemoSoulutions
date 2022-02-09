using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppDomainDemo
{
    public class IDCreateHelper
    {
        public static string GetID(IDType0 tpye0, IDType1 type1)
        {
            string now = DateTime.Now.ToString("yyyyMMdd");
            string value = now + (char)tpye0 + ((short)type1).ToString();
            return value;
        }
        public enum IDType0
        {
            定性 = 'A',
            定量 = 'B',
        }
        public enum IDType1 :short
        {
            tpye1 = 01,
            type2 = 02,
        }
    }
}
