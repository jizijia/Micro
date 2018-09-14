using Autofac;
using Autofac.Extensions.DependencyInjection;
using Micro.ApiFramework;
using Micro.ApiFramework.Extensions;
using Micro.ApiFramework.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Micro.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly object loggerFactory;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            loggerFactory = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // 跨域处理
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices().AddJsonOptions(Defaults.JsonOptionAction); ;

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "测试项目 - Web API",
                    Version = "v1"
                });
            });

            // 采用Autofac替代自带Ioc
            return services.TransToAutofac();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddNLog();
            app.UseLoggingMiddleware();
            app.UseErrorHandleMiddleware();

            app.UseMvc();

            // Swagger
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "RYH.API V1");
               });
        }
    }
}
