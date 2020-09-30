using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILBLI.UCore.ExceptionUnity;
using ILBLI.UCore.IExceptionUnity;
using ILBLI.UCore.ILogUnity;
using ILBLI.UCore.LogUnity;
using ILBLI.UCore.TestWeb.T1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ILBLI.UCore.TestWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
