1.要使用这个HTTP请求组件，需要注册服务如下：
    services.AddHttpClient();
    services.Configure<RequestAPIConfig>(configuration.GetSection(nameof(RequestAPIConfig))); 

2.可以通过封装注册服务来实现
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

3.该套请求组件基于配置文件配置：
     "RequestAPIConfig": {
    "IsUseToken": false,
    "IsReuseToken": false,
    "ExpireTime": 0,
    "TokenUri": "",
    "RequestUriConfigs": [
      {
        "MethodKey": "Search",
        "RequestUri": "http://172.17.0.11:8003/api/Index/Search?idLstStr={0}",
        "MethodDesc": "查询指定的股票信息"
      }
    ]
   }

4.改组件下的BaseRequest是AOP拦截器做日志拦截的，而APIRequest则是基于AOP拦截器作为日志拦截的，所以，它需要基于Autofac的

    核心：  
                builder.RegisterAssemblyTypes(assemblys.ToArray())
                    .Where(x => typeof(IUAutoInterceptor).IsAssignableFrom(x))
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(typeof(IULogAopAutoInterceptor))
                    .AsImplementedInterfaces();

    整体：
        /// <summary>
        /// core 3.0下替换自己的Autofac
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder AddUAutofac(this ContainerBuilder builder)
        {
            // 获取项目所有程序集，排除Microsft,Nuget下载的，并且程序集以ILBLI开头
            //var libs = DependencyContext.Default.CompileLibraries?.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith("ILBLI."));

            List<Assembly> assemblys = new List<Assembly>();
            foreach (var libName in _LibNameList)
            {
                assemblys.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(libName)));
            }



            if (assemblys.Count > 0)
            {
                //将接口注入仓储的所有接口进行注入//过滤：留下继承了IBaseRepository/IService接口的类
                //AsImplementedInterfaces()让具体实现类型，可以该类型继承的所有接口类型找到该实现类型
                builder.RegisterAssemblyTypes(assemblys.ToArray())
                    .Where(x => typeof(IURepository).IsAssignableFrom(x) || typeof(IUService).IsAssignableFrom(x)  )
                    .AsImplementedInterfaces();

                /*
                 *  这里注意,有两种开启拦截的模式,算是一个小坑,上面的注册方式需要使用接口模式 
                    .EnableClassInterceptors() 1.这是类模式 
                    .EnableInterfaceInterceptors() 2.这是接口模式 
                    .InterceptedBy(typeof(AutofacAopExtension)) 下面这是默认注入AOP,就不需要使用特性
                    .AsImplementedInterfaces();
                */
                //先注册  
                builder.RegisterAssemblyTypes(assemblys.ToArray())
                    .Where(x => typeof(IUAutoInterceptor).IsAssignableFrom(x))
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(typeof(IULogAopAutoInterceptor))
                    .AsImplementedInterfaces();
            }
             
            return builder;
        }