using ILBLI.UCore.BasicCommon;
using ILBLI.UCore.BasicModel;
using ILBLI.UCore.ILogUnity;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ILBLI.UCore.RestfulUnity
{
    public abstract class BaseRequest 
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly RequestAPIConfig _httpRequestConfig;
        /// <summary>
        /// Token的值
        /// </summary>
        private string Token { get; set; }
        /// <summary>
        /// 上次获取Token的时间
        /// </summary>
        private DateTime NextCreateTime { get; set; }
        /// <summary>
        /// token 锁
        /// </summary>
        private static object _lockToken => new object();

        private readonly IULog _uLog;

        public BaseRequest(IHttpClientFactory clientFactory,IOptions<RequestAPIConfig> reqOption,IULogProvider logProvider)
        {
            _clientFactory = clientFactory;
            _httpRequestConfig = reqOption.Value;
            _uLog = logProvider.GetHttpRequestULog();
        }


        private string GetTokenAsync()
        {
            if (!Token.IsNullOrEmpty() && DateTime.Now < NextCreateTime)
            {
                return Token;
            }

            lock (_lockToken)
            {
                if(Token.IsNullOrEmpty() || DateTime.Now >= NextCreateTime)
                {
                    RequestTokenAsync(null).Wait();
                } 
            }
            
            return Token;
        }

        /// <summary>
        /// 获取Token凭证
        /// </summary>
        /// <param name="headerAction"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task RequestTokenAsync(StringContent content)
        {
            string requestUri = _httpRequestConfig.TokenUri;

            var resMsg = await SendMessage(HttpMethod.Post, requestUri, content);

            Token = resMsg;
            NextCreateTime = DateTime.Now.AddMinutes(_httpRequestConfig.ExpireTime);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="postMsg"></param>
        /// <returns></returns>
        public virtual async Task<string> GetMessage(string requestUri, string postMsg)
        {
            StringContent content = new StringContent(postMsg);
            //如果需要Toekn则请求头加Token
            if (_httpRequestConfig.IsUseToken)
            {
                content.Headers.Add("Beraer", GetTokenAsync());
            }
            return await SendMessage(HttpMethod.Get, requestUri, content); 
        }


        /// <summary>
        /// 发送消息服务  
        /// </summary>
        /// <param name="method">请求方法Get,Post...</param>
        /// <param name="requestUri">请求地址</param>
        /// <param name="content">请求内容</param>
        /// <param name="headerAction">头部处理委托</param>
        /// <returns></returns>
        protected async Task<string> SendMessage(HttpMethod method,string requestUri, StringContent content)
        {
            string guidStr = Guid.NewGuid().ToString();
            _uLog.Info($"{guidStr}:请求地址：{requestUri};请求方式：{method.ToString()};请求参数：{content.ReadAsStringAsync()}");

            HttpRequestMessage requestMessage = new HttpRequestMessage(method, requestUri);
            try
            { 
                if (content != null)
                    requestMessage.Content = content;

                HttpClient client = _clientFactory.CreateClient();
                //请求超时30秒
                client.Timeout = new TimeSpan(0, 0, 30);

                var response = await client.SendAsync(requestMessage);
                string reqStr = string.Empty;
                if (response.IsSuccessStatusCode)
                {
                    reqStr = response.Content.ReadAsStringAsync().Result; 
                }
                else
                {
                    throw new HttpRequestConnectionException($"{requestUri}:请求失败!--返回状态异常！");
                }

                _uLog.Info($"{guidStr}:请求地址：{requestUri},请求成功! 返回状态：{response.StatusCode},返回数据：{reqStr}");

                return reqStr;
            }
            catch (Exception ex)
            {
                _uLog.Info($"{guidStr}:请求地址：{requestUri},请求失败!");

                throw new HttpRequestConnectionException($"{requestUri}:请求失败!--捕获到异常的信息【{ex.Message}】", ex);
            }
        }
    }
}
