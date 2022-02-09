using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace QuartzDemo.Controllers
{
    /// <summary>
    /// 健康接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GetHealthController : ControllerBase
    {
        /// <summary>
        /// 获取健康信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponse Get()
        {
            var response = HttpContext.Response;
            response.StatusCode = 200;
            response.ContentType = "Application/json";
            Console.WriteLine("asfasfas");
            response.WriteAsync("啊沙发沙发上");
            return response;
        }
    }
}