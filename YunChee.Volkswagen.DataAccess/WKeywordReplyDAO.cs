/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/7 19:36:22
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
using Yunchee.Volkswagen.Common.Const;

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// ���ݷ��ʣ� 1006�ؼ����Զ��ظ� WKeywordReply 
    /// ��WKeywordReply�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class WKeywordReplyDAO : BaseDAO<BasicUserInfo>, ICRUDable<WKeywordReplyEntity>, IQueryable<WKeywordReplyEntity>
    {
        #region ���ݹ����˺�ID��ȡ΢�Ų˵�

        /// <summary>
        /// ���ݹ����˺�ID��ȡ΢�Ų˵�
        /// </summary>
        /// <param name="applicationId">�����˺�ID</param>
        /// <returns></returns>
        public DataSet GetWKeyWordReplyListById(int applicationId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.* FROM dbo.WApplication a ");
            sql.AppendFormat(" LEFT JOIN dbo.WKeywordReply b ON a.ID = b.ApplicationID ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.IsDelete = 0 ");
            sql.AppendFormat(" AND b.ApplicationID = {0} ", applicationId);
            sql.AppendFormat(" ORDER BY b.SortIndex ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }


        /// <summary>
        /// ���ݹ����˺�ID��ȡ΢�Ų˵�
        /// </summary>
        /// <param name="applicationId">�����˺�ID</param>
        /// <returns></returns>
        public DataSet GetDataWKeyWordById(int applicationId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.* FROM dbo.WApplication a ");
            sql.AppendFormat(" LEFT JOIN dbo.WKeywordReply b ON a.ID = b.ApplicationID ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.IsDelete = 0 ");
            sql.AppendFormat(" AND b.ApplicationID = {0} ", applicationId);
            sql.AppendFormat(" ORDER BY b.ID ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #region ɾ��΢�Ų˵�

        /// <summary>
        /// ɾ��΢�Ų˵�
        /// </summary>
        /// <param name="wMenuId">΢�Ų˵�ID����  "1,2,3"</param>
        public void DeleteKeyWordReply(string wMenuId)
        {
            if (!string.IsNullOrEmpty(wMenuId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.WKeywordReply SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", wMenuId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region ��ȡָ���ͻ��Ĳ˵��б�

        /// <summary>
        /// ��ȡָ���ͻ��Ĳ˵��б�
        /// </summary>
        /// <returns></returns>
        public DataSet GetKeyWordListById(int id)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * from WKeywordReply where 1=1 and IsDelete=0");
       

            if (id>0)
            {
                sql.AppendFormat(" AND ID = {0} ", id);
            }

            sql.AppendFormat(" ORDER BY SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        /// <summary>
        /// ��ȡѡ��ͼ���б�
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetWKeyWordReplyList(PagedQueryEntity entity, int clientID, string loginType, Int32? objectID)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);
            //�����½
            if (loginType == C_ClientType.REGIONAL)
            {
                entity.TableName = "dbo.WNews a  ";
                entity.TableName += " INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                entity.TableName += "INNER JOIN dbo.WNewsMapping c ON c.NewsID=a.ID";
                entity.QueryCondition = " and a.IsDelete = 0 AND c.IsDelete=0 and c.TypeID='" + C_NewsType.KeywordsAutoReply + "' and c.ObjectID=" + objectID + " and (b.ParentID = " + clientID + " OR b.ID=" + clientID + "  )";
                entity.SortField = "c.SortIndex";
                entity.QueryFieldName = "c.id NewsMappingId,c.ObjectID,c.SortIndex MappingSortIndex, a.*";
            }
            //�����̵�½
            if (loginType == C_ClientType.DEALER)
            {
                entity.TableName = "  dbo.WNews a ";
                entity.TableName += " INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                entity.TableName += " INNER JOIN dbo.WNewsMapping c ON c.NewsID=a.ID ";
                entity.QueryCondition = "and a.IsDelete = 0 AND c.IsDelete=0 and  c.TypeID='" + C_NewsType.KeywordsAutoReply + "' and  c.ObjectID=" + objectID + " and ( b.ID = (SELECT c.ParentID FROM dbo.Client c WHERE c.IsDelete = 0 AND c.ID = " + clientID + ") ";
                entity.QueryCondition += "OR b.ID=" + clientID + " )";
                entity.SortField = "c.SortIndex";
                entity.QueryFieldName = "c.id NewsMappingId,c.ObjectID , c.SortIndex MappingSortIndex, a.*";
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);
            return result;
        }

        public void UpdateParentId(WKeywordReplyEntity entiy)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("update WKeywordReply set ParentID=@ParentID where id=@ID");
            SqlParameter[] parameters = 
            {
				new SqlParameter("@ParentID",entiy.ParentID),
                new SqlParameter("@ID",entiy.ID)
            };
            object ob = SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters);
        }


        #endregion
    }


}
