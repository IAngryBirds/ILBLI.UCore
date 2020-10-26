using ILBLI.UCore.ILogUnity;
using ILBLI.UCore.LogUnity;
using Microsoft.Extensions.DependencyInjection;

namespace ILBLI.UCore.UnityCore
{
    /// <summary>
    /// 日志服务组件注入
    /// </summary>
    public static class UseLoggerInit
    { 
        /// <summary>
        /// 添加上层日志服务组件（需额外注入底层日志组件）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultULogger(this IServiceCollection services)
        {
            //最基础的日志服务（这个是必须注册的）
            services.AddSingleton<IULog, ULogBasic>();
            //注册定制开发的日志服务（系统日志，操作日志，HTTP请求日志）（这三个是可有可无的，如果没有注册，则使用默认的日志服务）
            services.AddSingleton<IULog, ULogSystem>();
            services.AddSingleton<IULog, ULogOpeartion>();
            services.AddSingleton<IULog, ULogHttpRequest>();
            services.AddSingleton<IULogProvider, ULogProvider>();
            return services;
        }

        /// <summary>
        /// 添加上层日志服务组件（需额外注入底层日志组件）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddULogger<T>(this IServiceCollection services) where T:class,IULog
        {
            //最基础的日志服务（这个是必须注册的）
            services.AddSingleton<IULog, ULogBasic>();
            //注册定制开发的日志服务（系统日志，操作日志，HTTP请求日志）（这三个是可有可无的，如果没有注册，则使用默认的日志服务）
            services.AddSingleton<IULog, T>();

            services.AddSingleton<IULogProvider, ULogProvider>();
            return services;
        }

        /// <summary>
        /// 添加上层日志服务组件（需额外注入底层日志组件）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddULoggers<T1, T2>(this IServiceCollection services)
            where T1 : class, IULog 
            where T2 :class,IULog
        {
            //最基础的日志服务（这个是必须注册的）
            services.AddSingleton<IULog, ULogBasic>();
            //注册定制开发的日志服务（系统日志，操作日志，HTTP请求日志）（这三个是可有可无的，如果没有注册，则使用默认的日志服务）
            services.AddSingleton<IULog, T1>();
            services.AddSingleton<IULog, T2>();
            services.AddSingleton<IULogProvider, ULogProvider>();
            return services;
        }

        /// <summary>
        /// 添加上层日志服务组件（需额外注入底层日志组件）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddULoggers<T1, T2, T3>(this IServiceCollection services)
            where T1 : class, IULog
            where T2 : class, IULog
            where T3 : class, IULog
        {
            //最基础的日志服务（这个是必须注册的）
            services.AddSingleton<IULog, ULogBasic>();
            //注册定制开发的日志服务（系统日志，操作日志，HTTP请求日志）（这三个是可有可无的，如果没有注册，则使用默认的日志服务）
            services.AddSingleton<IULog, T1>();
            services.AddSingleton<IULog, T2>();
            services.AddSingleton<IULogProvider, ULogProvider>();
            return services;
        }
    }
}
