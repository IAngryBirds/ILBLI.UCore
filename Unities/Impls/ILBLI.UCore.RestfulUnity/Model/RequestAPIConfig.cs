using System.Collections.Generic;

namespace ILBLI.UCore.RestfulUnity
{
    public class RequestAPIConfig
    {
        /// <summary>
        /// 是否使用Token
        /// </summary>
        public bool IsUseToken { get; set; }

        /// <summary>
        /// 是否复用Token
        /// </summary>
        public bool IsReuseToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long ExpireTime { get; set; }

        /// <summary>
        /// 获取Token的地址
        /// </summary>
        public string TokenUri { get; set; }

        /// <summary>
        /// 接口地址信息
        /// </summary>
        public List<RequestUriConfig> RequestUriConfigs { get; set; }

      
    }
}
