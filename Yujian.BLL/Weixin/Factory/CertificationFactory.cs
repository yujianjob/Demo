using System.Web;
using Yunchee.Volkswagen.BLL.Weixin.Base;
using Yunchee.Volkswagen.BLL.Weixin.Interface;
using Yunchee.Volkswagen.Entity.Weixin;

namespace Yunchee.Volkswagen.BLL.Weixin.Factory
{
    /// <summary>
    /// 微信认证服务号基类
    /// </summary>
    public class CertificationFactory : IFactory
    {
        public BaseBLL CreateWeiXin(HttpContext httpContext, RequestParams requestParams)
        {
            return new CertificationBLL(httpContext, requestParams);
        }
    }
}
