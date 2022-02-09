using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuartzDemo.Dto;
using QuartzDemo.Interface;
using QuartzDemo.Util.jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo.Util.common
{
    public class TokenAuthenticationService : ITokenAuthenticateService
    {
        private readonly IUserReqService _userReqService;
        private readonly TokenManagement _tokenManagement;
        public TokenAuthenticationService(IUserReqService userReqService,IOptions<TokenManagement> tokenManagement)
        {
            this._userReqService = userReqService;
            this._tokenManagement = tokenManagement.Value;
        }
        public bool IsAuthenticated(LoginRequestDTO request,out string token)
        {
            token = string.Empty;
            if(!_userReqService.IsValid(request))
            {
                return false;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer,_tokenManagement.Audience,claims,expires:DateTime.Now.AddMilliseconds(_tokenManagement.AccessExpiration),signingCredentials:credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
