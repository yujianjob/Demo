using System.Web;
using Yunchee.Volkswagen.BLL.Weixin.Base;
using Yunchee.Volkswagen.Entity.Weixin;

namespace Yunchee.Volkswagen.BLL.Weixin.Interface
{
    /// <summary>
    /// 微信工厂
    /// </summary>
    public interface IFactory
    {
        BaseBLL CreateWeiXin(HttpContext httpContext, RequestParams requestParams);
    }
}
