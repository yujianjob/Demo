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

using System.Configuration;

namespace Yunchee.Volkswagen.Utility.DataAccess
{
    /// <summary>
    /// 直接的数据库连接字符串管理者 
    /// </summary>
    public class DirectConnectionStringManager : IConnectionStringManager
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public DirectConnectionStringManager(string pConnectionString)
        {
            this.ConnectionString = pConnectionString;
        }
        #endregion

        #region 属性集
        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string ConnectionString { get; set; }
        #endregion

        #region IConnectionStringManager 成员
        /// <summary>
        /// 根据用户信息获取数据库连接字符串
        /// </summary>
        /// <param name="pUserInfo">用户信息</param>
        /// <returns></returns>
        public string GetConnectionStringBy(BasicUserInfo pUserInfo)
        {
            return string.IsNullOrEmpty(this.ConnectionString) ? ConfigurationManager.AppSettings["conn"] : this.ConnectionString;
        }
        #endregion
    }
}
