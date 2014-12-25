using System.Web;
using Yunchee.Volkswagen.Entity.Weixin;

namespace Yunchee.Volkswagen.BLL.Weixin.Base
{
    /// <summary>
    /// 微信认证服务号基类
    /// </summary>
    public class CertificationBLL : BaseBLL
    {
        #region 构造函数

        public CertificationBLL(HttpContext httpContext, RequestParams requestParams)
            : base(httpContext, requestParams)
        {

        }

        #endregion
    }
}
