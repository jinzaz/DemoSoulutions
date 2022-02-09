using System;

namespace ChartState
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
              var s=  GetCity("杭州市市西湖区省市府路1号");

            }
            catch (Exception ex)
            {

                throw;
            }
            Console.ReadKey();
        }

        static string GetCity(string Content)
        {
            int ProvinceIndex = Content.IndexOf("省");
            int ProvinceEndIndex = Content.LastIndexOf("省");
            int CityIndex = Content.IndexOf("市");
            int CityEndIndex = Content.LastIndexOf("市");
            if (CityIndex < 0)
            {
                int CountyIndex = Content.IndexOf("县");
                int CountyEndIndex = Content.LastIndexOf("县");
                if (ProvinceIndex > -1 && CountyIndex == CountyEndIndex)
                {
                    return Content.Substring(ProvinceIndex + 1, CountyIndex - ProvinceIndex);
                }
                else
                {
                    return Content.Substring(0, CountyEndIndex + 1);
                }
            }
            else
            {
                if (ProvinceIndex > -1 && CityIndex == CityEndIndex && ProvinceIndex < CityIndex)
                {
                    return Content.Substring(ProvinceIndex + 1, CityIndex - ProvinceIndex);
                }
                else if (CityIndex == CityEndIndex)
                {
                    return Content.Substring(0, CityEndIndex + 1);
                }
                else
                {
                    return Content.Substring(0, CityIndex + 1);
                }
            }
        }
    }
}
