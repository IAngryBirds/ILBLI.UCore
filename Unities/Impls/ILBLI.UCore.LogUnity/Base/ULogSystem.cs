using Microsoft.Extensions.Logging;
using System;

namespace ILBLI.UCore.LogUnity
{
    /// <summary>
    /// 系统日志 --基于Redis日志--写入Elaksearch
    /// </summary>
    public class ULogSystem : ULogBasic
    {
        protected override string LogName => "系统日志服务";
        
        /// <summary>
        /// 继承父类构造函数
        /// </summary>
        /// <param name="logger"></param>
        public ULogSystem(ILogger<ULogSystem> logger) : base(logger)
        {

        }


        public override void Error(string message, Exception ex)
        {
            //执行自己的方法 

            //继续执行父类的方法
            base.Error(message, ex);
        }

        public override void Info(string message)
        {
            base.Info(message);
        }

        public override void Warn(string message)
        {
            base.Warn(message);
        }
    }
}
