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
    /// 数据库连接字符串管理者接口 
    /// </summary>
    public interface IConnectionStringManager
    {
        /// <summary>
        /// 根据用户信息获取数据库连接字符串
        /// </summary>
        /// <param name="pUserInfo">用户信息</param>
        /// <returns></returns>
        string GetConnectionStringBy(BasicUserInfo pUserInfo);
    }
}
