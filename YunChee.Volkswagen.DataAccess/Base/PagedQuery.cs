using System;
using System.Data;
using System.Text;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using Yunchee.Volkswagen.Utility.Log;

namespace Yunchee.Volkswagen.DataAccess.Base
{
    public class PagedQuery : BaseDAO<BasicUserInfo>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public PagedQuery(BasicUserInfo pUserInfo)
            : base(pUserInfo, ConfigInfo.CURRENT_CONNECTION_STRING_MANAGER)
        {
            this.SQLHelper.OnExecuted += new EventHandler<SqlCommandExecutionEventArgs>(SQLHelper_OnExecuted);
        }
        #endregion

        #region 事件处理
        /// <summary>
        /// SQL助手执行完毕后，记录日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SQLHelper_OnExecuted(object sender, SqlCommandExecutionEventArgs e)
        {
            if (e != null)
            {
                var log = new DatabaseLogInfo();
                //获取用户信息
                if (e.UserInfo != null)
                {
                    log.ClientID = e.UserInfo.ClientID;
                    log.UserID = e.UserInfo.UserID;
                }
                //获取T-SQL相关信息
                if (e.Command != null)
                {
                    TSQL tsql = new TSQL();
                    tsql.CommandText = e.Command.GenerateTSQLText();
                    if (e.Command.Connection != null)
                    {
                        tsql.DatabaseName = e.Command.Connection.Database;
                        tsql.ServerName = e.Command.Connection.DataSource;
                    }
                    tsql.ExecutionTime = e.ExecutionTime;
                    log.TSQL = tsql;
                }
                Loggers.DEFAULT.Database(log);
            }
        }
        #endregion

        #region 得到总的记录数

        ///<summary>
        /// 得到总的记录数
        ///</summary>
        /// <param name="entity">分页查询实体</param>
        ///<returns>总的记录数</returns>
        public int GetTotalCount(PagedQueryEntity entity)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT COUNT(1) FROM {0} WHERE 1 = 1 ", entity.TableName);

            if (!string.IsNullOrEmpty(entity.QueryCondition))
            {
                sql.AppendFormat(" {0} ", entity.QueryCondition);
            }
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 得到分页数据

        /// <summary>
        /// 得到分页数据
        /// </summary>
        /// <param name="entity">分页查询实体</param>
        /// <returns>数据集</returns>
        public DataSet GetPagedData(PagedQueryEntity entity)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM ( ");
            sql.AppendFormat(" SELECT {0} ", entity.QueryFieldName);    //显示字段
            sql.AppendFormat(" ,displayIndex = ROW_NUMBER() OVER(ORDER BY ");

            //排序字段，默认按照ID排序
            if (!string.IsNullOrEmpty(entity.SortField) && !string.IsNullOrEmpty(entity.SortDirection))
            {
                sql.AppendFormat(" {0} {1}) ", entity.SortField, entity.SortDirection);
            }
            else
            {
                sql.AppendFormat(" [ID] ASC) ");
            }

            sql.AppendFormat(" FROM {0} ", entity.TableName);   //表名
            sql.AppendFormat(" WHERE 1 = 1 ");
            sql.AppendFormat(" {0} ", entity.QueryCondition);   //查询条件
            sql.AppendFormat(" ) AS Temp ");

            int beginSize = entity.PageIndex * entity.PageSize + 1;
            int endSize = entity.PageIndex * entity.PageSize + entity.PageSize;

            sql.AppendFormat(" WHERE displayIndex BETWEEN '{0}' AND '{1}' ", beginSize, endSize);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
