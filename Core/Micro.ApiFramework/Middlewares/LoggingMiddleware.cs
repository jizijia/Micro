using Micro.ApiFramework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.ApiFramework.Middlewares
{
    /// <summary>
    /// 日志中间件
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public LoggingMiddleware(RequestDelegate next,
            ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;
            string method = context.Request.Method.ToUpper();
            string agent = context.Request.UserAgent();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}\t{method}\t{path}");
            sb.AppendLine($"Agent:{agent}");
            var headers = context.Request.Headers;
            if (headers != null)
            {
                sb.AppendLine("Headers");
                headers?.ToList().ForEach(x =>
                {
                    sb.AppendLine($"\t{x.Key}:{x.Value}");
                });
            }
            _logger.LogInformation(sb.ToString());
            sb.Clear();
            sb.AppendLine("Parameters");
            switch (method)
            {
                //case "POST":
                //    var form = context.Request.ReadFormAsync().Result;
                //    form.ToList().ForEach(y =>
                //    {
                //        sb.AppendLine($"\t{y.Key}:{y.Value}");
                //    });
                //    break;
                case "GET":
                    var query = context.Request.Query;
                    query.ToList().ForEach(x =>
                    {
                        sb.AppendLine($"\t{x.Key}:{x.Value}");
                    });
                    break;
                case "PUT":
                    break;
                case "DELETE":
                    break;
                default:
                    break;
            }

            _logger.LogInformation(sb.ToString());
            return this._next(context);
        }
    }
    /// <summary>
    /// 访问日志中间件扩展
    /// </summary>
    public static class LoggingMiddlewareExtensions
    {
        /// <summary>
        /// 使用访问日志中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLoggingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
