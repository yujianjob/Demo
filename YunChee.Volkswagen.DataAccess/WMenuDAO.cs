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
    /// ���ݷ��ʣ� 1007΢�Ų˵��� WMenu 
    /// ��WMenu�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class WMenuDAO : BaseDAO<BasicUserInfo>, ICRUDable<WMenuEntity>, IQueryable<WMenuEntity>
    {
        #region ��ȡ΢�Ų˵�

        /// <summary>
        /// ��ȡ΢�ŵ�һ���˵�
        /// </summary>
        /// <param name="applicationId">΢�Ź����˺�ID</param>
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
        /// ��ȡ΢�ŵڶ����˵�
        /// </summary>
        /// <param name="applicationId">΢�Ź����˺�ID</param>
        /// <param name="parentID">�ϼ��˵�ID</param>
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

        #region ���ݹ����˺�ID��ȡ΢�Ų˵�

        /// <summary>
        /// ���ݹ����˺�ID��ȡ΢�Ų˵�
        /// </summary>
        /// <param name="applicationId">�����˺�ID</param>
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

        #region ɾ��΢�Ų˵�

        /// <summary>
        /// ɾ��΢�Ų˵�
        /// </summary>
        /// <param name="wMenuId">΢�Ų˵�ID����  "1,2,3"</param>
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
