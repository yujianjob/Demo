using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Entity.Weixin
{
    /// <summary>
    /// 二维码实体
    /// </summary>
    public class QRCodeEntity : ResultEntity
    {
        /// <summary>
        /// 调用接口凭证
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public string expire_seconds { get; set; }
    }
}
