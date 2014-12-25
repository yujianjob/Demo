/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/13 13:42:54
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
    /// 数据访问： 0207活动游戏关系表 EventGameMapping 
    /// 表EventGameMapping的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class EventGameMappingDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventGameMappingEntity>, IQueryable<EventGameMappingEntity>
    {
        #region 获取市场活动游戏列表

        /// <summary>
        /// 获取市场活动游戏列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetEventGameList(PagedQueryEntity pageEntity, int eventIds)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " EventGameMapping ";
            pageEntity.QueryCondition = " and IsDelete = 0 ";
            pageEntity.QueryFieldName = " * ";
            pageEntity.QueryCondition += string.Format(" AND EventID={0} ", eventIds);

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }

        #endregion

        #region 根据EventID获取关系信息

        /// <summary>
        /// 根据EventID获取关系信息
        /// </summary>
        public DataSet GetEventGameMappingByEventId(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.EventGameMapping WHERE EventID = {0} AND IsDelete=0", eventId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 删除市场活动游戏列表

        /// <summary>
        /// 删除市场活动游戏列表
        /// </summary>
        /// <param name="gameMappingId">市场活动游戏ID</param>
        public void DeleteEventGameMapping(string gameMappingId)
        {
            if (!string.IsNullOrEmpty(gameMappingId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.EventGameMapping SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", gameMappingId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 根据EventID和游戏ID获取关系信息

        /// <summary>
        /// 根据EventID和游戏ID获取关系信息
        /// </summary>
        public DataSet GetGameInfoByEventIdGameID(string eventId,string gameID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT IsEnd=(CASE WHEN (SELECT EndTime FROM dbo.Event WHERE ID={0})>='{1}' THEN '0' ELSE '1' END), ", eventId,DateTime.Now.AddDays(-1));
            sql.AppendFormat(" ImageUrl,GameName,GameDescription,GameRule,InitialTime,ShareIconUrl=IconUrl,ShareTitle,ShareContent  ");
            sql.AppendFormat(" FROM  dbo.EventGameMapping ");
            sql.AppendFormat(" WHERE EventID= {0} ", eventId);
            sql.AppendFormat(" AND GameID={0} ", gameID);
            sql.AppendFormat(" AND IsDelete=0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 根据EventID和游戏ID获取关系信息

        /// <summary>
        /// 根据EventID和游戏ID获取关系信息
        /// </summary>
        public DataSet GetGameAllInfoByEventIdGameID(string eventId, string gameID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT  * ");
            sql.AppendFormat(" FROM  dbo.EventGameMapping ");
            sql.AppendFormat(" WHERE EventID= {0} ", eventId);
            sql.AppendFormat(" AND GameID={0} ", gameID);
            sql.AppendFormat(" AND IsDelete=0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
