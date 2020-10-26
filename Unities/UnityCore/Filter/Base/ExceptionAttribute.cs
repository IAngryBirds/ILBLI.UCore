using ILBLI.UCore.IExceptionUnity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ILBLI.UCore.UnityCore
{
    /// <summary>
    /// 第四层（Controller实例化之后）这层的异常捕获只会捕获Controller实例化之后的异常信息
    /// </summary>
    public class ExceptionAttribute : Attribute, IExceptionFilter
    {
        private readonly IExceptionHandler _ExceptionHandler;

        public ExceptionAttribute(IExceptionHandler handler)
        {
            _ExceptionHandler = handler;
        }

        public void OnException(ExceptionContext context)
        {
            //如果截获异常为我们自定义，可以处理的异常则通过我们自己的规则处理 
            string getMsg = _ExceptionHandler.WriteLog(context.Exception);
            context.Result = new BadRequest(new { Message = getMsg });
        }
    }


    /// <summary>
    /// 请求失败
    /// </summary>
    public class BadRequest : ObjectResult
    {
        public BadRequest(object value):base(value)
        {
            StatusCode = 400;
        }
    }

}
