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
    /// ���ݷ��ʣ� 1002ͼ���ز� WNews 
    /// ��WNews�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class WNewsDAO : BaseDAO<BasicUserInfo>, ICRUDable<WNewsEntity>, IQueryable<WNewsEntity>
    {
        #region ��ȡ��ҳȨ���б�

        /// <summary>
        /// ��ȡ��ҳȨ���б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetWNewsList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.WNews";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND ClientID = {0} ", this.CurrentUserInfo.ClientID);
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

        #region ɾ��ͼ���ز�

        /// <summary>
        /// ɾ��ͼ���ز�
        /// </summary>
        /// <param name="quesIds">ͼ���ز�ID����  "1,2,3"</param>
        public void DeleteWNews(string wnewsID)
        {
            if (!string.IsNullOrEmpty(wnewsID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.WNews SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", wnewsID);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region ����ͼ���ز�ID��ȡͼ����Ϣ

        /// <summary>
        /// ����ͼ���ز�ID��ȡͼ����Ϣ
        /// </summary>
        public DataSet GetWNewsListId(int NewsId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.WNews WHERE ID = {0} AND IsDelete = 0", NewsId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region �������²����IDֵ

        ///<summary>
        /// ����Ϊĳ���Ự���������е�ָ�������ɵ����±�ʶֵ
        ///</summary>
        ///<returns>����</returns>
        public int GetIdentCurrent(string tableName)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT IDENT_CURRENT('{0}') ", tableName);

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion


        #region ��ȡWeixinID

        /// <summary>
        /// ��ȡWeixinID
        /// </summary>
        public string GetWApplicationWeixinID(int clientId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@" SELECT TOP 1 WeixinID FROM WApplication WHERE ClientID={0} AND IsDelete=0", clientId);
            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion
    }
}
