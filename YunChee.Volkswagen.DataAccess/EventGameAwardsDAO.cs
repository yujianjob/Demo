/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/19 13:58:55
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
    /// 数据访问： 0208活动游戏奖项表  EventGameAwards 
    /// 表EventGameAwards的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class EventGameAwardsDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventGameAwardsEntity>, IQueryable<EventGameAwardsEntity>
    {

        #region 根据游戏ID获取游戏奖项列表

        /// <summary>
        /// 根据游戏ID获取游戏奖项列表
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        public DataSet GetEventGameAwardsListByMappingId(int mappingId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.* ");
            sql.AppendFormat(" FROM dbo.EventGameAwards a ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.MappingID = {0} ", mappingId);
            //sql.AppendFormat(" ORDER BY a.SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 删除游戏奖项列表

        /// <summary>
        /// 删除游戏奖项列表
        /// </summary>
        /// <param name="gameAwardsIds">奖项ID集合  "1,2,3"</param>
        public void DeleteEventGameAwards(string gameAwardsIds)
        {
            var sql = new StringBuilder();

            //更新奖项表
            sql.AppendFormat(" UPDATE dbo.EventGameAwards SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", gameAwardsIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region 根据游戏ID、游戏积分获取该游戏的中奖信息

        /// <summary>
        /// 根据游戏ID、游戏积分获取该游戏的中奖信息
        /// </summary>
        /// <param name="GameID">游戏ID</param>
        /// <returns></returns>
        public DataSet GetAwardsInfoByPoint(string GamePoint, string GameTime, string GameID,string EventID)
        {
            var sql = new StringBuilder();
            if (GameID != null && GameID.Trim() != "")
            {
                sql.AppendFormat(" SELECT TOP 1 Level = ROW_NUMBER() OVER(ORDER BY Grade DESC), * FROM dbo.EventGameAwards a,dbo.EventGameMapping b ");
                sql.AppendFormat(" WHERE {0}>=Point  ", GamePoint);
                sql.AppendFormat(" AND b.ID = a.MappingID  ");
                sql.AppendFormat(" AND a.GameID={0} ", GameID.ToInt());
                sql.AppendFormat(" AND b.EventID={0} ", EventID.ToInt());
                sql.AppendFormat(" AND a.IsDelete=0 AND b.IsDelete=0 ");
                sql.AppendFormat(" ORDER BY Point DESC,Time ASC ");
            }
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 根据游戏ID、游戏积分获取该游戏的中奖信息

        /// <summary>
        /// 根据游戏ID、游戏时间获取该游戏的中奖信息
        /// </summary>
        /// <param name="GameID">游戏ID</param>
        /// <returns></returns>
        public DataSet GetAwardsInfoByTime(string GamePoint, string GameTime, string GameID, string EventID)
        {
            var sql = new StringBuilder();
            if (GameID != null && GameID.Trim() != "")
            {
                sql.AppendFormat(" SELECT TOP 1 Level = ROW_NUMBER() OVER(ORDER BY Grade DESC), * FROM dbo.EventGameAwards a,dbo.EventGameMapping b ");
                sql.AppendFormat(" WHERE {0}<=Time  ", GameTime);
                sql.AppendFormat(" AND b.ID = a.MappingID  ");
                sql.AppendFormat(" AND a.GameID={0} ", GameID.ToInt());
                sql.AppendFormat(" AND b.EventID={0} ", EventID.ToInt());
                sql.AppendFormat(" AND a.IsDelete=0 AND b.IsDelete=0 ");
                sql.AppendFormat(" ORDER BY Point DESC,Time ASC ");
            }
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 根据关系ID删除游戏奖项列表

        /// <summary>
        /// 根据关系ID删除游戏奖项列表
        /// </summary>
        /// <param name="mappingId">关系ID集合</param>
        public void DeleteEventGameAwarIds(string mappingId)
        {
            var sql = new StringBuilder();

            //更新奖项表
            sql.AppendFormat(" UPDATE dbo.EventGameAwards SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE MappingID={0}; ", mappingId);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }
}
