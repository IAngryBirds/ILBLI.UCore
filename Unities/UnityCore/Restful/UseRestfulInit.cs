using ILBLI.UCore.RestfulUnity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ILBLI.UCore.UnityCore
{
    public static class UseRestfulInit
    {
        /// <summary>
        /// 添加上层日志服务组件（需额外注入底层日志组件）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRestful(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpClient();
             
            services.Configure<RequestAPIConfig>(configuration.GetSection(nameof(RequestAPIConfig))); 

            return services;
        }
    }
}
