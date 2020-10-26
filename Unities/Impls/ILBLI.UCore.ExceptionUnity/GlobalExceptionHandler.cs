using ILBLI.UCore.BasicModel;
using ILBLI.UCore.IExceptionUnity;
using ILBLI.UCore.ILogUnity;
using System;

namespace ILBLI.UCore.ExceptionUnity
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class GlobalExceptionHandler:IExceptionHandler
    {
        private readonly IULog _logger;

        public GlobalExceptionHandler(IULogProvider logProvder)
        {
            _logger = logProvder.GetSystemULog();
        }

        public string WriteLog(Exception ex)
        {
            //如果截获异常为我们自定义，可以处理的异常则通过我们自己的规则处理 
            string resMsg = ex.Message;

            if (ex is HttpRequestBadException)
            {
                _logger.Error(string.Empty, ex);
            }
            else
            {
                _logger.Error("意料之外的异常", ex);
                resMsg = "服务器被外星人拐跑了!";
            }

            return resMsg;
        }
    }
}
