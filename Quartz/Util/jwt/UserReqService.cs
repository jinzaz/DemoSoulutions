using QuartzDemo.Dto;
using QuartzDemo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzDemo.Util.jwt
{
    public class UserReqService : IUserReqService
    {
        public bool IsValid(LoginRequestDTO req)
        {
            return true;
        }
    }
}
