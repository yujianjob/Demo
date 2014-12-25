using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Entity.Weixin
{
    /// <summary>
    /// 上传群发实体
    /// </summary>
    public class UploadBroadCastEntity:ResultEntity
    {
        /// <summary>
        /// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb），次数为news，即图文素材 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        ///  	媒体文件/图文素材上传后获取的唯一标识 
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        ///  	媒体文件上传时间 
        /// </summary>
        public string created_at { get; set; }    
    }
}
