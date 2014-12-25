using System.Web;
using Yunchee.Volkswagen.Entity.Weixin;

namespace Yunchee.Volkswagen.BLL.Weixin.Base
{
    /// <summary>
    /// 微信服务号基类
    /// </summary>
    public class ServiceBLL : BaseBLL
    {
        #region 构造函数

        public ServiceBLL(HttpContext httpContext, RequestParams requestParams)
            : base(httpContext, requestParams)
        {

        }

        #endregion
    }
}
