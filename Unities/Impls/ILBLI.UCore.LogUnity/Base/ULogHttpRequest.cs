using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.UCore.LogUnity
{
    /// <summary>
    /// 三方接口日志
    /// </summary>
    public class ULogHttpRequest : ULogBasic
    {
        protected override string LogName => "三方接口服务";
        public ULogHttpRequest(ILogger<ULogHttpRequest> logger) : base(logger)
        {

        }
        public void Error(string message, Exception ex)
        {
            base.Error(message, ex);
        }

        public void Info(string message)
        {

            base.Info(message);
        }

        public void Warn(string message)
        {
            base.Warn(message);
        }
    }
}
