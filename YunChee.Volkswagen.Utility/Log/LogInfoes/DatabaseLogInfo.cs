/*
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

using System;
using System.Collections.Generic;
using System.Text;

using Yunchee.Volkswagen.Utility;

namespace Yunchee.Volkswagen.Utility.Log
{
    /// <summary>
    /// 数据库日志信息 
    /// </summary>
    [Serializable]
    public class DatabaseLogInfo:BaseLogInfo
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public DatabaseLogInfo()
        {
            this.StackTrances = SystemUtils.GetCurrentStackTraces();
            this.Location = this.TryGetLocationFromStackTrace();
        }
        #endregion

        /// <summary>
        /// T-SQL语句
        /// </summary>
        public TSQL TSQL { get; set; }
    }
}
