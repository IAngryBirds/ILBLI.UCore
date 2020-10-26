using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.UCore.LogUnity
{
    /// <summary>
    /// 用户操作日志
    /// </summary>
    public class ULogOpeartion : ULogBasic
    {
        protected override string LogName => "操作日志服务";

        public ULogOpeartion(ILogger<ULogOpeartion> logger) : base(logger)
        {

        }

        public void Error(string message, Exception ex)
        {
            base.Error(message,ex);
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
