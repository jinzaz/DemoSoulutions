using QuartzDemo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzDemo.Interface
{
   public interface ITokenAuthenticateService
   {
        bool IsAuthenticated(LoginRequestDTO request,out string token);
   }
}
