using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuJian.WeiXin.Entity.Interface.Request
{
    public class GetCurrentDataRequest
    {
        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string OpenID { get; set; }

    }
}
