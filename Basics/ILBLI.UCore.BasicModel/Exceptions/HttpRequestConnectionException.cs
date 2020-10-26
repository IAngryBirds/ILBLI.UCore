using System;
using System.Net.Http;

namespace ILBLI.UCore.BasicModel
{
    /// <summary>
    /// 三方系统连接异常【连接中断--404,401,500】
    /// </summary>
    public class HttpRequestConnectionException : HttpRequestException
    {
        public HttpRequestConnectionException(string message) : base(message)
        {
        }

        public HttpRequestConnectionException(string message,Exception inner) : base(message, inner)
        {

        }
    }
}
