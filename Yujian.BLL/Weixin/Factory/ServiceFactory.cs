using System.Web;
using Yunchee.Volkswagen.BLL.Weixin.Base;
using Yunchee.Volkswagen.BLL.Weixin.Interface;
using Yunchee.Volkswagen.Entity.Weixin;

namespace Yunchee.Volkswagen.BLL.Weixin.Factory
{
    /// <summary>
    /// 微信服务号基类
    /// </summary>
    public class ServiceFactory : IFactory
    {
        public BaseBLL CreateWeiXin(HttpContext httpContext, RequestParams requestParams)
        {
            return new ServiceBLL(httpContext, requestParams);
        }
    }
}
