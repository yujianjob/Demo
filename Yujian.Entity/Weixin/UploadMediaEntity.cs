﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Entity.Weixin
{
    /// <summary>
    /// 上传媒体文件接口
    /// </summary>
    public class UploadMediaEntity : ResultEntity
    {
        /// <summary>
        /// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 媒体文件上传后获取的唯一标识
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 媒体文件上传时间
        /// </summary>
        public string created_at { get; set; }
    }
}
