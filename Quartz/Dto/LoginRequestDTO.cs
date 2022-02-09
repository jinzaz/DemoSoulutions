using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzDemo.Dto
{
    /// <summary>
    /// 登录数据
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [JsonProperty("username")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
