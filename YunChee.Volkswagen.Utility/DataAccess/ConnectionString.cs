﻿/*
 * Author		:
 * EMail		:
 * Company		:
 * Create On	:
 * Description	:
 * 1st Modified On	:
 * 1st Modified By	:
 * 1st Modified Desc:
 * 1st Modifier EMail	:
 * 2st Modified On	:
 * 2st Modified By	:
 * 2st Modifier EMail	:
 * 2st Modified Desc:
 */

namespace Yunchee.Volkswagen.Utility.DataAccess
{
    /// <summary>
    /// 数据库连接字符串 
    /// </summary>
    public class ConnectionString
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public ConnectionString()
        {
        }
        #endregion

        /// <summary>
        /// 客户ID
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// 连接字符串的值
        /// </summary>
        public string Value { get; set; }
    }
}
