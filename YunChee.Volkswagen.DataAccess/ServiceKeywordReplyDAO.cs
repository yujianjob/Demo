/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/24 14:05:40
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
    /// 数据访问： 1114多客服关键词回复表 ServiceKeywordReply 
    /// 表ServiceKeywordReply的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ServiceKeywordReplyDAO : BaseDAO<BasicUserInfo>, ICRUDable<ServiceKeywordReplyEntity>, IQueryable<ServiceKeywordReplyEntity>
    {
        #region 获取关键词菜单列表

        /// <summary>
        /// 获取关键词菜单列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetServiceKeywordReplyList()
        {
            var sql = new StringBuilder();
            //sql.AppendFormat(" SELECT * FROM dbo.ServiceKeywordReply WHERE IsDelete=0 ");
            sql.AppendFormat(" SELECT a.ID,a.Keyword,a.Text,a.Type ,a.SortIndex ,a.ParentID ,KeywordType=b.Name ");
            sql.AppendFormat(" FROM   dbo.ServiceKeywordReply a ,dbo.BasicData b ");
            sql.AppendFormat(" WHERE ");
            sql.AppendFormat(" a.Type = b.Value AND b.IsDelete = 0 AND b.TypeCode = 'ServiceKeywordType' ");
            sql.AppendFormat(" AND a.IsDelete=0 ");
            sql.AppendFormat(" AND a.ClientID = {0} ", this.CurrentUserInfo.ClientID);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取分页列表

        /// <summary>
        /// 获取分页列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetServiceKeywordReplys(PagedQueryEntity pageEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = "dbo.ServiceKeywordReply a";
            pageEntity.TableName += " LEFT JOIN dbo.BasicData b ON a.Type = b.Value AND b.IsDelete=0  AND b.TypeCode='ServiceKeywordType' ";
            pageEntity.QueryFieldName = "a.*";
            pageEntity.QueryFieldName += " , TypeName = b.Name ";
            pageEntity.QueryCondition = " AND a.IsDelete = 0 ";
            pageEntity.SortField = "a." + pageEntity.SortField;
            pageEntity.SortField = " a.SortIndex";

            pageEntity.QueryCondition += string.Format(" AND a.ClientID = {0} ", this.CurrentUserInfo.ClientID);

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }


        #endregion

        #region 删除快捷回复消息指令

        /// <summary>
        /// 删除快捷回复消息指令
        /// </summary>
        /// <param name="serviceKeywordReplyId">快捷回复消息ID 集合</param>
        public void DeleteServiceKeywordReply(string serviceKeywordReplyId)
        {
            if (!string.IsNullOrEmpty(serviceKeywordReplyId))
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" UPDATE dbo.ServiceKeywordReply SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", serviceKeywordReplyId);
                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }


        #endregion
    }
}
