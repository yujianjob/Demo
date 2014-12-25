
namespace Yunchee.Volkswagen.Entity.Weixin
{
    /// <summary>
    /// 获取凭证接口
    /// </summary>
    public class AccessTokenEntity : ResultEntity
    {
        /// <summary>
        /// 调用接口凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public string expires_in { get; set; }
    }
}
