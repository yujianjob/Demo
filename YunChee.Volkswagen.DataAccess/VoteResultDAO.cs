/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/8 14:14:40
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
    /// 数据访问： 0603投票结果表 VoteResult 
    /// 表VoteResult的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class VoteResultDAO : BaseDAO<BasicUserInfo>, ICRUDable<VoteResultEntity>, IQueryable<VoteResultEntity>
    {
        #region 获取投票条数(活动ID，投票选项、投票ID以及客户ID)

        /// <summary>
        /// 获取投票条数
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// <param name="VoteID">投票ID</param>
        /// <param name="VoteOptionID">投票选项ID</param>
        /// <param name="CustomerID">客户ID</param>
        /// <returns></returns>
        public int GetCountVoteResult(int EventID, int VoteID, int CustomerID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT COUNT(*) FROM dbo.VoteResult a ");
            sql.AppendFormat(" WHERE a.VoteID={0} ", VoteID);
            //sql.AppendFormat(" AND a.VoteOptionID={0} ", VoteOptionID);
            sql.AppendFormat(" AND a.EventID={0} ", EventID);
            sql.AppendFormat(" AND a.CustomerID={0} ", CustomerID);
            sql.AppendFormat(" AND IsDelete=0 ", CustomerID);
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 投票报表

        public DataSet GetEventVoteResult(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.name AS '选项名称', ");
            sql.AppendFormat(" '结果' = (SELECT COUNT(*) FROM dbo.VoteResult t WHERE t.VoteOptionID = a.ID AND t.IsDelete=0 )  ");
            sql.AppendFormat(" FROM dbo.VoteOption a ");
            sql.AppendFormat(" INNER JOIN dbo.Vote b ON b.ID = a.VoteID ");
            sql.AppendFormat(" INNER JOIN dbo.EventVoteMapping c ON c.VoteID = b.ID AND c.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.ID = (SELECT VoteID FROM dbo.EventVoteMapping WHERE IsDelete=0 AND  EventID = {0}) AND c.EventID = {0} ", eventId);


            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取VoteID

        /// <summary>
        /// 获取VoteID
        /// </summary>
        public string GetEventVoteMapping(int eventId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT VoteID FROM dbo.EventVoteMapping WHERE IsDelete=0 AND  EventID = {0} ", eventId);

            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();

            return result;
        }

        #endregion

        #region 获取投票名称

        /// <summary>
        /// 获取投票名称
        /// </summary>
        public string GetVoteName(int eventId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"  SELECT Name FROM dbo.Vote WHERE IsDelete=0 AND  ID = ( SELECT VoteID FROM dbo.EventVoteMapping WHERE IsDelete=0 AND  EventID={0} ) ", eventId);

            var obj = this.SQLHelper.ExecuteScalar(sb.ToString());

            if (obj != null)
            {
                result = obj.ToString();
            }

            return result;
        }

        #endregion
    }
}
