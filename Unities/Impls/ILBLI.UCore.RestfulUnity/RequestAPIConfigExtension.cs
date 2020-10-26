using ILBLI.UCore.BasicCommon;
using ILBLI.UCore.BasicModel;

namespace ILBLI.UCore.RestfulUnity
{
    public static  class RequestAPIConfigExtension
    {
        /// <summary>
        /// 获取指定方法key的uri地址
        /// </summary>
        /// <param name="config"></param>
        /// <param name="mothedKey">方法Key</param>
        /// <returns></returns>
        public static string GetMethodKeyUri(this RequestAPIConfig config,string mothedKey)
        {
            var uriConfig = config?.RequestUriConfigs?.Find(x => x.MethodKey.EqualIgnoreCase(mothedKey));
            if (uriConfig == null || uriConfig.RequestUri.IsNullOrEmpty())
                throw new HttpRequestConnectionException($"接口请求：{mothedKey}uri地址尚未配置");

            return uriConfig.RequestUri;
        }
    }
}
