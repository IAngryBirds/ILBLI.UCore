using ILBLI.UCore.ILogUnity;
using ILBLI.UCore.IRestfulUnity;
using Microsoft.AspNetCore.Mvc;

namespace ILBLI.UCore.TestWeb.Controller
{
    /// <summary>
    /// 默认的示例
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IULog _logger;
        private readonly IRestful _restful;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uLogProvider"></param>
        /// <param name="restful"></param>
        public ValuesController(IULogProvider uLogProvider,IRestful restful)
        {
            _logger = uLogProvider.GetOperationULog();
            _restful = restful;
        }
        [HttpGet]
        public void Index()
        {

            var msg = _restful.GetMessage("Search", "sh600789");
            
            _logger.Info(msg.Result);
        }
    }
}
