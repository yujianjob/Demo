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

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// 数据访问： 1008关键词记录表 WKeywordRecord 
    /// 表WKeywordRecord的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WKeywordRecordDAO : BaseDAO<BasicUserInfo>, ICRUDable<WKeywordRecordEntity>, IQueryable<WKeywordRecordEntity>
    {
        #region 获取分页权限区域列表

        /// <summary>
        /// 获取分页权限区域列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetKeyRecordList(PagedQueryEntity pageEntity,WKeywordRecordEntity recordEntity, WKeywordReplyEntity replyEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(new BasicUserInfo() { ClientID=1, UserID=1});

            pageEntity.TableName = " dbo.WKeywordRecord a ";
            pageEntity.TableName += " LEFT JOIN dbo.WKeywordReply b ON a.KeywordID = b.ID AND b.IsDelete = 0 AND  b.Enabled=1";

            pageEntity.QueryFieldName = " a.[ID],a.[KeywordID],a.[WxId] ,a.[WxOpenId],a.[FirstKeywordID] ";
            pageEntity.QueryFieldName+= ", b.[ID] as bid ,b.[ParentID],b.[Keyword],b.[ReplyTypeID] ,b.[Text] ,b.[Level]";
            pageEntity.QueryCondition = " AND a.IsDelete = 0 ";

            if (!string.IsNullOrEmpty(replyEntity.Keyword ))
            {
                pageEntity.QueryCondition += string.Format(" AND b.Keyword like '%{0}%' ", replyEntity.Keyword);
            }
            if (!string.IsNullOrEmpty(recordEntity.WxId))
            {
                pageEntity.QueryCondition += string.Format(" AND a.WxId = '{0}' ", recordEntity.WxId);
            }
            if (!string.IsNullOrEmpty(recordEntity.WxOpenId))
            {
                pageEntity.QueryCondition += string.Format(" AND a.WxOpenId = '{0}' ", recordEntity.WxOpenId);
            }
            if (replyEntity.ApplicationID!=-1)
            {
                pageEntity.QueryCondition += string.Format(" AND b.ApplicationID = {0} ", replyEntity.ApplicationID);
            }
            //if (replyEntity.Level != -1)
            //{
            //    pageEntity.QueryCondition += string.Format(" AND b.Level={0}) ", replyEntity.Level);
            //}
            //if (replyEntity.ParentID != -1)
            //{
            //    pageEntity.QueryCondition += string.Format(" AND b.ParentID={0}) ", replyEntity.ParentID);
            //}
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;
        }

        #endregion
    }
}
