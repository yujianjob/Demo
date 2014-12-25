/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/24 15:48:54
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
    /// 数据访问： 1002图文素材 WNews 
    /// 表WNews的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WNewsDAO : BaseDAO<BasicUserInfo>, ICRUDable<WNewsEntity>, IQueryable<WNewsEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetWNewsList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.WNews";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND ClientID = {0} ", this.CurrentUserInfo.ClientID);
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Title LIKE '%{0}%') ", searchText);   
            }
            entity.SortField = " SortIndex";
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 删除图文素材

        /// <summary>
        /// 删除图文素材
        /// </summary>
        /// <param name="quesIds">图文素材ID集合  "1,2,3"</param>
        public void DeleteWNews(string wnewsID)
        {
            if (!string.IsNullOrEmpty(wnewsID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.WNews SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", wnewsID);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 根据图文素材ID获取图文信息

        /// <summary>
        /// 根据图文素材ID获取图文信息
        /// </summary>
        public DataSet GetWNewsListId(int NewsId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.WNews WHERE ID = {0} AND IsDelete = 0", NewsId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
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


        #region 获取WeixinID

        /// <summary>
        /// 获取WeixinID
        /// </summary>
        public string GetWApplicationWeixinID(int clientId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@" SELECT TOP 1 WeixinID FROM WApplication WHERE ClientID={0} AND IsDelete=0", clientId);
            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion
    }
}
