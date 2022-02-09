using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PipeLines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        async Task ProcessLinesAsync(NetworkStream stream)
        {
            var buffer = new byte[1024];
            await stream.ReadAsync(buffer,0,buffer.Length);

            //在buffer中处理一行信息
            ProcessLine(buffer);
        }
    }
}