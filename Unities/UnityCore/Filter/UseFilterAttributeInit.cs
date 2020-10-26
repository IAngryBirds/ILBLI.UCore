using ILBLI.UCore.ExceptionUnity;
using ILBLI.UCore.IExceptionUnity;
using Microsoft.Extensions.DependencyInjection;

namespace ILBLI.UCore.UnityCore
{
    public static class UseFilterAttributeInit
    {
        /// <summary>
        /// 注册拦截层服务组件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomFilters(this IServiceCollection services)
        { 
            services.AddSingleton<IExceptionHandler,GlobalExceptionHandler>();
            //注册拦截器层
            services.AddMvcCore(option => {
                option.Filters.Add<ExceptionAttribute>();
                //拦截器顺序 IAuthorizationFilter --> IResourceFilter -->IActionFilter --> IResultFilter 跟注入时的顺序无关
                //日志打印顺序: IAuthorizationFilter_In --> IResourceFilter_In --> IActionFilter_In -->[方法体] -->
                //              IActionFilter_Out --> IResultFilter_In -->IResultFilter_Out -->IResourceFilter_Out  
                //option.Filters.Add<ResultAttribute>();
                //option.Filters.Add<DataAnnotationAttribute>();
                //option.Filters.Add<ResourceAttribute>();
                //option.Filters.Add<AuthorizationAttribute>(); 
            });
            return services;
        } 
    }
}
