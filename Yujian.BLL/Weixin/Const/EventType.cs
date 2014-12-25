using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.BLL.Weixin.Const
{
    /// <summary>
    /// 微信平台事件类型
    /// </summary>
    public class EventType
    {
        /// <summary>
        /// 关注
        /// </summary>
        public const string SUBSCRIBE = "subscribe";
        /// <summary>
        /// 取消关注
        /// </summary>
        public const string UNSUBSCRIBE = "unsubscribe";
        /// <summary>
        /// 扫描带参数二维码事件
        /// </summary>
        public const string SCAN = "scan";
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        public const string LOCATION = "location";
        /// <summary>
        /// 点击菜单拉取消息时的事件推送
        /// </summary>
        public const string CLICK = "click";
        /// <summary>
        /// 点击菜单跳转链接时的事件推送
        /// </summary>
        public const string VIEW = "view";
        /// <summary>
        /// 群发结果 
        /// </summary>
        public const string MASSSENDJOBFINISH = "masssendjobfinish";
    }
}
