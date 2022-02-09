using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PDFHelper
{
    public static class PDFOperation
    {
        /// <summary>
        /// 读取合并的pdf文件名称
        /// </summary>
        /// <param name="Directorypath">目录</param>
        /// <param name="outpath">导出的路径</param>
        public static void MergePDF(string inputpath, string outpath)
        {

            List<string> filelist2 = new List<string>();
            System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo(inputpath);
            FileInfo[] ff2 = di2.GetFiles("*.pdf");
            BubbleSort(ff2);
            foreach (FileInfo temp in ff2)
            {
                filelist2.Add(inputpath + "\\" + temp.Name);
            }
            mergePDFFiles(filelist2, outpath);
            //如不需要删除原有文件，可以注释
            DeleteAllPdf(inputpath);
        }

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="arr">文件名数组</param>
        public static void BubbleSort(FileInfo[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i; j < arr.Length; j++)
                {
                    if (arr[i].LastWriteTime > arr[j].LastWriteTime)//按创建时间（升序）
                    {
                        FileInfo temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 合成pdf文件
        /// </summary>
        /// <param name="fileList">文件名list</param>
        /// <param name="outMergeFile">输出路径</param>
        public static void mergePDFFiles(List<string> fileList, string outMergeFile)
        {
            PdfReader reader;
            Rectangle rec = new Rectangle(2105, 1487);
            Document document = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outMergeFile, FileMode.Create));
            document.Open();
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            PdfImportedPage newPage;
            for (int i = 0; i < fileList.Count; i++)
            {
                reader = new PdfReader(fileList[i]);
                int iPageNum = reader.NumberOfPages;
                for (int j = 1; j <= iPageNum; j++)
                {
                    newPage = writer.GetImportedPage(reader, j);
                    Rectangle r = reader.GetPageSize(j);
                    document.SetPageSize(r);
                    document.NewPage();
                    cb.AddTemplate(newPage, 0, 0);
                }
            }
            document.Close();
        }

        /// <summary>
        /// 合成pdf文件
        /// </summary>
        /// <param name="fileList">字节流list</param>
        /// <param name="outMergeFile">输出路径</param>
        public static byte[] mergePDFFiles(List<byte[]> fileList)
        {
            PdfReader reader;
            Rectangle rec = new Rectangle(2105, 1487);
            Document document = new Document(rec);
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            //PdfWriter writer = PdfWriter.GetInstance(document, ms);
            //iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            PdfImportedPage newPage;
            using (var copy = new PdfSmartCopy(document,ms))
            {
                document.Open();
                for (int i = 0; i < fileList.Count; i++)
                {
                    //reader = new PdfReader(fileList[i]);
                    //int iPageNum = reader.NumberOfPages;
                    //for (int j = 1; j <= iPageNum; j++)
                    //{
                    //    newPage = writer.GetImportedPage(reader, j);
                    //    Rectangle r = reader.GetPageSize(j);
                    //    document.SetPageSize(r);
                    //    document.NewPage();
                    //    cb.AddTemplate(newPage, 0, 0);
                    //}
                    reader = new PdfReader(fileList[i]);
                    copy.AddDocument(reader);
                }
            }
            document.Close();
            //byte[] lastfile = new byte[ms.Length];
            //ms.Write(lastfile, 0, lastfile.Length);

            //ms.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// 删除一个文件里所有的文件
        /// </summary>
        /// <param name="Directorypath">文件夹路径</param>
        public static void DeleteAllPdf(string Directorypath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Directorypath);
            if (di.Exists)
            {
                FileInfo[] ff = di.GetFiles("*.pdf");
                foreach (FileInfo temp in ff)
                {
                    File.Delete(Directorypath + "\\" + temp.Name);
                }
            }
        }
    }
}
