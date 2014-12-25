/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/5/28 18:58:54
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
    /// ���ݷ��ʣ� 0905�ͻ�Ȩ�޹�ϵ�� ClientPowers 
    /// ��ClientPowers�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ClientPowersDAO : BaseDAO<BasicUserInfo>, ICRUDable<ClientPowersEntity>, IQueryable<ClientPowersEntity>
    {
        #region ��ȡ��������û�е�Ȩ���б�

        /// <summary>
        /// ��ȡ��������û�е�Ȩ���б�
        /// </summary>
        /// <param name="entity">��ѯ����</param>
        /// <param name="searchText">��Ȩ������ģ����ѯ</param>
        /// <param name="powersId">��������ID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetPowersList(PagedQueryEntity entity, string searchText, int powersId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

           // SELECT a.* FROM dbo.Powers a WHERE a.IsDelete=0
           //AND NOT EXISTS(SELECT * FROM dbo.ClientPowers b WHERE b.IsDelete=0 AND b.ClientID=1 AND b.PowersID=a.ID) 

            entity.TableName = "dbo.Powers a";
            entity.QueryFieldName = "a.*";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            // �ų��Ѿ����ڱ��������̵�Ȩ��
            entity.QueryCondition += string.Format(" AND NOT EXISTS(SELECT * FROM dbo.ClientPowers b WHERE b.IsDelete=0 AND b.ClientID={0} AND b.PowersID=a.ID ) ", powersId);


            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (a.Name LIKE '%{0}%' OR a.GroupName LIKE '%{0}%' OR a.Title LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region ��ȡ�������̵�Ȩ���б�

        /// <summary>
        /// ��ȡ�������̵�Ȩ���б�
        /// </summary>
        /// <param name="entity">��ѯ����</param>
        /// <param name="searchText">��Ȩ������ģ����ѯ</param>
        /// <param name="powersId">��������ID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetIsPowersList(PagedQueryEntity entity, string searchText, int powersId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            //SELECT b.* FROM dbo.ClientPowers a, dbo.Powers b WHERE b.ID=a.PowersID
            //AND a.IsDelete=0 AND b.IsDelete=0 AND a.ClientID=2

            entity.TableName = "dbo.ClientPowers a INNER JOIN dbo.Powers b ON a.PowersID = b.ID AND b.IsDelete = 0";
            entity.QueryFieldName = "b.*";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            // ���������̵�Ȩ��
            entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", powersId);


            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (b.Name LIKE '%{0}%' OR b.GroupName LIKE '%{0}%' OR b.Title LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region ������������Ȩ�޹�ϵ��

        ///<summary>
        /// ������������Ȩ�޹�ϵ��
        ///</summary>
        ///<param name="clientId">��������ID</param>
        /// <param name="powersIds">Ȩ��ID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
        public void UpdateClientPowersIsDelete(int clientId, string powersIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ClientPowers SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (clientId != -1)
            {
                sql.AppendFormat(" AND ClientID = {0} ", clientId);
            }
            if (!string.IsNullOrEmpty(powersIds))
            {
                sql.AppendFormat(" AND PowersID IN ({0}) ", powersIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region ��ȡָ��Ȩ���б�������ɾ����Ȩ�ޣ�

        /// <summary>
        /// ��ȡָ��Ȩ���б�������ɾ����Ȩ�ޣ�
        /// </summary>
        /// <param name="clientId">��������ID</param>
        /// <returns></returns>
        public DataSet GetClientPowersListByPowersId(int clientId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.PowersID FROM dbo.ClientPowers a ");
            sql.AppendFormat(" WHERE a.ClientID = {0} ", clientId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
