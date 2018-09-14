using Micro.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Micro.ApiFramework.Middlewares
{
    /// <summary>
    /// 错误处理中间件
    /// </summary>
    public class ErrorHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandleMiddleware> _logger;
        private readonly IClientContext _clientContext;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public ErrorHandleMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<ErrorHandleMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            try
            {
                _clientContext = IocResolver.Resolve<IClientContext>();
            }
            catch { }
        }
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                await _next(context);
            }
            catch (CustomException customException)
            {
                sb = new StringBuilder();
                sb.AppendLine($"CUSTOM_EXCEPTION\t{customException.Message}");
                if (context.Response.HasStarted)
                {
                    sb.AppendLine($"\tThe response has already started, the http status code middleware will not be executed.");
                    throw;
                }
                context.Response.Clear();
                context.Response.StatusCode = 900;// customException.HttpStatusCode;
                context.Response.ContentType = "text/plain; charset=utf-8"; ;
                await context.Response.WriteAsync(JsonHelper.ToJson(new { message = customException.Message }));
                return;
            }
            catch (Exception ex)
            {
                sb = new StringBuilder();
                sb.AppendLine($"UNHANDLE_EXCEPTION\t{ex.Message}");
                if (context.Response.HasStarted)
                {
                    sb.AppendLine($"\tThe response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = 900;
                context.Response.ContentType = "text/plain; charset=utf-8"; ;
                await context.Response.WriteAsync(JsonHelper.ToJson(new { message = "System error, please try again." }));
                return;
            }
            finally
            {
                if (sb.Length > 0)
                {
                    if (_clientContext != null)
                    {
                        sb.Insert(0, $"MemberId:{_clientContext.User?.Id}\r\n\tMemberName:{_clientContext.User?.UserName}\r\n\tToken:{_clientContext.User.Token}\r\n\tPath:{context.Request?.Path}\r\n");
                    }
                    _logger.LogError(sb.ToString());
                }
            }
        }
    }

    /// <summary>
    /// 错误处理中间件扩展
    /// </summary>
    public static class ErrorHandleMiddlewareExtensions
    {
        /// <summary>
        /// 使用错误处理中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandleMiddleware>();
        }
    }
}
