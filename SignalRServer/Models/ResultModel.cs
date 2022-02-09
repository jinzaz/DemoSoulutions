using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    public class ResultModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<UserModel> OnlineUser { get; set; }
    }
}
