/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/14 10:48:36
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
    /// ���ݷ��ʣ� 0809ϵͳ�����  SystemNotice 
    /// ��SystemNotice�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class SystemNoticeDAO : BaseDAO<BasicUserInfo>, ICRUDable<SystemNoticeEntity>, IQueryable<SystemNoticeEntity>
    {
        #region ��ȡ��ҳȨ���б�

        /// <summary>
        /// ��ȡ��ҳȨ���б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetSystemNoticeList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.SystemNotice";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Title LIKE '%{0}%') ", searchText);
            }
            entity.SortField = " SortIndex";
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region ɾ����������̹��ں�
        /// <summary>
        /// ɾ��������߹��ں�
        /// </summary>
        /// <param name="quesIds"></param>
        public void DeleteAystemNotice(string quesIds)
        {
            if (!string.IsNullOrEmpty(quesIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.SystemNotice SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", quesIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }


        #endregion

        #region ��ѯ�ظ�����
        public DataTable querysystemNotice(string inputName, int systemNoticeId)
        {
            var sql=new StringBuilder();
            sql.AppendFormat("select * from SystemNotice where Title='{0}'", inputName);
            sql.AppendFormat(" and ID!={0} and IsDelete=0  ", systemNoticeId);

           DataSet ds=   SQLHelper.ExecuteDataset(sql.ToString());
           return ds.Tables[0];
           
        }
        #endregion
    }
}
