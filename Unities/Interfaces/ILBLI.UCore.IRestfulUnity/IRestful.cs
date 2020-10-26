using ILBLI.UCore.BasicCommon;
using System.Net.Http;
using System.Threading.Tasks;

namespace ILBLI.UCore.IRestfulUnity
{
    public interface IRestful :IUService, IUAutoInterceptor
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task RequestTokenAsync(StringContent content);

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="postMsg"></param>
        /// <returns></returns>
        Task<string> GetMessage(string requestUri, string postMsg);
        
    }
}
