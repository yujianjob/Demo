/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/24 15:48:54
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
    /// 数据访问： 1007微信菜单表 WMenu 
    /// 表WMenu的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WMenuDAO : BaseDAO<BasicUserInfo>, ICRUDable<WMenuEntity>, IQueryable<WMenuEntity>
    {
        #region 获取微信菜单

        /// <summary>
        /// 获取微信第一级菜单
        /// </summary>
        /// <param name="applicationId">微信公众账号ID</param>
        /// <returns></returns>
        public DataSet GetFirstMenus(int applicationId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.WMenu ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND ApplicationID = {0} AND Level = 1 ", applicationId);
            sql.AppendFormat(" ORDER BY SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        /// <summary>
        /// 获取微信第二级菜单
        /// </summary>
        /// <param name="applicationId">微信公众账号ID</param>
        /// <param name="parentID">上级菜单ID</param>
        /// <returns></returns>
        public DataSet GetSecondMenus(int applicationId, int parentID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.WMenu ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND ApplicationID = {0} AND Level = 2 ", applicationId);
            sql.AppendFormat(" AND ParentId = {0} ", parentID);
            sql.AppendFormat(" ORDER BY SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 根据公共账号ID获取微信菜单

        /// <summary>
        /// 根据公共账号ID获取微信菜单
        /// </summary>
        /// <param name="applicationId">公共账号ID</param>
        /// <returns></returns>
        public DataSet GetWMenuListByWMenuId(int applicationId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.* FROM dbo.WApplication a ");
            sql.AppendFormat(" LEFT JOIN dbo.WMenu b ON a.ID = b.ApplicationID ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.IsDelete = 0 ");
            sql.AppendFormat(" AND b.ApplicationID = {0} ", applicationId);
            sql.AppendFormat(" ORDER BY b.SortIndex ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 删除微信菜单

        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <param name="wMenuId">微信菜单ID集合  "1,2,3"</param>
        public void DeleteWMenus(string wMenuId)
        {
            if (!string.IsNullOrEmpty(wMenuId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.WMenu SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", wMenuId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

    }
}
