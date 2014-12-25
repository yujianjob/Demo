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
    /// ���ݷ��ʣ� 0906�û��� Users 
    /// ��Users�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class UsersDAO : BaseDAO<BasicUserInfo>, ICRUDable<UsersEntity>, IQueryable<UsersEntity>
    {
        #region ��ȡ��ҳ�û��б�

        /// <summary>
        /// ��ȡ��ҳ�û��б�
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

        #region ��ȡ��ҳ���������û��б�

        /// <summary>
        /// ��ȡ��ҳ���������û��б�
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

        #region ��¼

        /// <summary>
        /// ��¼
        /// </summary>
        /// <param name="loginName">��¼��</param>
        /// <param name="password">����</param>
        /// <param name="clientCode">�ͻ���</param>
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

        #region ���������û�����״̬

        /// <summary>
        /// ���������û�����״̬
        /// </summary>
        /// <param name="userIds">�û�ID���� "1,2,3"</param>
        /// <param name="enabled">����״̬</param>
        /// <returns></returns>
        public void UpdateUserEnabled(string userIds, E_TrueOrFalse enabled)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" UPDATE dbo.Users SET [Enabled] = {0} ", enabled.GetHashCode());
            sql.AppendFormat(" WHERE ID IN ({0}) ", userIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region ����ɾ���û�

        /// <summary>
        /// ����ɾ���û�
        /// </summary>
        /// <param name="userIds">�û�ID���� "1,2,3"</param>
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

        #region ��ȡָ���û��Ľ�ɫ�б�������ɾ���Ľ�ɫ��

        /// <summary>
        /// ��ȡָ���û��Ľ�ɫ�б�������ɾ���Ľ�ɫ��
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <returns></returns>
        public DataSet GetRoleUsersByUserId(int userId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.RoleID FROM dbo.RoleUsers a ");
            sql.AppendFormat(" WHERE a.UserID = {0} ", userId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ���½�ɫ�û����Ƿ�ɾ��

        ///<summary>
        /// ���½�ɫ�û����Ƿ�ɾ��
        ///</summary>
        ///<param name="userId">�û�ID</param>
        /// <param name="roleIds">��ɫID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
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

        #region �����û�ID��ȡ������ɫ����

        /// <summary>
        /// �����û�ID��ȡ������ɫ����
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="clientID">��������ID</param>
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

        #region ���û���Ϣ�󶨵�΢��

        /// <summary>
        /// ���û���Ϣ�󶨵�΢��
        /// </summary>
        /// <param name="ID">�û���ϢID</param>
        /// <param name="wxOpenID">΢��ʶ���</param>
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
