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
    /// 数据访问： 0202活动报名表 EventApply 
    /// 表EventApply的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class EventApplyDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventApplyEntity>, IQueryable<EventApplyEntity>
    {
        #region 根据活动ID获取报名信息

        /// <summary>
        /// 根据活动ID获取报名信息
        /// </summary>
        /// <param name="eventId">活动ID</param>
        public DataSet GetEventApplyByEventId(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT OptionValue FROM dbo.EventApply WHERE EventID = {0} AND IsDelete = 0", eventId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取市场促销活动报名信息

        /// <summary>
        /// 获取市场促销活动报名信息
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// <returns></returns>
        public DataSet GetMarketEventApplyInfo(int EventID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT  OptionValue=a.value , ");
            sql.AppendFormat(" ISNULL(b.Isshow,0) AS IsShow ");//查出表中如果该值为null，则改为0
            sql.AppendFormat(" FROM    dbo.BasicData AS a ");
            sql.AppendFormat("  LEFT JOIN ( SELECT  OptionValue ,Isshow = 1 ");//默认活动报名中所包含的具体报名选项值即为默认在前端所要展示的Isshow = 1。
            sql.AppendFormat(" FROM    EventApply ");
            sql.AppendFormat("  WHERE   EventID = {0} ",EventID);
            sql.AppendFormat(" AND IsDelete = 0 ) AS b ON a.Value = b.OptionValue ");
            sql.AppendFormat(" WHERE   a.TypeCode = 'EventApplyOption' AND a.IsDelete = 0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取活动报名ID(活动ID，报名选项)

        /// <summary>
        /// 获取活动报名
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// <param name="OptionValue">报名选项值</param>
        /// <returns></returns>
        public int GetIdByEventIdOption(int EventID, string OptionValue)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ID FROM EventApply a ");
            sql.AppendFormat(" WHERE a.EventID={0} ", EventID);
            sql.AppendFormat(" AND a.OptionValue='{0}' ", OptionValue);
            sql.AppendFormat(" AND a.IsDelete=0 ");
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion
    }
}
