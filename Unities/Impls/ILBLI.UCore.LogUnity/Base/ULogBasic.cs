using ILBLI.UCore.ILogUnity;
using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.UCore.LogUnity
{
    /// <summary>
    /// 通用基础日志【基于log4net服务实现】
    /// </summary>
    public class ULogBasic : IULog
    {
        /// <summary>
        /// 当前日志服务名
        /// </summary>
        protected virtual string LogName => "通用日志服务";

        private readonly ILogger<ULogBasic> _logger;
        public ULogBasic(ILogger<ULogBasic> logger)
        {
            _logger = logger;
        }
        
        public virtual void Error(string message, Exception ex)
        {
            _logger.LogError(ex,$"{LogName}[{DateTime.Now}]_[ERROR]:{message}[{ex.Message}]");
        }

        public virtual void Info(string message)
        {
            _logger.LogInformation($"{LogName}[{DateTime.Now}]_[INFO]:{message}");
        }

        public virtual void Warn(string message)
        {
            _logger.LogWarning($"{LogName}[{DateTime.Now}]_[WARN]:{message}");
        }
    }
}
