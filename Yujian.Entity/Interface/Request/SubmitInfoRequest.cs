using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuJian.WeiXin.Entity.Interface.Request
{
    public class SubmitInfoRequest
    {
        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string OpenID { get; set; }

        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string Phone { get; set; }
    }
}
