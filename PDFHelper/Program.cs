using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PDFHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid guid = Guid.NewGuid();
            List<string> filelist2 = new List<string>();
            //读取PDF路径
            const string Directorypath = "C:/Users/rita/Desktop/TEST";
            //PDF输出路径
            const string outpath = "C:/Users/rita/Desktop/TEST/NEW/merge.pdf";
            System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo(Directorypath);
            FileInfo[] ff2 = di2.GetFiles("*.pdf");
            PDFOperation.BubbleSort(ff2);
            List<byte[]> list = new List<byte[]>();

            foreach (FileInfo temp in ff2)
            {
                //filelist2.Add(Directorypath + "\\" + temp.Name);
                FileStream fileStream = new FileStream(temp.FullName,FileMode.Open,FileAccess.ReadWrite,FileShare.Read);
                fileStream.Position = 0;
                byte[] bt = new byte[fileStream.Length];
                fileStream.Read(bt, 0, bt.Length) ;
                fileStream.Close();
                Stream stream = new MemoryStream(bt);
                
                
                list.Add(bt);
            }
            byte[] last =  PDFOperation.mergePDFFiles(list);
            FileStream file = new FileStream("C:/Users/rita/Desktop/TEST/NEW/StreamMerge.pdf", FileMode.Create);
            file.Write(last);
            file.Close();
            //PDFOperation.DeleteAllPdf(Directorypath);
        }
    }
}
