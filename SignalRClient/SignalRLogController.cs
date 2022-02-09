using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRClient
{
    public class SignalRLogController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLog(string message)
        {
            var connection = new HubConnectionBuilder()
                .ConfigureLogging(logging => 
                {
                    logging.AddDebug();

                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();

            return new JsonResult(new object { });
        }
    }
}
