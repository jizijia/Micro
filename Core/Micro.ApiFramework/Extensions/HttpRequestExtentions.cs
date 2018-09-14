
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.ApiFramework.Extensions
{
    /// <summary>
    /// HttpRequest
    /// </summary>
    public static class HttpRequestExtentions
    {
        /// <summary>
        /// 获取User Agent
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string UserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"];
        }
    }
}
