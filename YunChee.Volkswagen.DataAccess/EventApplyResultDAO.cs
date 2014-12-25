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
    /// 数据访问： 0203活动报名结果表 EventApplyResult 
    /// 表EventApplyResult的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class EventApplyResultDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventApplyResultEntity>, IQueryable<EventApplyResultEntity>
    {
        #region 获取客户报名信息(活动ID，微信识别号)

        /// <summary>
        /// 获取客户报名信息
        /// </summary>
        /// <param name="EventID">活动ID</param>
        /// <param name="OpenID">微信识别号</param>
        /// <returns></returns>
        public DataSet GetResultByEventIdOpenId(int EventID, string OpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM EventApplyResult a ");
            sql.AppendFormat(" WHERE a.EventID={0} ", EventID);
            sql.AppendFormat(" AND CustomerID=(SELECT id FROM dbo.Customer WHERE WxOpenId='{0}' AND IsDelete=0) ", OpenID);
            sql.AppendFormat(" AND a.IsDelete=0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region

        public PagedQueryObjectResult<DataSet> GetEventApplyResult(PagedQueryEntity pageEntity, EventApplyResultEntity queryEntity, int eventId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"
                    SELECT  a.CustomerID ,
                            a.OptionName ,
                            a.Result
                    INTO    #temp
                    FROM    dbo.EventApplyResult a
                    WHERE   a.IsDelete = 0
                            AND a.EventID = {0}

                    DECLARE @sql VARCHAR(8000)
                    SELECT  @sql = ISNULL(@sql + '],[', '') + Name
                    FROM    dbo.BasicData a
                    WHERE   a.IsDelete = 0
                            AND a.TypeCode = 'EventApplyOption'
                    ORDER BY a.SortIndex
                    SET @sql = '[' + @sql + ']'

                    SET @sql= '
                    SELECT *,
                    applytime = (SELECT Min(c.CreateTime) FROM dbo.EventApplyResult c 
			                     WHERE c.IsDelete = 0 AND c.EventID = {0} AND c.CustomerID = a.CustomerID )
                    FROM ( SELECT * FROM #temp ) b 
                    PIVOT (MAX(Result) FOR OptionName IN ('+@sql+')) a '
                    EXEC(@sql) ", eventId);
            pageEntity.QueryFieldName = string.Format("*");

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }

        #endregion

        #region  报名报表

        public DataSet GetEventApplyResults(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat("SELECT  a.CustomerID , ");
            sql.AppendFormat("        a.OptionName , ");
            sql.AppendFormat("        Result = (CASE a.OptionValue WHEN '2' THEN ");
		    sql.AppendFormat(" (SELECT t.Name FROM dbo.BasicData t WHERE t.TypeCode = 'Gender' AND t.IsDelete = 0 AND t.Value = a.Result) ");
            sql.AppendFormat(" ELSE a.Result END) ");
            sql.AppendFormat("INTO    #temp ");
            sql.AppendFormat("FROM    dbo.EventApplyResult a ");
            sql.AppendFormat("WHERE   a.IsDelete = 0 ");
            sql.AppendFormat("        AND a.EventID = {0} ", eventId);

            sql.AppendFormat("DECLARE @sql VARCHAR(8000) ");
            sql.AppendFormat("SELECT  @sql = ISNULL(@sql + '],[', '') + Name ");
            sql.AppendFormat("FROM    dbo.BasicData a ");
            sql.AppendFormat("WHERE   a.IsDelete = 0 ");
            sql.AppendFormat("        AND a.TypeCode = 'EventApplyOption' ");
            sql.AppendFormat("ORDER BY a.SortIndex ");
            sql.AppendFormat("SET @sql = '[' + @sql + ']' ");

            sql.AppendFormat("SET @sql= ' ");
            sql.AppendFormat("SELECT *, ");
            sql.AppendFormat("applytime = (SELECT Min(c.CreateTime) FROM dbo.EventApplyResult c  ");
            sql.AppendFormat("			 WHERE c.IsDelete = 0 AND c.EventID = {0} AND c.CustomerID = a.CustomerID ) ", eventId);
            sql.AppendFormat("FROM ( SELECT * FROM #temp ) b  ");
            sql.AppendFormat("PIVOT (MAX(Result) FOR OptionName IN ('+@sql+')) a '");

            sql.AppendFormat("EXEC(@sql) ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

    }
}
