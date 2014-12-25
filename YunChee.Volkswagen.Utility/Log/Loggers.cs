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

using log4net.Config;
using System;
using System.IO;

namespace Yunchee.Volkswagen.Utility.Log
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public static class Loggers
    {
        #region 类构造函数
        static Loggers()
        {
            //初始化日志的配置
            string diskLogConfig = AppDomain.CurrentDomain.BaseDirectory + "Resource\\Config\\loggers.xml";
            if (!System.IO.File.Exists(diskLogConfig))
                diskLogConfig = AppDomain.CurrentDomain.BaseDirectory + "Resource\\Config\\logger.xml";
            if (System.IO.File.Exists(diskLogConfig))
            {//自定义的配置文件                
                XmlConfigurator.Configure(new FileInfo(diskLogConfig)); 
            }
            else
            {//内置的配置文件
                var config = ReflectionUtils.GetEmbeddedResource("Yunchee.Volkswagen.Utility.Log.loggers.xml");
                XmlConfigurator.Configure(config);
            }
            //初始化默认的日志记录器
            DEFAULT = new DefaultLogger();
            //DEFAULT.Debug(new DebugLogInfo() { ClientID = "-1", UserID = "-1", Message = "【diskLogConfig】:[" + diskLogConfig + "]" });
        }
        #endregion

        #region 类属性集
        /// <summary>
        /// 默认的日志记录器
        /// </summary>
        public static IYuncheeLogger DEFAULT { get; private set; }
        #endregion

        #region 工具方法

        #region 记录调试信息
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="pLogInfo">调试信息</param>
        public static void Debug(DebugLogInfo pLogInfo)
        {
            //暂时去掉Debug日志功能,按需要打开
            Loggers.DEFAULT.Debug(pLogInfo);
        }
        #endregion

        #region 记录数据库执行的信息
        /// <summary>
        /// 记录数据库执行的信息
        /// </summary>
        /// <param name="pLogInfo">数据库执行的信息</param>
        public static void Database(DatabaseLogInfo pLogInfo)
        {
            Loggers.DEFAULT.Database(pLogInfo);
        }
        #endregion

        #region 记录异常信息
        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="pLogInfo">异常信息</param>
        public static void Exception(ExceptionLogInfo pLogInfo)
        {
            Loggers.DEFAULT.Exception(pLogInfo);
        }
        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="pUserInfo">当前的用户信息</param>
        /// <param name="pException">异常</param>
        public static void Exception(BasicUserInfo pUserInfo, Exception pException)
        {
            var info = new ExceptionLogInfo(pUserInfo, pException);
            Loggers.DEFAULT.Exception(info);
        }
        #endregion

        #endregion
    }
}
