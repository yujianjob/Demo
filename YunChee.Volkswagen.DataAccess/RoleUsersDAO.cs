/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/4/17 13:36:57
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
using System.Text;
using Yunchee.Volkswagen.DataAccess.Base;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;
using Yunchee.Volkswagen.Utility.ExtensionMethod;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// 数据访问： 0905角色用户关系表 RoleUsers 
    /// 表RoleUsers的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class RoleUsersDAO : BaseDAO<BasicUserInfo>, ICRUDable<RoleUsersEntity>, IQueryable<RoleUsersEntity>
    {
        #region 获取指定角色ID的角色用户总数

        ///<summary>
        /// 获取指定角色ID的角色用户总数
        ///</summary>
        /// <param name="roleId">角色ID</param>
        ///<returns>总的记录数</returns>
        public int GetRoleUsersCountByRoleId(int roleId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT COUNT(1) FROM RoleUsers WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND RoleID = {0} ", roleId);

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取分页角色用户列表

        /// <summary>
        /// 获取分页角色用户列表
        /// </summary>
        /// <param name="entity">查询条件</param>
        /// <param name="searchText">按用户名称模糊查询</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetRoleUsersList(PagedQueryEntity entity, string searchText, int roleId, int clientId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.RoleUsers a INNER JOIN dbo.Users b ON a.UserID = b.ID AND b.IsDelete = 0";
            entity.QueryFieldName = "b.*";
            entity.QueryFieldName += " , BeOnDutys = CASE ISNULL(BeOnDuty, '0') WHEN '0' THEN 'false' WHEN '1' THEN 'true' END ";
            entity.QueryCondition = string.Format(" AND b.ClientID = {0} ", clientId);
            entity.QueryCondition += " AND b.Name <> 'admin' AND a.IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND a.RoleID = {0} ", roleId);

            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (b.Name LIKE '%{0}%' OR b.ChineseName LIKE '%{0}%' OR b.EnglishName LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 更新角色用户表是否删除

        ///<summary>
        /// 更新角色用户表是否删除
        ///</summary>
        ///<param name="roleId">角色ID</param>
        /// <param name="usersIds">用户ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateRoleUsersIsDelete(int roleId, string usersIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RoleUsers SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (roleId != -1)
            {
                sql.AppendFormat(" AND RoleID = {0} ", roleId);
            }
            if (!string.IsNullOrEmpty(usersIds))
            {
                sql.AppendFormat(" AND UserID IN ({0}) ", usersIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取分页用户列表

        /// <summary>
        /// 获取分页用户列表
        /// </summary>
        /// <param name="entity">查询条件</param>
        /// <param name="searchText">按用户名称模糊查询</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetUsersList(PagedQueryEntity entity, string searchText, int roleId, int clientId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Users a";
            entity.QueryFieldName = "a.*";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", clientId);//this.CurrentUserInfo.ClientID
            // 排除已经属于本角色的用户
            entity.QueryCondition += string.Format(" AND NOT EXISTS(SELECT 1 FROM dbo.RoleUsers b WHERE b.IsDelete = 0 AND b.RoleID = {0} AND b.UserID = a.ID) ", roleId);

            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (a.Name LIKE '%{0}%' OR a.ChineseName LIKE '%{0}%' OR a.EnglishName LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 获取指定角色的用户列表（包含已删除的用户）

        /// <summary>
        /// 获取指定角色的用户列表（包含已删除的用户）
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public DataSet GetRoleUsersListByRoleId(int roleId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.UserID FROM dbo.RoleUsers a ");
            sql.AppendFormat(" WHERE a.RoleID = {0} ", roleId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
