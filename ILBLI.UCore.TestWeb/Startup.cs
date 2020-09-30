using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILBLI.UCore.ExceptionUnity;
using ILBLI.UCore.IExceptionUnity;
using ILBLI.UCore.ILogUnity;
using ILBLI.UCore.LogUnity;
using ILBLI.UCore.UnityCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ILBLI.UCore.TestWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option=>option.EnableEndpointRouting=false);
            services.AddSingleton<IULog, ULogSystem>();
            services.AddSingleton<IULog, ULogOpeartion>();
            services.AddSingleton<IULog, ULogHttpRequest>();
            services.AddSingleton<IULogFactory, ULogFactory>();
            services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            //app.UseRouting();
            //添加自己封装的swaggerUI
            app.UseSwaggerUIInit();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
