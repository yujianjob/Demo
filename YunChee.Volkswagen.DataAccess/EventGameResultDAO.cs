/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/20 15:12:51
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
    /// 数据访问： 0209活动游戏结果表  EventGameResult 
    /// 表EventGameResult的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class EventGameResultDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventGameResultEntity>, IQueryable<EventGameResultEntity>
    {
        #region 获取用户最佳得分

        /// <summary>
        /// 获取用户最佳得分
        /// </summary>
        /// <returns></returns>
        public DataSet GetCustomerBestPoint(string EventID, string GameID, int CustomerID)
        {
            var sql = new StringBuilder();
            if (EventID != null && EventID.Trim() != "")
            {
                sql.AppendFormat(" SELECT TOP 1 * FROM dbo.EventGameResult  ");
                sql.AppendFormat(" WHERE IsDelete=0  ");
                sql.AppendFormat(" AND EventID={0} ", EventID.ToInt());
                sql.AppendFormat(" AND CustomerID={0} ", CustomerID);
                sql.AppendFormat(" AND GameID={0} ", GameID);
                sql.AppendFormat(" ORDER BY GamePoint DESC,GameTime ASC ");
            }
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion 
    }
}
