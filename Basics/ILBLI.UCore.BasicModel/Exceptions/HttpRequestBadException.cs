using System.Net.Http;

namespace ILBLI.UCore.BasicModel
{
    /// <summary>
    /// 三方系统通讯返回消息内容为异常状态【返回数据解析成功后--消息体内容状态值非正常状态】
    /// </summary>
    public class HttpRequestBadException:HttpRequestException
    {
    }
}
