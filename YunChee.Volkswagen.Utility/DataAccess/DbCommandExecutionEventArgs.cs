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
using System.Data;

namespace Yunchee.Volkswagen.Utility.DataAccess
{
    /// <summary>
    /// SqlCommand执行的事件参数 
    /// </summary>
    public abstract class DbCommandExecutionEventArgs:System.EventArgs
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public DbCommandExecutionEventArgs()
        {
        }
        #endregion

        /// <summary>
        /// 用户信息
        /// </summary>
        public BasicUserInfo UserInfo { get; set; }

        /// <summary>
        /// 被执行的SQL命令
        /// </summary>
        public IDbCommand Command { get; set; }

        /// <summary>
        /// SQl命令的执行时间
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }
    }
}
