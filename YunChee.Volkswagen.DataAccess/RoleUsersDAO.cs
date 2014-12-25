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
    /// ���ݷ��ʣ� 0905��ɫ�û���ϵ�� RoleUsers 
    /// ��RoleUsers�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class RoleUsersDAO : BaseDAO<BasicUserInfo>, ICRUDable<RoleUsersEntity>, IQueryable<RoleUsersEntity>
    {
        #region ��ȡָ����ɫID�Ľ�ɫ�û�����

        ///<summary>
        /// ��ȡָ����ɫID�Ľ�ɫ�û�����
        ///</summary>
        /// <param name="roleId">��ɫID</param>
        ///<returns>�ܵļ�¼��</returns>
        public int GetRoleUsersCountByRoleId(int roleId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT COUNT(1) FROM RoleUsers WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND RoleID = {0} ", roleId);

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region ��ȡ��ҳ��ɫ�û��б�

        /// <summary>
        /// ��ȡ��ҳ��ɫ�û��б�
        /// </summary>
        /// <param name="entity">��ѯ����</param>
        /// <param name="searchText">���û�����ģ����ѯ</param>
        /// <param name="roleId">��ɫID</param>
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

        #region ���½�ɫ�û����Ƿ�ɾ��

        ///<summary>
        /// ���½�ɫ�û����Ƿ�ɾ��
        ///</summary>
        ///<param name="roleId">��ɫID</param>
        /// <param name="usersIds">�û�ID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
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

        #region ��ȡ��ҳ�û��б�

        /// <summary>
        /// ��ȡ��ҳ�û��б�
        /// </summary>
        /// <param name="entity">��ѯ����</param>
        /// <param name="searchText">���û�����ģ����ѯ</param>
        /// <param name="roleId">��ɫID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetUsersList(PagedQueryEntity entity, string searchText, int roleId, int clientId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Users a";
            entity.QueryFieldName = "a.*";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", clientId);//this.CurrentUserInfo.ClientID
            // �ų��Ѿ����ڱ���ɫ���û�
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

        #region ��ȡָ����ɫ���û��б�������ɾ�����û���

        /// <summary>
        /// ��ȡָ����ɫ���û��б�������ɾ�����û���
        /// </summary>
        /// <param name="roleId">��ɫID</param>
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
