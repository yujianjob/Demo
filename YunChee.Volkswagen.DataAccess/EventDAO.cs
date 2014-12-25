/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/9 20:13:46
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
    /// 数据访问： 0201活动表 Event 
    /// 表Event的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class EventDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventEntity>, IQueryable<EventEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetEventList(PagedQueryEntity entity, EventEntity eventEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Event a";
            entity.TableName += " LEFT JOIN dbo.BasicData b ON a.ClientType = b.Value AND b.IsDelete=0  AND b.TypeCode='ClientType' ";
            entity.TableName += " LEFT JOIN dbo.BasicData c ON a.IsShow = c.Value AND c.IsDelete=0  AND c.TypeCode='YesOrNo' ";
            entity.TableName += " LEFT JOIN dbo.BasicData d ON a.IsTop = d.Value AND d.IsDelete=0  AND d.TypeCode='YesOrNo' ";
            entity.TableName += " LEFT JOIN dbo.BasicData e ON a.Template = e.Value AND e.IsDelete=0  AND e.TypeCode='ContentShowScope' ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , ClientTypeName = b.Name ";
            entity.QueryFieldName += " , IsShows = c.Name ";
            entity.QueryFieldName += " , IsTops = d.Name ";
            entity.QueryFieldName += " , TemplateName = e.Name ";
            entity.QueryFieldName += ",  EndStatus = (CASE WHEN  ISNULL(EndTime,GETDATE()) <= GETDATE() THEN '已结束'  WHEN ISNULL(EndTime,GETDATE()) > GETDATE() THEN '未结束'  END) ";
            entity.QueryFieldName += ",  EventStatus = (CASE ISNULL(IsShow, '0') WHEN '0' THEN '未发布' WHEN '1' THEN '已发布'  END) ";
            entity.QueryFieldName += " , RegionSecondName = (CASE a.ClientType  ";
            entity.QueryFieldName += " WHEN '1' THEN  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID) END  ";
            entity.QueryFieldName += " WHEN '2' THEN ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID) END  ";
            entity.QueryFieldName += " END) ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            entity.SortField = "a." + entity.SortField;
            entity.SortField = " a.SortIndex";

            if (!string.IsNullOrEmpty(eventEntity.ClientType))
            {
                if (eventEntity.ClientType != "-1")
                {
                    if (eventEntity.ClientType != "0")
                    {
                        entity.QueryCondition += string.Format(" AND a.ClientType = {0} ", eventEntity.ClientType);
                    }
                }
            }
            if (eventEntity.RegionFirstTypeID != 0)
            {
                if (eventEntity.RegionFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionFirstTypeID = {0} ", eventEntity.RegionFirstTypeID);
                }
            }
            if (eventEntity.RegionSecondTypeID != 0)
            {
                if (eventEntity.RegionSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionSecondTypeID = {0} ", eventEntity.RegionSecondTypeID);
                }
            }
            if (eventEntity.DealerFirstTypeID != 0)
            {
                if (eventEntity.DealerFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerFirstTypeID = {0} ", eventEntity.DealerFirstTypeID);
                }
            }
            if (eventEntity.DealerSecondTypeID != 0)
            {
                if (eventEntity.DealerSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerSecondTypeID = {0} ", eventEntity.DealerSecondTypeID);
                }
            }
            if (eventEntity.DealerLastTypeID != 0)
            {
                if (eventEntity.DealerLastTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerLastTypeID = {0} ", eventEntity.DealerLastTypeID);
                }
            }
            if (eventEntity.IsShow != "-1")
            {
                entity.QueryCondition += string.Format(" AND ISNULL(a.IsShow, '0') = {0} ", eventEntity.IsShow);
            }
            //entity.QueryCondition += string.Format("ORDER BY CreateTime ASC ");
            if (!string.IsNullOrEmpty(eventEntity.Title))
            {
                entity.QueryCondition += string.Format(" AND (a.Title LIKE '%{0}%') ", eventEntity.Title);
            }
            if (eventEntity.EndStatusTime != "-1")
            {
                if (eventEntity.EndStatusTime == "0")//未结束
                {
                    entity.QueryCondition += string.Format(" AND ISNULL(EndTime,GETDATE()) > GETDATE() ");
                }
                if (eventEntity.EndStatusTime == "1")//已结束
                {
                    entity.QueryCondition += string.Format(" AND ISNULL(EndTime,GETDATE()) <= GETDATE() ");
                }
            }
            entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", CurrentUserInfo.ClientID);

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 根据区域查下面经销商

        /// <summary>
        /// 根据区域查下面经销商
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetEventLists(PagedQueryEntity entity, EventEntity eventEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Event a";
            entity.TableName += " LEFT JOIN dbo.BasicData b ON a.ClientType = b.Value AND b.IsDelete=0  AND b.TypeCode='ClientType' ";
            entity.TableName += " LEFT JOIN dbo.BasicData c ON a.IsShow = c.Value AND c.IsDelete=0  AND c.TypeCode='YesOrNo' ";
            entity.TableName += " LEFT JOIN dbo.BasicData d ON a.IsTop = d.Value AND d.IsDelete=0  AND d.TypeCode='YesOrNo' ";
            entity.TableName += " LEFT JOIN dbo.BasicData f ON a.Template = f.Value AND f.IsDelete=0  AND f.TypeCode='ContentShowScope' ";
            entity.TableName += " LEFT JOIN dbo.Client e ON e.ID = a.ClientID AND b.IsDelete=0 ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , ClientTypeName = b.Name ";
            entity.QueryFieldName += " , IsShows = c.Name ";
            entity.QueryFieldName += " , IsTops = d.Name ";
            entity.QueryFieldName += " , TemplateName = f.Name ";
            entity.QueryFieldName += " , ClientNames = e.Name ";
            entity.QueryFieldName += ",  EndStatus = (CASE WHEN  ISNULL(EndTime,GETDATE()) <= GETDATE() THEN '已结束'  WHEN ISNULL(EndTime,GETDATE()) > GETDATE() THEN '未结束'  END) ";
            entity.QueryFieldName += ",  EventStatus = (CASE ISNULL(IsShow, '0') WHEN '0' THEN '未发布' WHEN '1' THEN '已发布'  END) ";
            entity.QueryFieldName += " , RegionSecondName = (CASE a.ClientType  ";
            entity.QueryFieldName += " WHEN '1' THEN  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID) END  ";
            entity.QueryFieldName += " WHEN '2' THEN ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID) END  ";
            entity.QueryFieldName += " END) ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            entity.SortField = "a." + entity.SortField;
            entity.SortField = " a.SortIndex";

            if (!string.IsNullOrEmpty(eventEntity.ClientType))
            {
                if (eventEntity.ClientType != "-1")
                {
                    if (eventEntity.ClientType != "0")
                    {
                        entity.QueryCondition += string.Format(" AND a.ClientType = {0} ", eventEntity.ClientType);
                    }
                }
            }
            if (eventEntity.DealerFirstTypeID != 0)
            {
                if (eventEntity.DealerFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerFirstTypeID = {0} ", eventEntity.DealerFirstTypeID);
                }
            }
            if (eventEntity.DealerSecondTypeID != 0)
            {
                if (eventEntity.DealerSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerSecondTypeID = {0} ", eventEntity.DealerSecondTypeID);
                }
            }
            if (eventEntity.DealerLastTypeID != 0)
            {
                if (eventEntity.DealerLastTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerLastTypeID = {0} ", eventEntity.DealerLastTypeID);
                }
            }
            if (eventEntity.ClientID != -1)
            {
                entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", eventEntity.ClientID);
            }
            if (eventEntity.IsShow != "-1")
            {
                entity.QueryCondition += string.Format(" AND ISNULL(a.IsShow, '0') = {0} ", eventEntity.IsShow);
            }
            //entity.QueryCondition += string.Format("ORDER BY CreateTime ASC ");
            if (!string.IsNullOrEmpty(eventEntity.Title))
            {
                entity.QueryCondition += string.Format(" AND (a.Title LIKE '%{0}%') ", eventEntity.Title);
            }
            if (eventEntity.EndStatusTime == "0")//未结束
            {
                entity.QueryCondition += string.Format(" AND ISNULL(a.EndTime,GETDATE()) > GETDATE() ");
            }
            if (eventEntity.EndStatusTime == "1")//已结束
            {
                entity.QueryCondition += string.Format(" AND ISNULL(a.EndTime,GETDATE()) <= GETDATE() ");
            }

            entity.QueryCondition += string.Format(" AND e.ParentID = {0} ", CurrentUserInfo.ClientID);

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 获取内容表里面数据

        /// <summary>
        /// 获取内容表里面数据
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetContentLibraryList(PagedQueryEntity entity, ContentLibraryEntity contentEntity, string searchId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ContentLibrary a";
            entity.QueryFieldName = " a.*, ";
            entity.QueryFieldName += " LibraryCount = (SELECT COUNT(*) FROM dbo.Event b WHERE b.IsDelete = 0 AND b.ContentLibraryID = a.ID)";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            if (!string.IsNullOrEmpty(searchId))
            {
                entity.QueryCondition += string.Format(" AND a.ContentShowScope LIKE '%{0}%' ", searchId);
            }
            entity.QueryCondition += string.Format(" AND a.Enabled = 1 ");
            entity.QueryCondition += string.Format(" AND a.ContentApplicationScope LIKE '%1%' ");
            entity.QueryCondition += string.Format(" AND a.ClientType={0} ", this.CurrentUserInfo.ClientType);//区域还是经销商

            if (contentEntity.RegionFirstTypeID != 0)
            {
                if (contentEntity.RegionFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionFirstTypeID={0} ", contentEntity.RegionFirstTypeID);
                }
            }
            if (contentEntity.RegionSecondTypeID != 0)
            {
                if (contentEntity.RegionSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionSecondTypeID={0} ", contentEntity.RegionSecondTypeID);
                }
            }
            if (contentEntity.DealerFirstTypeID != 0) 
            {
                if (contentEntity.DealerFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerFirstTypeID={0}", contentEntity.DealerFirstTypeID);
                }
            }
            if (contentEntity.DealerSecondTypeID != 0) 
            {
                if (contentEntity.DealerSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerSecondTypeID={0}", contentEntity.DealerSecondTypeID);
                }
            }
            if (contentEntity.DealerLastTypeID != 0) 
            {
                if (contentEntity.DealerLastTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerLastTypeID={0}", contentEntity.DealerLastTypeID);
                }
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        //#region 获取市场活动表里面数据

        ///// <summary>
        ///// 获取市场活动表里面数据
        ///// </summary>
        //public PagedQueryObjectResult<DataSet> GetEventLists(PagedQueryEntity entity, string searchText, int clientId, int id, string eventIds)
        //{
        //    var result = new PagedQueryObjectResult<DataSet>();
        //    var query = new PagedQuery(this.CurrentUserInfo);

        //    entity.TableName = "dbo.Event ";
        //    entity.QueryFieldName = "*";
        //    entity.QueryCondition = " AND IsDelete = 0 ";
        //    if (!string.IsNullOrEmpty(searchText))
        //    {
        //        entity.QueryCondition += string.Format(" AND (Title LIKE '%{0}%') ", searchText);
        //    }
        //    if (clientId != -1)
        //    {
        //        if (clientId != 0)
        //        {
        //            if (id != -1)
        //            {
        //                entity.QueryCondition += string.Format(" AND ClientID IN({0},{1})", CurrentUserInfo.ClientID, clientId);
        //            }
        //            else
        //            {
        //                entity.QueryCondition += string.Format(" AND ClientID={0}", clientId);
        //            }

        //        }
        //        if (clientId == 0)//经销商
        //        {
        //            entity.QueryCondition += string.Format(" AND ClientID={0}", CurrentUserInfo.ClientID);
        //        }
        //    }
        //    if (clientId == -1)
        //    {
        //        entity.QueryCondition += string.Format(" AND ClientID = {0} ", CurrentUserInfo.ClientID);
        //    }
        //    if (!string.IsNullOrEmpty(eventIds))
        //    {
        //        entity.QueryCondition += string.Format(" AND ID NOT IN({0}) ", eventIds);
        //    }
        //    entity.QueryCondition += string.Format(" AND Title IS NOT NULL ");

        //    result.RowCount = query.GetTotalCount(entity);
        //    result.Data = query.GetPagedData(entity);

        //    return result;
        //}

        //#endregion

        #region 获取市场活动表里面数据

        /// <summary>
        /// 获取市场活动表里面数据
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetEventLists(PagedQueryEntity entity, string searchText, string clientId, int id, string eventIds)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Event ";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Title LIKE '%{0}%') ", searchText);
            }
            if (!string.IsNullOrEmpty(clientId))
            {
                if (id != -1)
                {
                    entity.QueryCondition += string.Format(" AND ClientID IN({0},{1})", CurrentUserInfo.ClientID, clientId);
                }
                if (id == -1)
                {
                    entity.QueryCondition += string.Format(" AND ClientID={0}", clientId);
                }
            }
            if (!string.IsNullOrEmpty(eventIds))
            {
                entity.QueryCondition += string.Format(" AND ID NOT IN({0}) ", eventIds);
            }
            entity.QueryCondition += string.Format(" AND Title IS NOT NULL ");

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 获取父类ID

        /// <summary>
        /// 获取父类ID
        /// </summary>
        public string GetEventListId(int parentId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
          SELECT  ParentID FROM dbo.Client WHERE ID={0} AND IsDelete=0
            ", parentId);

            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion

        #region 返回最新插入的ID值

        ///<summary>
        /// 返回为某个会话和作用域中的指定表生成的最新标识值
        ///</summary>
        ///<returns>表名</returns>
        public int GetIdentCurrent(string tableName)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT IDENT_CURRENT('{0}') ", tableName);

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 删除市场活动

        /// <summary>
        /// 删除市场活动
        /// </summary>
        /// <param name="eventId">市场活动ID</param>
        public void DeleteEvent(string eventId)
        {
            if (!string.IsNullOrEmpty(eventId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Event SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", eventId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 更新活动报名表是否删除

        ///<summary>
        /// 更新活动报名表是否删除
        ///</summary>
        ///<param name="eventId">活动ID</param>
        /// <param name="eventApplyIds">活动报名ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateEventApplyIsDelete(int eventId, string eventApplyIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.EventApply SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (eventId != -1)
            {
                sql.AppendFormat(" AND EventID = {0} ", eventId);
            }
            if (!string.IsNullOrEmpty(eventApplyIds))
            {
                sql.AppendFormat(" AND OptionValue IN ({0}) ", eventApplyIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取指定活动报名

        /// <summary>
        /// 获取指定活动报名
        /// </summary>
        /// <param name="eventId">活动ID</param>
        /// <returns></returns>
        public DataSet GetEventApplyByEventId(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.OptionValue FROM dbo.EventApply a ");
            sql.AppendFormat(" WHERE a.EventID = {0} ", eventId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 根据活动ID获取所属活动报名集合

        /// <summary>
        /// 根据活动ID获取所属活动报名集合
        /// </summary>
        /// <param name="eventId">活动ID</param>
        /// <returns></returns>
        public DataSet GetEventListByEventId(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.ID,a.OptionValue FROM dbo.EventApply a ");
            sql.AppendFormat(" INNER JOIN dbo.Event b ON a.EventID = b.ID AND b.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.EventID = {0} ", eventId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取区域市场促销活动信息(售前)

        /// <summary>
        /// 获取区域市场促销活动信息
        /// </summary>
        /// <param name="Count">区域活动总数</param>
        /// <returns></returns>
        public DataSet GetRegionMarketEventList(int count, string openID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP {0} EventID=id, ", count);
            sql.AppendFormat(" EventName=Title,Description,PictureUrl, ");
            sql.AppendFormat(" StartTime=CONVERT(varchar(100),StartTime, 120),EndTime=CONVERT(varchar(100),EndTime, 120), ");
            sql.AppendFormat(" EventType=Template ");
            sql.AppendFormat(" FROM dbo.Event ");
            sql.AppendFormat(" WHERE IsDelete = 0 AND ClientType = 1 ");
            sql.AppendFormat(" AND isshow =1 AND IsTop=1 ");
            sql.AppendFormat(" AND EndTime>'{0}' ", DateTime.Now.AddDays(-1)); //活动的最后日期,只精确到天,所以是包括这一天的;例如,2014-08-12,应该是包括12日这一天的
            sql.AppendFormat(" AND RegionFirstTypeID = ( SELECT TOP 1  b.ID FROM dbo.ContentType b WHERE  b.IsDelete = 0AND b.Name = '销售' AND ClientType = '1') ");
            //sql.AppendFormat(" AND ClientID=( SELECT a.ClientID FROM dbo.Customer a WHERE a.WxOpenId='{0}' AND a.IsDelete=0) ", openID);
            //jacky.qian  2014-08-14
            //需要判断用户是属于区域还是经销商
            sql.AppendFormat(" AND ClientID = ( CASE WHEN ( ( SELECT TOP 1   b.Type ");
            sql.AppendFormat(" 			       FROM     dbo.Customer a ");
            sql.AppendFormat(" 					INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			       WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 					AND a.IsDelete = 0 ");
            sql.AppendFormat(" 					AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			     ) = '2' ) ");
            sql.AppendFormat(" 		      THEN ( SELECT TOP 1  b.ParentID ");  //用户属于经销商
            sql.AppendFormat(" 			     FROM   dbo.Customer a ");
            sql.AppendFormat(" 				    INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			     WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 				    AND a.IsDelete = 0 ");
            sql.AppendFormat(" 				    AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			   ) ");
            sql.AppendFormat(" 		      ELSE ( SELECT TOP 1  b.ID ");    //用户属于区域
            sql.AppendFormat(" 			     FROM   dbo.Customer a ");
            sql.AppendFormat(" 				    INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			     WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 				    AND a.IsDelete = 0 ");
            sql.AppendFormat(" 				    AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			   ) ");
            sql.AppendFormat(" 		 END ) ");
            sql.AppendFormat(" ORDER BY SortIndex DESC,LastUpdateTime DESC ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取区域市场促销活动信息(售后)

        /// <summary>
        /// 获取区域市场促销活动信息(售后)
        /// </summary>
        /// <param name="Count">区域活动总数</param>
        /// <returns></returns>
        public DataSet GetRegionAfterSaleEventList(int count, string openID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP {0} EventID=id, ", count);
            sql.AppendFormat(" EventName=Title,Description,PictureUrl, ");
            sql.AppendFormat(" StartTime=CONVERT(varchar(100),StartTime, 120),EndTime=CONVERT(varchar(100),EndTime, 120), ");
            sql.AppendFormat(" EventType=Template ");
            sql.AppendFormat(" FROM dbo.Event ");
            sql.AppendFormat(" WHERE IsDelete = 0 AND ClientType = 1 ");
            sql.AppendFormat(" AND isshow =1 AND IsTop=1 ");
            sql.AppendFormat(" AND EndTime>'{0}' ", DateTime.Now.AddDays(-1)); //活动的最后日期,只精确到天,所以是包括这一天的;例如,2014-08-12,应该是包括12日这一天的
            sql.AppendFormat(" AND RegionFirstTypeID = ( SELECT TOP 1  b.ID FROM dbo.ContentType b WHERE  b.IsDelete = 0AND b.Name = '售后' AND ClientType = '1') ");
            //sql.AppendFormat(" AND ClientID=( SELECT a.ClientID FROM dbo.Customer a WHERE a.WxOpenId='{0}' AND a.IsDelete=0) ", openID);
            //  2014-08-15
            //需要判断用户是属于区域还是经销商
            sql.AppendFormat(" AND ClientID = ( CASE WHEN ( ( SELECT TOP 1   b.Type ");
            sql.AppendFormat(" 			       FROM     dbo.Customer a ");
            sql.AppendFormat(" 					INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			       WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 					AND a.IsDelete = 0 ");
            sql.AppendFormat(" 					AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			     ) = '2' ) ");
            sql.AppendFormat(" 		      THEN ( SELECT TOP 1  b.ParentID ");  //用户属于经销商
            sql.AppendFormat(" 			     FROM   dbo.Customer a ");
            sql.AppendFormat(" 				    INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			     WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 				    AND a.IsDelete = 0 ");
            sql.AppendFormat(" 				    AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			   ) ");
            sql.AppendFormat(" 		      ELSE ( SELECT TOP 1  b.ID ");    //用户属于区域
            sql.AppendFormat(" 			     FROM   dbo.Customer a ");
            sql.AppendFormat(" 				    INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			     WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 				    AND a.IsDelete = 0 ");
            sql.AppendFormat(" 				    AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			   ) ");
            sql.AppendFormat(" 		 END ) ");
            sql.AppendFormat(" ORDER BY SortIndex DESC,LastUpdateTime DESC ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取经销商市场促销活动信息(售前)

        /// <summary>
        /// 获取经销商市场促销活动信息
        /// </summary>
        /// <param name="Count">经销商活动总数</param>
        /// <returns></returns>
        public DataSet GetDealerMarketEventList(int count, string openID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP {0} EventID=id, ", count);
            sql.AppendFormat(" EventName=Title,Description,PictureUrl, ");
            sql.AppendFormat(" StartTime=CONVERT(varchar(100),StartTime, 120),EndTime=CONVERT(varchar(100),EndTime, 120), ");
            sql.AppendFormat(" EventType=Template ");
            sql.AppendFormat(" FROM dbo.Event ");
            sql.AppendFormat(" WHERE IsDelete = 0 AND ClientType = 2 ");
            sql.AppendFormat(" AND isshow =1 AND IsTop=1 ");
            sql.AppendFormat(" AND EndTime>'{0}' ", DateTime.Now.AddDays(-1)); //活动的最后日期,只精确到天,所以是包括这一天的;例如,2014-08-12,应该是包括12日这一天的
            sql.AppendFormat(" AND DealerFirstTypeID = ( SELECT TOP 1  b.ID FROM dbo.ContentType b WHERE  b.IsDelete = 0AND b.Name = '销售' AND ClientType = '2') ");
            //sql.AppendFormat(" AND ClientID=( SELECT a.ClientID FROM dbo.Customer a WHERE a.WxOpenId='{0}' AND a.IsDelete=0) ", openID);
            //需要判断用户是属于区域还是经销商
            sql.AppendFormat(" AND ClientID = ( CASE WHEN ( ( SELECT  TOP 1  b.Type ");
            sql.AppendFormat(" 			       FROM     dbo.Customer a ");
            sql.AppendFormat(" 					INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			       WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 					AND a.IsDelete = 0 ");
            sql.AppendFormat(" 					AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			     ) = '2' ) ");
            sql.AppendFormat(" 		      THEN ( SELECT TOP 1  a.ClientID ");  //用户属于经销商,只需获取经销商的活动
            sql.AppendFormat(" 			     FROM dbo.Customer a ");
            sql.AppendFormat(" 			     WHERE a.WxOpenId='{0}' AND a.IsDelete=0 ", openID);
            sql.AppendFormat(" 			   ) ");
            sql.AppendFormat(" 		      ELSE ( -100 ) ");    //用户属于区域,获取不到任何经销商的活动
            sql.AppendFormat(" 		 END ) ");
            sql.AppendFormat(" ORDER BY SortIndex DESC,LastUpdateTime DESC ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取经销商市场促销活动信息(售后)

        /// <summary>
        /// 获取经销商市场促销活动信息
        /// </summary>
        /// <param name="Count">经销商活动总数</param>
        /// <returns></returns>
        public DataSet GetDealerAfterSaleEventList(int count, string openID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP {0} EventID=id, ", count);
            sql.AppendFormat(" EventName=Title,Description,PictureUrl, ");
            sql.AppendFormat(" StartTime=CONVERT(varchar(100),StartTime, 120),EndTime=CONVERT(varchar(100),EndTime, 120), ");
            sql.AppendFormat(" EventType=Template ");
            sql.AppendFormat(" FROM dbo.Event ");
            sql.AppendFormat(" WHERE IsDelete = 0 AND ClientType = 2 ");
            sql.AppendFormat(" AND isshow =1 AND IsTop=1 ");
            sql.AppendFormat(" AND EndTime>'{0}' ", DateTime.Now.AddDays(-1)); //活动的最后日期,只精确到天,所以是包括这一天的;例如,2014-08-12,应该是包括12日这一天的
            sql.AppendFormat(" AND DealerFirstTypeID = ( SELECT TOP 1  b.ID FROM dbo.ContentType b WHERE  b.IsDelete = 0AND b.Name = '售后' AND ClientType = '2') ");
            //sql.AppendFormat(" AND ClientID=( SELECT a.ClientID FROM dbo.Customer a WHERE a.WxOpenId='{0}' AND a.IsDelete=0) ", openID);
            //需要判断用户是属于区域还是经销商
            sql.AppendFormat(" AND ClientID = ( CASE WHEN ( ( SELECT TOP 1   b.Type ");
            sql.AppendFormat(" 			       FROM     dbo.Customer a ");
            sql.AppendFormat(" 					INNER JOIN dbo.Client b ON a.ClientID = b.ID ");
            sql.AppendFormat(" 			       WHERE    a.WxOpenId = '{0}' ", openID);
            sql.AppendFormat(" 					AND a.IsDelete = 0 ");
            sql.AppendFormat(" 					AND b.IsDelete = 0 ");
            sql.AppendFormat(" 			     ) = '2' ) ");
            sql.AppendFormat(" 		      THEN ( SELECT TOP 1  a.ClientID ");  //用户属于经销商,只需获取经销商的活动
            sql.AppendFormat(" 			     FROM dbo.Customer a ");
            sql.AppendFormat(" 			     WHERE a.WxOpenId='{0}' AND a.IsDelete=0 ", openID);
            sql.AppendFormat(" 			   ) ");
            sql.AppendFormat(" 		      ELSE ( -100 ) ");    //用户属于区域,获取不到任何经销商的活动
            sql.AppendFormat(" 		 END ) ");
            sql.AppendFormat(" ORDER BY SortIndex DESC,LastUpdateTime DESC ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取市场活动详情信息(活动ID，微信用户标识)

        /// <summary>
        /// 获取市场活动详情信息
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// <param name="OpenID">微信用户标识</param>
        /// <returns></returns>
        public DataSet GetMarketEventDetailInfo(int EventID, string OpenID)
        {
            //在指定客户、指定活动下，获取报名的条数
            int count1 = 1;
            int count2 = 1;
            if (!string.IsNullOrEmpty(OpenID))
            {
                count1 = GetCountApplyResult(EventID, OpenID);
                //在指定客户、指定活动下，获取投票的条数
                count2 = GetCountVoteResult(EventID, OpenID);
            }
            DataSet ds = GetVoteType(EventID);
            string voteTyle = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                voteTyle = ds.Tables[0].Rows[0]["Type"].ToString();
            }
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT EventID=a.ID, ");
            sql.AppendFormat(" EventName=a.Title,Description,Content, ");
            sql.AppendFormat(" PictureUrl, ");
            sql.AppendFormat(" GameUrl= ");
            sql.AppendFormat(" ISNULL((SELECT GameUrl FROM dbo.Game WHERE id=(SELECT TOP 1 GameID FROM EventGameMapping WHERE EventID=a.ID)),''),  ");
            sql.AppendFormat(" GameID= ");
            sql.AppendFormat(" ISNULL((SELECT TOP 1 GameID FROM EventGameMapping WHERE EventID=a.ID AND IsDelete=0),0) ,");
            sql.AppendFormat(" StartTime=CONVERT(varchar(100),StartTime, 120),EndTime=CONVERT(varchar(100),EndTime, 120), ");
            sql.AppendFormat(" EventType=a.Template,VideoUrl, ");
            sql.AppendFormat(" IsEnd=(CASE WHEN a.EndTime<'{0}' THEN 1 ELSE 0 END), ", DateTime.Now.AddDays(-1));
            if (voteTyle == "1")
            {
                sql.AppendFormat(" VoteType='1', ");
            }
            if (voteTyle == "2")
            {
                sql.AppendFormat(" VoteType='2', ");
            }
            else
            {
                sql.AppendFormat(" VoteType='', ");
            }
            if (count1 > 0)
                sql.AppendFormat(" IsApply=1, ");
            else
                sql.AppendFormat(" IsApply=0, ");
            if (count2 > 0)
                sql.AppendFormat(" IsVote=1 ");
            else
                sql.AppendFormat(" IsVote=0 ");

            sql.AppendFormat(" FROM Event a ");
            sql.AppendFormat(" WHERE id={0} ", EventID);
            sql.AppendFormat(" AND IsDelete=0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取用户是否报名(活动ID，微信用户标识)

        /// <summary>
        /// 获取用户是否已经报名
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// <param name="OpenID">微信用户标识</param>
        /// <returns></returns>
        public int GetCountApplyResult(int EventID, string OpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT COUNT(*) FROM Customer a,EventApplyResult b ");
            sql.AppendFormat(" WHERE  a.WxOpenId='{0}' ", OpenID);
            sql.AppendFormat(" AND b.EventID={0} ", EventID);
            sql.AppendFormat(" AND a.id = b.CustomerID ");
            sql.AppendFormat(" AND b.IsDelete=0 AND a.IsDelete=0 ");
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取用户是否投票(活动ID，微信用户标识)

        /// <summary>
        /// 获取用户是否投票
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// <param name="OpenID">微信用户标识</param>
        /// <returns></returns>
        public int GetCountVoteResult(int EventID, string OpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT COUNT(*) FROM Customer a,VoteResult b ");
            sql.AppendFormat(" WHERE  a.WxOpenId='{0}' ", OpenID);
            sql.AppendFormat(" AND b.EventID={0} ", EventID);
            sql.AppendFormat(" AND a.id = b.CustomerID ");
            sql.AppendFormat(" AND b.IsDelete=0 AND a.IsDelete=0 ");
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取投票类型(活动ID)

        /// <summary>
        /// 获取用户是否投票
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// 
        /// <returns></returns>
        public DataSet GetVoteType(int EventID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.Type FROM vote a  ");
            sql.AppendFormat(" WHERE a.id IN ");
            sql.AppendFormat(" (SELECT b.VoteID FROM EventVoteMapping b WHERE b.EventID={0} AND b.IsDelete=0) ", EventID);
            sql.AppendFormat(" AND a.IsDelete=0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取可以兑换礼品的活动(场景一)

        /// <summary>
        /// 获取可以兑换礼品的活动 ，可以兑换兑换码活动且没过期，且结束时间最大的活动的ID
        /// </summary>
        /// <returns></returns>
        public DataSet GetEventByCS(int clientID)
        {
            //string str = null;
            //for (int i = 0; i < listEventID.Count; i++)
            //{
            //    str += listEventID[i] + ",";
            //}
            //str = str.Substring(0, str.Length - 1);
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP 1 * FROM dbo.Event a WHERE a.ContentScenario='1' AND a.IsDelete=0 ");
            sql.AppendFormat(" AND a.EndTime>'{0}' ", DateTime.Now.AddDays(-1)); //活动的最后日期,只精确到天,所以是包括这一天的;例如,2014-08-12,应该是包括12日这一天的
            //if(listEventID!=null)
            //    sql.AppendFormat(" AND ID IN ({0}) ", str);
            sql.AppendFormat(" AND a.ClientID={0} ", clientID);
            sql.AppendFormat(" ORDER BY a.EndTime DESC ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取可以兑换礼品的活动(场景二)

        /// <summary>
        /// 获取可以兑换礼品的活动 ，可以兑换兑换码活动且没过期，且结束时间最大的活动的ID
        /// </summary>
        /// <returns></returns>
        public DataSet GetEventByCSA(int clientID)
        {
            //string str = null;
            //for (int i = 0; i < listEventID.Count; i++)
            //{
            //    str += listEventID[i] + ",";
            //}
            //str = str.Substring(0, str.Length - 1);
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP 1 * FROM dbo.Event a WHERE a.ContentScenario='2' AND a.IsDelete=0 ");
            sql.AppendFormat(" AND a.EndTime>'{0}' ", DateTime.Now.AddDays(-1)); //活动的最后日期,只精确到天,所以是包括这一天的;例如,2014-08-12,应该是包括12日这一天的
            //if(listEventID!=null)
            //    sql.AppendFormat(" AND ID IN ({0}) ", str);
            sql.AppendFormat(" AND a.ClientID={0} ", clientID);
            sql.AppendFormat(" ORDER BY a.EndTime DESC ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
        //#region 获取经销商市场促销活动信息

        ///// <summary>
        ///// 获取经销商市场促销活动信息
        ///// </summary>
        ///// <param name="Count">经销商活动总数</param>
        ///// <returns></returns>
        //public DataSet GetMarketEventDetailInfo(int EventID, string OpenID)
        //{
        //    var sql = new StringBuilder();

        //    sql.AppendFormat(" SELECT TOP {0} EventID=id, ", count);
        //    sql.AppendFormat(" EventName=Title,Description,PictureUrl, ");
        //    sql.AppendFormat(" StartTime,EndTime, ");
        //    sql.AppendFormat(" EventType=Template ");
        //    sql.AppendFormat(" FROM dbo.Event ");
        //    sql.AppendFormat(" WHERE IsDelete = 0 AND ClientType = 2 ");
        //    sql.AppendFormat(" AND isshow =1 AND IsTop=1 ");
        //    sql.AppendFormat(" ORDER BY SortIndex DESC,LastUpdateTime DESC ");
        //    return this.SQLHelper.ExecuteDataset(sql.ToString());
        //}

        //#endregion

        #region 添加市场活动

        /// <summary>
        /// 添加市场活动
        /// </summary>
        public void AddEvent(EventEntity eventEntity)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" INSERT INTO dbo.Event ");
            sql.AppendFormat(" ( ClientType ,RegionFirstTypeID ,RegionSecondTypeID ,DealerFirstTypeID ,DealerSecondTypeID ,DealerLastTypeID ,Template ,ClientID ,CreateBy ,CreateTime ,LastUpdateBy ,LastUpdateTime ,IsDelete) ");
            sql.AppendFormat(" SELECT {0},{1},{2},{3},{4},{5},{6},{7},{8} ,GETDATE() ,{8}, GETDATE() ,1 ", eventEntity.ClientType, eventEntity.RegionFirstTypeID, eventEntity.RegionSecondTypeID, eventEntity.DealerFirstTypeID, eventEntity.DealerSecondTypeID, eventEntity.DealerLastTypeID, eventEntity.Template, eventEntity.ClientID, this.CurrentUserInfo.UserID);

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 包括状态 IsDelete=1
        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public EventEntity GetByIDs(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Event] where ID='{0}' ", id.ToString());
            //读取数据
            EventEntity m = null;
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    this.Load(rdr, out m);
                    break;
                }
            }
            //返回
            return m;
        }
        #endregion

        #region 更新IsDelete=1操作
        /// <summary>
        /// 更新IsDelete=1操作
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Updates(EventEntity pEntity, IDbTransaction pTran, bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.ID.HasValue)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
            //初始化固定字段
            pEntity.LastUpdateTime = DateTime.Now;
            pEntity.LastUpdateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.IsDelete = 0;

            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [Event] set ");
            if (pIsUpdateNullField || pEntity.ClientType != null)
                strSql.Append("[ClientType]=@ClientType,");
            if (pIsUpdateNullField || pEntity.RegionFirstTypeID != null)
                strSql.Append("[RegionFirstTypeID]=@RegionFirstTypeID,");
            if (pIsUpdateNullField || pEntity.RegionSecondTypeID != null)
                strSql.Append("[RegionSecondTypeID]=@RegionSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerFirstTypeID != null)
                strSql.Append("[DealerFirstTypeID]=@DealerFirstTypeID,");
            if (pIsUpdateNullField || pEntity.DealerSecondTypeID != null)
                strSql.Append("[DealerSecondTypeID]=@DealerSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerLastTypeID != null)
                strSql.Append("[DealerLastTypeID]=@DealerLastTypeID,");
            if (pIsUpdateNullField || pEntity.Template != null)
                strSql.Append("[Template]=@Template,");
            if (pIsUpdateNullField || pEntity.Title != null)
                strSql.Append("[Title]=@Title,");
            if (pIsUpdateNullField || pEntity.Description != null)
                strSql.Append("[Description]=@Description,");
            if (pIsUpdateNullField || pEntity.Content != null)
                strSql.Append("[Content]=@Content,");
            if (pIsUpdateNullField || pEntity.PictureUrl != null)
                strSql.Append("[PictureUrl]=@PictureUrl,");
            if (pIsUpdateNullField || pEntity.StartTime != null)
                strSql.Append("[StartTime]=@StartTime,");
            if (pIsUpdateNullField || pEntity.EndTime != null)
                strSql.Append("[EndTime]=@EndTime,");
            if (pIsUpdateNullField || pEntity.SortIndex != null)
                strSql.Append("[SortIndex]=@SortIndex,");
            if (pIsUpdateNullField || pEntity.IsShow != null)
                strSql.Append("[IsShow]=@IsShow,");
            if (pIsUpdateNullField || pEntity.IsTop != null)
                strSql.Append("[IsTop]=@IsTop,");
            if (pIsUpdateNullField || pEntity.VideoUrl != null)
                strSql.Append("[VideoUrl]=@VideoUrl,");
            if (pIsUpdateNullField || pEntity.OriginalUrl != null)
                strSql.Append("[OriginalUrl]=@OriginalUrl,");
            if (pIsUpdateNullField || pEntity.Remark != null)
                strSql.Append("[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ClientID != null)
                strSql.Append("[ClientID]=@ClientID,");
            if (pIsUpdateNullField || pEntity.ContentScenario != null)
                strSql.Append("[ContentScenario]=@ContentScenario,");
            if (pIsUpdateNullField || pEntity.ContentLibraryID != null)
                strSql.Append("[ContentLibraryID]=@ContentLibraryID,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy != null)
                strSql.Append("[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime != null)
                strSql.Append("[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.IsDelete != null)
                strSql.Append("[IsDelete]=@IsDelete");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@ClientType",SqlDbType.NVarChar),
					new SqlParameter("@RegionFirstTypeID",SqlDbType.Int),
					new SqlParameter("@RegionSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerFirstTypeID",SqlDbType.Int),
					new SqlParameter("@DealerSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerLastTypeID",SqlDbType.Int),
					new SqlParameter("@Template",SqlDbType.NVarChar),
					new SqlParameter("@Title",SqlDbType.NVarChar),
					new SqlParameter("@Description",SqlDbType.NVarChar),
					new SqlParameter("@Content",SqlDbType.NVarChar),
					new SqlParameter("@PictureUrl",SqlDbType.NVarChar),
					new SqlParameter("@StartTime",SqlDbType.DateTime),
					new SqlParameter("@EndTime",SqlDbType.DateTime),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@IsShow",SqlDbType.NVarChar),
					new SqlParameter("@IsTop",SqlDbType.NVarChar),
					new SqlParameter("@VideoUrl",SqlDbType.NVarChar),
					new SqlParameter("@OriginalUrl",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@ContentScenario",SqlDbType.NVarChar),
					new SqlParameter("@ContentLibraryID",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
                    new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@ID",SqlDbType.Int)
            };
            parameters[0].Value = pEntity.ClientType;
            parameters[1].Value = pEntity.RegionFirstTypeID;
            parameters[2].Value = pEntity.RegionSecondTypeID;
            parameters[3].Value = pEntity.DealerFirstTypeID;
            parameters[4].Value = pEntity.DealerSecondTypeID;
            parameters[5].Value = pEntity.DealerLastTypeID;
            parameters[6].Value = pEntity.Template;
            parameters[7].Value = pEntity.Title;
            parameters[8].Value = pEntity.Description;
            parameters[9].Value = pEntity.Content;
            parameters[10].Value = pEntity.PictureUrl;
            parameters[11].Value = pEntity.StartTime;
            parameters[12].Value = pEntity.EndTime;
            parameters[13].Value = pEntity.SortIndex;
            parameters[14].Value = pEntity.IsShow;
            parameters[15].Value = pEntity.IsTop;
            parameters[16].Value = pEntity.VideoUrl;
            parameters[17].Value = pEntity.OriginalUrl;
            parameters[18].Value = pEntity.Remark;
            parameters[19].Value = pEntity.ClientID;
            parameters[20].Value = pEntity.ContentScenario;
            parameters[21].Value = pEntity.ContentLibraryID;
            parameters[22].Value = pEntity.LastUpdateBy;
            parameters[23].Value = pEntity.LastUpdateTime;
            parameters[24].Value = pEntity.IsDelete;
            parameters[25].Value = pEntity.ID;

            //执行语句
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region   获取EventID

        public DataSet GetWNewsListByEventId(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT EventID FROM dbo.WNews ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND EventID > {0} ", eventId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取WNewsID

        /// <summary>
        /// 获取WNewsID
        /// </summary>
        public string GetWNewsLists(int eventId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@" SELECT ID FROM dbo.WNews WHERE IsDelete = 0 AND EventID = {0}", eventId);

            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion
    }
}
