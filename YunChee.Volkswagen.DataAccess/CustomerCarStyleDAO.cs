/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/14 18:49:34
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
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.Entity;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using Yunchee.Volkswagen.Utility.DataAccess;
using Yunchee.Volkswagen.Utility.Log;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility.DataAccess.Query;
using Yunchee.Volkswagen.DataAccess.Base;

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// 数据访问： 0104客户车款关系表 CustomerCarStyle 
    /// 表CustomerCarStyle的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CustomerCarStyleDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerCarStyleEntity>, IQueryable<CustomerCarStyleEntity>
    {
        #region 获取车型对比的数量(通过微信用户标识,车款ID)

        /// <summary>
        /// 获取车型对比的数量
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        /// <param name="CarStyleID">车款ID</param>
        public DataSet GetCountByRequest(string OpenID, int CarStyleID)
        { 
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.CustomerCarStyle a ");
            sql.AppendFormat(" WHERE a.CarStyleID={0}  ", CarStyleID);
            sql.AppendFormat(" AND a.CustomerID=(SELECT b.ID FROM dbo.Customer b WHERE  b.IsDelete = 0 AND b.WxOpenId='{0}') ", OpenID);
            sql.AppendFormat(" AND  a.IsDelete = 0");
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region 更改车型对比的状态

        /// <summary>
        /// 更改车型对比的状态
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        /// <param name="CarStyleID">车款ID</param>
        public void UpdateByRequest(string OpenID, int CarStyleID)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.CustomerCarStyle SET IsDelete = 0, ");
                sql.AppendFormat("  LastUpdateTime = '{0}' ",  DateTime.Now);
                sql.AppendFormat(" WHERE CarStyleID={0} ", CarStyleID);
                sql.AppendFormat(" AND CustomerID=(SELECT b.ID FROM dbo.Customer b WHERE b.IsDelete = 0 AND b.WxOpenId='{0}') ", OpenID);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 删除车型对比

        /// <summary>
        /// 删除车型对比
        /// <param name="OpenID">微信用户标识</param>
        /// <param name="CarStyleIDs">车款ID集合</param>
        public void DeleteByRequest(string OpenID, int[] CarStyleIDs)
        {
            if (!string.IsNullOrEmpty(OpenID) && CarStyleIDs.Length>0)
            {
                string str = null;
                for (int i = 0; i < CarStyleIDs.Length; i++)
                {
                    str += CarStyleIDs[i] + ",";          
                }
                str = str.Substring(0, str.Length - 1);
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.CustomerCarStyle SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateTime = '{0}' ",  DateTime.Now);
                sql.AppendFormat(" WHERE CarStyleID IN ({0}) ", str);
                sql.AppendFormat(" AND CustomerID=(SELECT b.ID FROM dbo.Customer b WHERE b.WxOpenId='{0}') ", OpenID);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion
    }
}
