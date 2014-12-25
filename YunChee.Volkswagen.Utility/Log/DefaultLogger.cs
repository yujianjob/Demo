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

using log4net;
using System;
using System.Net;

namespace Yunchee.Volkswagen.Utility.Log
{
    /// <summary>
    /// 默认的日志记录器
    /// <remarks>
    /// <para>1.该类仅能在log4net初始化完毕后才能new.</para>
    /// </remarks>
    /// </summary>
    internal class DefaultLogger:IYuncheeLogger
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public DefaultLogger()
        {
            this.InternalDebugLoger = LogManager.GetLogger("DefaultLogger.Debug");
            this.InternalDatabaseLoger = LogManager.GetLogger("DefaultLogger.Database");
            this.InternalExceptionLoger = LogManager.GetLogger("DefaultLogger.Exception");
            try
            {
                this._hostName = Dns.GetHostName();
            }
            catch (Exception ex)
            {
                this._hostName = string.Empty;
                Exception(new ExceptionLogInfo(ex));
            }
        }
        #endregion

        #region 属性集
        /// <summary>
        /// 内部的Debug日志记录器
        /// </summary>
        public ILog InternalDebugLoger { get; set; }
        /// <summary>
        /// 内部的Exception日志记录器
        /// </summary>
        public ILog InternalExceptionLoger { get; set; }
        /// <summary>
        /// 内部的Database日志记录器
        /// </summary>
        public ILog InternalDatabaseLoger { get; set; }

        private string _hostName;
        #endregion

        #region IYuncheeLogger 成员
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="pLogInfo">日志信息</param>
        public void Debug(DebugLogInfo pLogInfo)
        {
            if (pLogInfo!=null)
            {
                log4net.ThreadContext.Properties["ClientID"] = pLogInfo.ClientID;
                log4net.ThreadContext.Properties["UserID"] = pLogInfo.UserID;
                log4net.ThreadContext.Properties["Location"] = pLogInfo.Location;
                log4net.ThreadContext.Properties["StackTrace"] = pLogInfo.StackTrances;
                this.InternalDebugLoger.Debug(pLogInfo);
            }
        }

        /// <summary>
        /// 记录对数据库的操作信息
        /// </summary>
        /// <param name="pLogInfo">日志信息</param>
        public void Database(DatabaseLogInfo pLogInfo)
        {
            if (pLogInfo != null)
            {
                log4net.ThreadContext.Properties["ClientID"] = pLogInfo.ClientID;
                log4net.ThreadContext.Properties["UserID"] = pLogInfo.UserID;
                log4net.ThreadContext.Properties["Location"] = pLogInfo.Location;
                log4net.ThreadContext.Properties["StackTrace"] = pLogInfo.StackTrances;
                this.InternalDatabaseLoger.Debug(pLogInfo);
            }
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="pLogInfo">日志信息</param>
        public void Exception(ExceptionLogInfo pLogInfo)
        {
            if (pLogInfo != null)
            {
                log4net.ThreadContext.Properties["ClientID"] = pLogInfo.ClientID;
                log4net.ThreadContext.Properties["UserID"] = pLogInfo.UserID;
                
                log4net.ThreadContext.Properties["Location"] = "Host:【" + this._hostName + "】;AssemblyLocation:【"+System.Reflection.Assembly.GetExecutingAssembly().Location+"】;" + pLogInfo.Location;
                log4net.ThreadContext.Properties["StackTrace"] = pLogInfo.StackTrances;
                this.InternalExceptionLoger.Debug(pLogInfo);
            }
        }
        #endregion
    }
}
