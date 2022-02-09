using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JSControl
{
    public class XmlOperate
    {

        protected string strXmlFile;
        protected XmlDocument objXmlDoc = new XmlDocument();
        

        public XmlOperate(string XmlFile)
        {
            // 
            // TODO: 在这里加入建构函式的程序代码 
            // 
            try
            {

                objXmlDoc.Load(XmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            strXmlFile = XmlFile;
        }
        public XmlNodeList GetData(string XmlPathNode)
        {
            
            return objXmlDoc.SelectNodes(XmlPathNode);
        }
    }
}
