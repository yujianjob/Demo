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

namespace Yunchee.Volkswagen.Utility.Log
{
    /// <summary>
    /// Yunchee日志记录器接口 
    /// </summary>
    public interface IYuncheeLogger
    {
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="pLogInfo">日志信息</param>
        void Debug(DebugLogInfo pLogInfo);

        /// <summary>
        /// 记录对数据库的操作信息
        /// </summary>
        /// <param name="pLogInfo">日志信息</param>
        void Database(DatabaseLogInfo pLogInfo);

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="pLogInfo">日志信息</param>
        void Exception(ExceptionLogInfo pLogInfo);
    }
}
