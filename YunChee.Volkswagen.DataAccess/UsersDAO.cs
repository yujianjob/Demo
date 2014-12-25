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
using System.Data.SqlClient;
using System.Text;
using Yunchee.Volkswagen.Common.Enum;
using Yunchee.Volkswagen.DataAccess.Base;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;
using Yunchee.Volkswagen.Utility.ExtensionMethod;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// 数据访问： 0906用户表 Users 
    /// 表Users的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class UsersDAO : BaseDAO<BasicUserInfo>, ICRUDable<UsersEntity>, IQueryable<UsersEntity>
    {
        #region 获取分页用户列表

        /// <summary>
        /// 获取分页用户列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetUsersList(PagedQueryEntity entity, string searchText, int enabled)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Users";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 AND Name <> 'admin' ";
            entity.QueryCondition += string.Format(" AND ClientID = {0} ", this.CurrentUserInfo.ClientID);

            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%' OR ChineseName LIKE '%{0}%' OR EnglishName LIKE '%{0}%') ", searchText);
            }
            if (enabled != -1)
            {
                entity.QueryCondition += string.Format(" AND [Enabled] = {0} ", enabled);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 获取分页区域经销商用户列表

        /// <summary>
        /// 获取分页区域经销商用户列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetRegionalUsersList(PagedQueryEntity entity, string searchText, int clientId, int enabled)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Users";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 AND Name <> 'admin' ";
            entity.QueryCondition += string.Format(" AND ClientID = {0} ", clientId);

            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%' OR ChineseName LIKE '%{0}%' OR EnglishName LIKE '%{0}%') ", searchText);
            }
            if (enabled != -1)
            {
                entity.QueryCondition += string.Format(" AND [Enabled] = {0} ", enabled);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码</param>
        /// <param name="clientCode">客户名</param>
        /// <returns></returns>
        public DataSet LoginIn(string loginName, string password, string clientCode)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.*, ClientType = b.Type FROM dbo.Users a ");
            sql.AppendFormat(" INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.IsDelete = 0 ");
            sql.AppendFormat(" AND a.Name = @Name AND a.Password = @Password ");
            sql.AppendFormat(" AND b.Code = @ClientCode ");

            SqlParameter[] parameters = 
            {
                new SqlParameter{ ParameterName = "@Name", SqlDbType = SqlDbType.NVarChar, Value = loginName},
                new SqlParameter{ ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar, Value = password},
                new SqlParameter{ ParameterName = "@ClientCode", SqlDbType = SqlDbType.NVarChar, Value = clientCode}
            };

            return this.SQLHelper.ExecuteDataset(CommandType.Text, sql.ToString(), parameters);
        }

        #endregion

        #region 批量更新用户启用状态

        /// <summary>
        /// 批量更新用户启用状态
        /// </summary>
        /// <param name="userIds">用户ID集合 "1,2,3"</param>
        /// <param name="enabled">启用状态</param>
        /// <returns></returns>
        public void UpdateUserEnabled(string userIds, E_TrueOrFalse enabled)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" UPDATE dbo.Users SET [Enabled] = {0} ", enabled.GetHashCode());
            sql.AppendFormat(" WHERE ID IN ({0}) ", userIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region 批量删除用户

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="userIds">用户ID集合 "1,2,3"</param>
        public void DeleteUserByUserIds(string userIds)
        {
            if (!string.IsNullOrEmpty(userIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Users SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", userIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 获取指定用户的角色列表（包含已删除的角色）

        /// <summary>
        /// 获取指定用户的角色列表（包含已删除的角色）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public DataSet GetRoleUsersByUserId(int userId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.RoleID FROM dbo.RoleUsers a ");
            sql.AppendFormat(" WHERE a.UserID = {0} ", userId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 更新角色用户表是否删除

        ///<summary>
        /// 更新角色用户表是否删除
        ///</summary>
        ///<param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateRoleUsersIsDelete(int userId, string roleIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RoleUsers SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (userId != -1)
            {
                sql.AppendFormat(" AND UserID = {0} ", userId);
            }
            if (!string.IsNullOrEmpty(roleIds))
            {
                sql.AppendFormat(" AND RoleID IN ({0}) ", roleIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 根据用户ID获取所属角色集合

        /// <summary>
        /// 根据用户ID获取所属角色集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="clientID">区域经销商ID</param>
        /// <returns></returns>
        public DataSet GetRoleListByUserId(int userId, int clientID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.ID, b.Name FROM dbo.RoleUsers a ");
            sql.AppendFormat(" INNER JOIN dbo.Roles b ON a.RoleID = b.ID AND b.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.UserID = {0} ", userId);

            if (clientID != -1)
            {
                sql.AppendFormat(" AND b.ClientID = {0} ", clientID);
            }

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 将用户信息绑定到微信

        /// <summary>
        /// 将用户信息绑定到微信
        /// </summary>
        /// <param name="ID">用户信息ID</param>
        /// <param name="wxOpenID">微信识别号</param>
        /// <returns></returns>
        public void UserAndWxOpenID(int ID,string wxOpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" UPDATE dbo.Users SET  ");
            sql.AppendFormat(" WxOpenId='{0}', ",wxOpenID);
            sql.AppendFormat(" WxId=(SELECT WxId FROM dbo.Customer WHERE WxOpenId='{0}'AND IsDelete=0), ",wxOpenID);
            sql.AppendFormat(" LastUpdateTime='{0}' ",DateTime.Now);
            sql.AppendFormat(" WHERE id={0} AND IsDelete=0 ",ID);
            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }
}
