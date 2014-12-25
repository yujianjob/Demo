using System.Web;
using Yunchee.Volkswagen.Entity.Weixin;

namespace Yunchee.Volkswagen.BLL.Weixin.Base
{
    /// <summary>
    /// 微信订阅号基类
    /// </summary>
    public class SubscriptionBLL : BaseBLL
    {
        #region 构造函数

        public SubscriptionBLL(HttpContext httpContext, RequestParams requestParams)
            : base(httpContext, requestParams)
        {

        }

        #endregion
    }
}
