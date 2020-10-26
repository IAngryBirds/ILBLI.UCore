using ILBLI.UCore.ILogUnity;
using System;
using System.Collections.Generic;

namespace ILBLI.UCore.LogUnity
{
    public class ULogProvider: IULogProvider
    {
        private readonly IULog _uLogBasic;
        private readonly IULog _uLogSystem;
        private readonly IULog _uLogOperation;
        private readonly IULog _uLogHttpRequest;

        /// <summary>
        /// 至少需要注册ULogBasic通用日志服务
        /// </summary>
        /// <param name="uLogs"></param>
        public ULogProvider(IEnumerable<IULog> uLogs)
        {
            if(uLogs!=null)
            {  
                foreach (var logger in uLogs)
                {
                    if(logger is ULogSystem)
                    {
                        _uLogSystem = logger;
                    }
                    else if(logger is ULogOpeartion)
                    {
                        _uLogOperation = logger;
                    }
                    else if(logger is ULogHttpRequest)
                    {
                        _uLogHttpRequest = logger;
                    }
                    else
                    { 
                        //默认的日志服务，必须放在最后，因为它是基类
                        _uLogBasic = logger; 
                    }
                }
            }

            if (_uLogBasic == null)
            {
                throw new SystemException("系统初始化失败：尚未注册通用日志服务-ULogBasic");
            }
            if(_uLogSystem == null)
            {
                _uLogSystem = _uLogBasic;
            }
            if(_uLogOperation == null)
            {
                _uLogOperation = _uLogBasic;
            }
            if(_uLogHttpRequest == null)
            {
                _uLogHttpRequest = _uLogBasic;
            }
        }
        public IULog GetSystemULog()
        {
            return _uLogSystem;
        }
        public IULog GetOperationULog()
        {
            return _uLogOperation;
        }
        public IULog GetHttpRequestULog()
        {
            return _uLogHttpRequest;
        }
    }
}