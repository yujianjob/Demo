using System.Xml;

namespace Yunchee.Volkswagen.Entity.Weixin
{
    /// <summary>
    /// 微信平台请求参数
    /// </summary>
    public class RequestParams
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 公众平台ID
        /// </summary>
        public string WeixinId { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 微信平台推送的消息
        /// </summary>
        public XmlNode XmlNode { get; set; }
    }
}
