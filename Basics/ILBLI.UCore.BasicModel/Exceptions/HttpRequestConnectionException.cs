using System.Net.Http;

namespace ILBLI.UCore.BasicModel
{
    /// <summary>
    /// 三方系统连接异常【连接中断--404,401,500】
    /// </summary>
    public class HttpRequestConnectionException : HttpRequestException
    {
    }
}
