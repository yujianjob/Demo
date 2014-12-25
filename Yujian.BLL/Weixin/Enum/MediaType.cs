using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.BLL.Weixin.Enum
{
    /// <summary>
    /// 媒体文件类型
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 语音
        /// </summary>
        Voice,
        /// <summary>
        /// 视频
        /// </summary>
        Video,
        /// <summary>
        /// 缩略图
        /// </summary>
        Thumb
    }
    
    /// <summary>
    /// 自动回复类型
    /// 1=微信菜单
    ///2=被添加自动回复
    ///3=消息自动回复
    ///4=关键词自动回复
    /// </summary>
    public enum AutoReplyType
    {
        /// <summary>
        /// 微信菜单
        /// </summary>
        MenuKey=1,
        /// <summary>
        /// 被添加自动回复
        /// </summary>
        Subscrib=2,
        /// <summary>
        /// 消息自动回复
        /// </summary>
        Message=3,
        /// <summary>
        /// 关键词自动回复
        /// </summary>
        KeyWord=4
    }
}
