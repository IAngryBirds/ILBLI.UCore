using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;

namespace ILBLI.UCore.UnityCore
{
    public static class UseSwaggerInit
    {

        public static IApplicationBuilder UseSwaggerUIInit(this IApplicationBuilder app)
        {
            //启用中间件服务生成swagger作为json的终结点
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            //启用中间件服务对swagger-ui，制定Swagger Json终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            //注册Swagger生成器，定义一个和多个Swagger文档
            services.AddSwaggerGen(c => { 
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MY API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = new System.Uri("http://baidu.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "ilbli",
                        Email = "757102006@qq.com",
                        Url = new System.Uri("http://baidu.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证名字",
                        Url = new System.Uri("http://baidu.com")
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(UseSwaggerInit).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "ILBLI.UCore.TestWeb.xml");
                c.IncludeXmlComments(xmlPath);
            });
            return services;
        }

    }
}
