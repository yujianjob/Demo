/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/5/28 18:16:04
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
    /// 数据访问： 0905客户菜单关系表 ClientMenus 
    /// 表ClientMenus的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ClientMenusDAO : BaseDAO<BasicUserInfo>, ICRUDable<ClientMenusEntity>, IQueryable<ClientMenusEntity>
    {

        #region 获取分页菜单关系列表

        /// <summary>
        /// 获取分页菜单关系列表
        /// </summary>
        /// <param name="entity">查询条件</param>
        /// <param name="searchText">按菜单名称模糊查询</param>
        /// <param name="clientId">菜单ID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetClientMenusList(PagedQueryEntity entity, string searchText, int clientId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ClientMenus a INNER JOIN dbo.Menus b ON a.MenusID = b.ID AND b.IsDelete = 0";
            entity.QueryFieldName = "b.*";
            //entity.QueryCondition = string.Format(" AND b.ID = {0} ", this.CurrentUserInfo.ClientID);
            entity.QueryCondition += " AND a.IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", clientId);

            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (b.Name LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 更新菜单关系表

        ///<summary>
        /// 更新菜单关系表
        ///</summary>
        ///<param name="clientId">菜单ID</param>
        /// <param name="menusIds">菜单ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateClientMenusIsDelete(int clientId, string menusIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ClientMenus SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (clientId != -1)
            {
                sql.AppendFormat(" AND ClientID = {0} ", clientId);
            }
            if (!string.IsNullOrEmpty(menusIds))
            {
                sql.AppendFormat(" AND MenusID IN ({0}) ", menusIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取指定菜单列表（包含已删除的菜单）

        /// <summary>
        /// 获取指定菜单列表（包含已删除的菜单）
        /// </summary>
        /// <param name="clientId">菜单ID</param>
        /// <returns></returns>
        public DataSet GetClientMenusListByClientId(int clientId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.MenusID FROM dbo.ClientMenus a ");
            sql.AppendFormat(" WHERE a.ClientID = {0} ", clientId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetRegionalMenuNewList(PagedQueryEntity entity, string searchText, int uid)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Client";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND Type = 2 ";
            entity.QueryCondition += string.Format(" AND ParentID = {0} ", uid);
            entity.QueryCondition += " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%' OR Code LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 添加菜单权限列表

        /// <summary>
        /// 添加菜单权限列表
        /// </summary>
        /// <param name="clientId">客户ID</param>
        /// <param name="menuIds">菜单ID集合</param>
        public void AddClientPowers(int clientId, string menuIds)
        {
            var sql = new StringBuilder();

            //sql.AppendFormat(" INSERT INTO dbo.ClientPowers ");
            //sql.AppendFormat(" (ClientID ,PowersID ,CreateBy ,CreateTime ,LastUpdateBy ,LastUpdateTime ,IsDelete) ");
            //sql.AppendFormat(" SELECT  {0} ,{1}, {2} ,GETDATE(), {2}, GETDATE() ,0 ", clientId, powerIds, this.CurrentUserInfo.UserID);

            sql.AppendFormat(" INSERT INTO dbo.ClientPowers ");
            sql.AppendFormat(" ( ClientID ,PowersID ,CreateBy ,CreateTime ,LastUpdateBy ,LastUpdateTime ,IsDelete) ");
            sql.AppendFormat(" SELECT    {0} , a.ID , ", clientId);
            sql.AppendFormat("           {0} , GETDATE() , {0} , GETDATE() , 0 ", this.CurrentUserInfo.UserID);
            sql.AppendFormat(" FROM dbo.Powers a ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.MenusID IN ({0}) ", menuIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 根据区域经销商ID获取菜单
        /// <summary>
        /// 根据区域经销商ID获取菜单
        /// </summary>
        /// <param name="clientId">区域经销商ID</param>
        /// <param name="searchText">名称条件</param>
        /// <returns></returns>
        public DataSet GetClientMenuListByClientId(int clientId, string searchText)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.* FROM dbo.ClientMenus a ");
            sql.AppendFormat(" LEFT JOIN dbo.Menus b ON a.MenusID = b.ID ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.IsDelete = 0 ");
            sql.AppendFormat(" AND a.ClientID = {0} ", clientId);

            if (!string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(" AND (b.Name LIKE '%{0}%') ", searchText);
            }

            sql.AppendFormat(" ORDER BY b.SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region   根据菜单ID获取权限ID

        public DataSet GetClientPowersListByPowersId(int menusID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ID FROM dbo.Powers ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND MenusID = {0} ", menusID);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 更新区域经销商权限关系表

        ///<summary>
        /// 更新区域经销商权限关系表
        ///</summary>
        ///<param name="clientId">区域经销商ID</param>
        /// <param name="menuIds">菜单ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateClientPowersIsDelete(int clientId, string menuIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ClientPowers SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (clientId != -1)
            {
                sql.AppendFormat(" AND ClientID = {0} ", clientId);
            }
            if (!string.IsNullOrEmpty(menuIds))
            {
                sql.AppendFormat(" AND PowersID IN ( SELECT ID FROM dbo.Powers WHERE  MenusID IN ({0})) ", menuIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion
    }
}
