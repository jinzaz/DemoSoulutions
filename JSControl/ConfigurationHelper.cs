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
    public static class ConfigurationHelper
    {

        public static void AddJS(string Orgcode,string WorkPath)
        {
            try
            {
                //读取xml文件路径
                string path = Path.GetFullPath("../..") + @"\TemplateXml\JSTemplate.xml";
                var data = new XmlOperate(path).GetData("/FileList/File");
                for (int i = 0; i < data.Count; i++)
                {
                    try
                    {
                        var datas = data.Item(i).SelectNodes("FileTemplate").Item(0).ChildNodes.Item(1);
                        datas.InnerText = Orgcode;
                        string content = data.Item(i).SelectNodes("FileTemplate").Item(0).InnerText;
                        string filepath = WorkPath + data.Item(i).SelectNodes("FilePath").Item(0).InnerText;
                        string oldContent = System.IO.File.ReadAllText(filepath);
                        if (oldContent.IndexOf(content, StringComparison.Ordinal) > -1)
                        {
                            Console.WriteLine($"禁止重复加入：{filepath}");
                            continue;
                        }
                        System.IO.File.WriteAllText(filepath, oldContent + content);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public static void AddHtml(string Orgcode, string WorkPath)
        {
            try
            {
                //读取xml文件路径
                string path = Path.GetFullPath("../..") + @"\TemplateXml\HtmlTemplate.xml";
                var data = new XmlOperate(path).GetData("/FileList/File");
                for (int i = 0; i < data.Count; i++)
                {
                    try
                    {
                        var datas = data.Item(i).SelectNodes("FileTemplate").Item(0).ChildNodes.Item(0);
                        datas.InnerText = $"'{Orgcode}'";
                        string content = data.Item(i).SelectNodes("FileTemplate").Item(0).InnerText;
                        string filepath = WorkPath + data.Item(i).SelectNodes("FilePath").Item(0).InnerText;
                        string oldContent = System.IO.File.ReadAllText(filepath);
                        StringBuilder oldContentBuild = new System.Text.StringBuilder(oldContent);
                        int startIndex = oldContent.IndexOf("orgss");
                        if (startIndex > -1)
                        {
                            int endindex = oldContent.IndexOf("]", startIndex);
                            if (oldContent[endindex - 1] == ',')
                            {
                                oldContentBuild.Insert(endindex, content);
                            }
                            else
                            {
                                oldContentBuild.Insert(endindex, $",{content}");
                            }

                        }
                        System.IO.File.WriteAllText(filepath, oldContentBuild.ToString(), Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public static void UpdateCsCode(string Orgcode, string WorkPath)
        {
            try
            {
                //读取xml文件路径
                string path = Path.GetFullPath("../..") + @"\TemplateXml\HtmlTemplate.xml";
                var data = new XmlOperate(path).GetData("/FileList/File");
                for (int i = 0; i < data.Count; i++)
                {
                    var datas = data.Item(i).SelectNodes("FileTemplate").Item(0).ChildNodes.Item(0);
                    datas.InnerText = $"'{Orgcode}'";
                    string content = data.Item(i).SelectNodes("FileTemplate").Item(0).InnerText;
                    string filepath = WorkPath + data.Item(i).SelectNodes("FilePath").Item(0).InnerText;
                    string oldContent = System.IO.File.ReadAllText(filepath);
                    StringBuilder oldContentBuild = new System.Text.StringBuilder(oldContent);
                    int startIndex = oldContent.IndexOf("orgss");
                    if (startIndex > -1)
                    {
                        int endindex = oldContent.IndexOf("]", startIndex);
                        if (oldContent[endindex - 1] == ',')
                        {
                            oldContentBuild.Insert(endindex, content);
                        }
                        else
                        {
                            oldContentBuild.Insert(endindex, $",{content}");
                        }

                    }
                    System.IO.File.WriteAllText(filepath, oldContentBuild.ToString(),Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static void CreateFile(string Orgcode, string WorkPath)
        {
            try
            {
                //读取xml文件路径
                string path = Path.GetFullPath("../..") + @"\TemplateXml\HtmlTemplate.xml";
                var data = new XmlOperate(path).GetData("/FileList/File");
                for (int i = 0; i < data.Count; i++)
                {
                    var datas = data.Item(i).SelectNodes("FileTemplate").Item(0).ChildNodes.Item(0);
                    datas.InnerText = $"'{Orgcode}'";
                    string content = data.Item(i).SelectNodes("FileTemplate").Item(0).InnerText;
                    string filepath = WorkPath + data.Item(i).SelectNodes("FilePath").Item(0).InnerText;
                    string oldContent = System.IO.File.ReadAllText(filepath);
                    StringBuilder oldContentBuild = new System.Text.StringBuilder(oldContent);
                    int startIndex = oldContent.IndexOf("orgss");
                    if (startIndex > -1)
                    {
                        int endindex = oldContent.IndexOf("]", startIndex);
                        if (oldContent[endindex - 1] == ',')
                        {
                            oldContentBuild.Insert(endindex, content);
                        }
                        else
                        {
                            oldContentBuild.Insert(endindex, $",{content}");
                        }

                    }
                    System.IO.File.WriteAllText(filepath, oldContentBuild.ToString());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">参数体</param>
        /// <param name="sCode">JavaScript代码的字符串</param>
        /// <returns></returns>
        private static string ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }
    }
}
