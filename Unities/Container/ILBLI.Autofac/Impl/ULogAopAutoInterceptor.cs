using Castle.DynamicProxy;
using ILBLI.UCore.BasicCommon;
using ILBLI.UCore.ILogUnity;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ILBLI.UCore.Autofac
{
    /// <summary>
    /// 日志拦截服务
    /// </summary>
    public class ULogAopAutoInterceptor : IULogAopAutoInterceptor
    {
        private readonly IULog _ulog;
        public ULogAopAutoInterceptor(IULogProvider uLogProvider)
        {
            _ulog = uLogProvider.GetSystemULog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public async void Intercept(IInvocation invocation)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            string jsonStr = JsonConvert.SerializeObject(invocation.Arguments);
            //方法执行前
            string msg = $"当前请求方法({invocation.Method.Name});请求参数({jsonStr});请求开始...";
            _ulog.Info(msg);

            //执行方法
            invocation.Proceed();

            //方法执行后
            jsonStr = "获取执行后的方法异常";
            try
            {
                //判断是否是异步的方法,如果是异步的方法直接返回
                if (IsAsyncMethod(invocation.Method))
                    return;
                
                sp.Stop();
                jsonStr = JsonConvert.SerializeObject(invocation.ReturnValue);

            }
            catch(Exception ex)
            {
                _ulog.Warn($"日志拦截器AOP异常：{ex.Message}");
            }

            msg += $"---请求结束,返回数据{jsonStr};总耗时{sp.ElapsedMilliseconds}"; 
            _ulog.Info(msg);
        }

        private bool IsAsyncMethod(MethodInfo method)
        {
            return (
            method.ReturnType == typeof(Task) ||
            (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }
    }
}
