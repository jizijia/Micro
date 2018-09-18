using Micro.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.ApiFramework
{
    public class DefaultClientContext : IClientContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public DefaultClientContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        /// <summary>
        /// UserAgent
        /// </summary>
        public string UserAgent
        {
            get => _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"];
        }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp
        {
            get => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
        }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestPath
        {
            get => _httpContextAccessor.HttpContext?.Request?.Path;
        }
        public dynamic User { get; set; }

        public string CreateToken(dynamic user)
        {
            return JsonHelper.ToJson(user);
        }
        //internal class CacheUser
        //{
        //    string UserInfo { get; set; }
        //    string IP { get; set; }
        //}
    }
}
