using System.Configuration;
using Yunchee.Volkswagen.Utility.DataAccess;

namespace Yujian.DLL.Base
{
    /// <summary>
    /// 配置信息 
    /// </summary>
    internal static class ConfigInfo
    {
        /// <summary>
        /// 当前的数据库连接字符串管理者
        /// </summary>
        public static readonly DefaultConnectionStringManager CURRENT_CONNECTION_STRING_MANAGER = null;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ConfigInfo()
        {
            CURRENT_CONNECTION_STRING_MANAGER = new DefaultConnectionStringManager();
            CURRENT_CONNECTION_STRING_MANAGER.Add(new ConnectionString() { Value =ConfigurationManager.AppSettings["SqlConn"] }, true);
        }
    }
}
