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
    /// ���ݷ��ʣ� 0903��ɫȨ�ޱ� RolePowers 
    /// ��RolePowers�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class RolePowersDAO : BaseDAO<BasicUserInfo>, ICRUDable<RolePowersEntity>, IQueryable<RolePowersEntity>
    {
        #region ��ȡָ����ɫ��Ȩ���б�

        /// <summary>
        /// ��ȡָ����ɫ��Ȩ���б�
        /// </summary>
        /// <param name="roleIds">��ɫID���� "1,2,3"</param>
        /// <returns></returns>
        public DataSet GetRolePowersList(string roleIds)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.Name FROM dbo.RolePowers a ");
            sql.AppendFormat(" INNER JOIN dbo.Powers b ON a.PowerID = b.ID ");
            sql.AppendFormat(" INNER JOIN dbo.ClientPowers c ON b.ID = c.PowersID ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.IsDelete = 0 ");
            sql.AppendFormat(" AND c.IsDelete = 0 AND c.ClientID = {0} ", this.CurrentUserInfo.ClientID);

            if (!string.IsNullOrEmpty(roleIds))
            {
                sql.AppendFormat(" AND a.RoleID IN ({0}) ", roleIds);
            }

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡָ����ɫ��Ȩ���б�

        /// <summary>
        /// ��ȡָ����ɫ��Ȩ���б�
        /// </summary>
        /// <param name="roleIds">��ɫID���� "1,2,3"</param>
        /// <param name="clientId">��������ID</param>
        /// <returns></returns>
        public DataSet GetRolePowersListByClientID(string roleIds, string clientId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.Name FROM dbo.RolePowers a ");
            sql.AppendFormat(" INNER JOIN dbo.Powers b ON a.PowerID = b.ID ");
            sql.AppendFormat(" INNER JOIN dbo.ClientPowers c ON b.ID = c.PowersID ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.IsDelete = 0 ");
            sql.AppendFormat(" AND c.IsDelete = 0 AND c.ClientID = {0} ", clientId);

            if (!string.IsNullOrEmpty(roleIds))
            {
                sql.AppendFormat(" AND a.RoleID IN ({0}) ", roleIds);
            }

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡָ��ID�Ľ�ɫȨ������

        ///<summary>
        /// ��ȡָ��ID�Ľ�ɫȨ������
        ///</summary>
        /// <param name="powersIds">Ȩ��ID���� "1,2,3"</param>
        ///<returns>�ܵļ�¼��</returns>
        public int GetRolePowersCountByIds(string powersIds)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT COUNT(1) FROM RolePowers WHERE IsDelete = 0 ");

            if (!string.IsNullOrEmpty(powersIds))
            {
                sql.AppendFormat(" AND PowerID IN ({0}) ", powersIds);
            }

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region ��ȡָ����ɫ��Ȩ���б�������ɾ����Ȩ�ޣ�

        /// <summary>
        /// ��ȡָ����ɫ��Ȩ���б�������ɾ����Ȩ�ޣ�
        /// </summary>
        /// <param name="roleId">��ɫID</param>
        /// <returns></returns>
        public DataSet GetRolePowersListByRoleId(int roleId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.PowerID FROM dbo.RolePowers a ");
            sql.AppendFormat(" WHERE a.RoleID = {0} ", roleId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡָ���ͻ���Ȩ���б�

        /// <summary>
        /// ��ȡָ���ͻ���Ȩ���б�
        /// </summary>
        /// <returns></returns>
        public DataSet GetPowersListByClientId()
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * ");
            sql.AppendFormat(" INTO #tmp1");
            sql.AppendFormat(" FROM dbo.Menus ");
            sql.AppendFormat(" WHERE ParentID IS NULL AND IsDelete = 0");
            sql.AppendFormat(" ORDER BY SortIndex");

            sql.AppendFormat(" SELECT b.ID, b.Name,");
            sql.AppendFormat(" DisplayIndex = ROW_NUMBER() OVER(ORDER BY a.SortIndex, b.SortIndex)");
            sql.AppendFormat(" INTO #tmp2");
            sql.AppendFormat(" FROM dbo.Menus b");
            sql.AppendFormat(" LEFT JOIN #tmp1 a ON b.ParentID = a.ID");
            sql.AppendFormat(" WHERE b.IsDelete = 0");

            sql.AppendFormat(" SELECT  a.*");
            sql.AppendFormat(" FROM dbo.Powers a");
            sql.AppendFormat(" INNER JOIN dbo.ClientPowers b ON a.ID = b.PowersID AND b.IsDelete = 0 ");
            sql.AppendFormat(" INNER JOIN #tmp2 c ON a.MenusID = c.ID");
            sql.AppendFormat(" WHERE   a.IsDelete = 0");
            sql.AppendFormat("         AND b.ClientID = {0} ", this.CurrentUserInfo.ClientID);
            sql.AppendFormat(" ORDER BY c.DisplayIndex");

            //sql.AppendFormat(" SELECT a.* FROM dbo.Powers a ");
            //sql.AppendFormat(" INNER JOIN dbo.ClientPowers b ON a.ID = b.PowersID AND b.IsDelete = 0 ");
            //sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.ClientID = {0} ", this.CurrentUserInfo.ClientID);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡָ�������̵�Ȩ���б�

        /// <summary>
        /// ��ȡָ�������̵�Ȩ���б�
        /// </summary>
        /// <returns></returns>
        public DataSet GetPowersListByClientId(int clientId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * ");
            sql.AppendFormat(" INTO #tmp1");
            sql.AppendFormat(" FROM dbo.Menus ");
            sql.AppendFormat(" WHERE ParentID IS NULL AND IsDelete = 0");
            sql.AppendFormat(" ORDER BY SortIndex");

            sql.AppendFormat(" SELECT b.ID, b.Name,");
            sql.AppendFormat(" DisplayIndex = ROW_NUMBER() OVER(ORDER BY a.SortIndex, b.SortIndex)");
            sql.AppendFormat(" INTO #tmp2");
            sql.AppendFormat(" FROM dbo.Menus b");
            sql.AppendFormat(" LEFT JOIN #tmp1 a ON b.ParentID = a.ID");
            sql.AppendFormat(" WHERE b.IsDelete = 0");

            sql.AppendFormat(" SELECT  a.*");
            sql.AppendFormat(" FROM dbo.Powers a");
            sql.AppendFormat(" INNER JOIN dbo.ClientPowers b ON a.ID = b.PowersID AND b.IsDelete = 0 ");
            sql.AppendFormat(" INNER JOIN #tmp2 c ON a.MenusID = c.ID");
            sql.AppendFormat(" WHERE   a.IsDelete = 0");
            sql.AppendFormat("         AND b.ClientID = {0} ", clientId);
            sql.AppendFormat(" ORDER BY c.DisplayIndex");

            //sql.AppendFormat(" SELECT a.* FROM dbo.Powers a ");
            //sql.AppendFormat(" INNER JOIN dbo.ClientPowers b ON a.ID = b.PowersID AND b.IsDelete = 0 ");
            //sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.ClientID = {0} ", clientId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ���½�ɫȨ�ޱ��Ƿ�ɾ��

        ///<summary>
        /// ���½�ɫȨ�ޱ��Ƿ�ɾ��
        ///</summary>
        ///<param name="roleId">��ɫID</param>
        /// <param name="powersIds">Ȩ��ID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns></returns>
        public void UpdateRolePowersIsDelete(int roleId, string powersIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RolePowers SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE RoleID = '{0}' ", roleId);

            if (!string.IsNullOrEmpty(powersIds))
            {
                sql.AppendFormat(" AND PowerID IN ({0}) ", powersIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion
    }
}
