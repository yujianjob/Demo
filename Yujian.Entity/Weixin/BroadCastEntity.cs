using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Entity.Weixin
{
    /// <summary>
    /// 群发返回结果
    /// </summary>
    public class BroadCastEntity:ResultEntity
    {
        /// <summary>
        ///  	消息ID  
        /// </summary>
        public string msg_id { get; set; }
        /// <summary>
        ///  	媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb），次数为news，即图文素材  
        /// </summary>
        public string type { get; set; }
    }
}
