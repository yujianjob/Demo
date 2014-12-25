/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/10 17:48:52
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
    /// ���ݷ��ʣ� 1110Ӧ�ó������ñ� ServiceConfig 
    /// ��ServiceConfig�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ServiceConfigDAO : BaseDAO<BasicUserInfo>, ICRUDable<ServiceConfigEntity>, IQueryable<ServiceConfigEntity>
    {

        #region ��ȡ������Ϣ�ľ���ֵ(ҵ������ֵ,״ֵ̬)

        /// <summary>
        /// ��ȡ������Ϣ�ľ���ֵ
        /// </summary>
        /// <param name="BusinessType">ҵ������ֵ</param>
        /// <param name="StatuValue">״ֵ̬</param>
        /// <returns></returns>
        public DataSet GetConfigValue(string businessType, int applicationID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM ServiceConfig ");
            sql.AppendFormat(" WHERE   ");
            if (businessType=="3")
                sql.AppendFormat(" Code='TestDriveBegin' ");
            if (businessType == "1")
                sql.AppendFormat(" Code='MaintanceBegin' ");
            if (businessType == "2")
                sql.AppendFormat(" Code='RepairBegin' ");
            if (businessType == "4")
                sql.AppendFormat(" Code='AskPriceBegin' ");
            sql.AppendFormat(" AND ClientID={0} ", applicationID);
            sql.AppendFormat(" AND IsDelete=0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ���������б�

        /// <summary>
        /// ��ȡ���������б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetServiceConfigList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ServiceConfig";
            entity.QueryFieldName = " * ";
            entity.QueryCondition = string.Format(" AND ClientID = {0} ", this.CurrentUserInfo.ClientID);
            entity.QueryCondition += " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND Name LIKE '%{0}%' ", searchText);
            }
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region ��ȡ�����б�

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetServiceConfigGroup(PagedQueryEntity entity, string searchText, int applicationID, string typeCode)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ServiceConfig";
            entity.QueryFieldName = " * ";
            entity.QueryCondition = string.Format(" AND ApplicationID = {0} ", applicationID);
            entity.QueryCondition = string.Format(" AND TypeCode = '{0}' ", typeCode);
            entity.QueryCondition += " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND Name LIKE '%{0}%' ", searchText);
            }
            entity.SortField =  entity.SortField;
            entity.SortField = " SortIndex";

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region ��ȡ��������

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public string GetNumbers(int applicationID, string strCode)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@" SELECT COUNT(*) FROM dbo.ServiceConfig  WHERE TypeCode='{0}' AND ApplicationID={1} AND IsDelete=0", strCode, applicationID);

            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion

        #region ɾ����������

        /// <summary>
        /// ɾ����������
        /// </summary>
        /// <param name="serviceConfigId">����ID</param>
        public void DeleteServiceConfig(string serviceConfigId)
        {
            if (!string.IsNullOrEmpty(serviceConfigId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.ServiceConfig SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", serviceConfigId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion
    }
}
