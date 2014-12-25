/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/23 15:00:25
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
    /// 数据访问： 1009微信群发记录表 WMassSendRecord 
    /// 表WMassSendRecord的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WMassSendRecordDAO : BaseDAO<BasicUserInfo>, ICRUDable<WMassSendRecordEntity>, IQueryable<WMassSendRecordEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetMassSendRecordList(PagedQueryEntity entity, int applicationId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.WMassSendRecord a";
            entity.TableName += " LEFT JOIN dbo.BasicData b ON a.ReplyTypeID = b.Value AND b.IsDelete=0  AND b.TypeCode='ReplyType' ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , ReplyTypeIDs = b.Name ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND a.ApplicationID = {0} ", applicationId);
            //entity.QueryCondition += " order by a.CreateTime desc ";
            entity.SortField = "a." + entity.SortField;
            if (entity.SortField.Equals("a.ReplyTypeIDs"))
                entity.SortField = "b.Name";
            //if (!string.IsNullOrEmpty(applicationId))
            //{
            //    entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%' OR EnglishName LIKE '%{0}%') ", searchText);
            //}

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region  获取图文素材数据

        /// <summary>
        /// 获取图文素材数据
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetSendRecordList(PagedQueryEntity entity, string sendId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.WNews";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND id in({0}) ", sendId);
            entity.SortField = "SortIndex";
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion


        #region 根据性别，客户类型获取ID集合

        /// <summary>
        /// 根据性别，客户类型ID获取集合
        /// </summary>
        /// <param name="Sex">性别</param>
        /// <param name="type">客户类型</param>
        /// <returns></returns>
        public DataSet GetCustomeListByCustomeId(string wxSex, string type)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT WxOpenId FROM dbo.Customer ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            if (type != "-1")
            {
                sql.AppendFormat(" AND Type = {0} ", type);
            }

            if (wxSex != "-1")
            {
                sql.AppendFormat(" AND WxSex = {0} ", wxSex);
            }

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
        
        #region 获取每月已发送的次数

        /// <summary>
        /// 获取每月已发送的次数
        /// </summary>
        public string GetSendRecordLists(int sendId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
          SELECT ( SELECT    ConfigValue
          FROM      dbo.Configs
          WHERE     ConfigKey = 'MonthMassSendCount'
                    AND IsDelete = 0
        ) - ( SELECT    COUNT(*)
              FROM      dbo.WMassSendRecord
              WHERE     DATEDIFF(MONTH, CreateTime, GETDATE()) = 0
                        AND ApplicationID = {0}
                        AND IsDelete = 0
            )", sendId);

            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion


        #region  获取图文素材ID

        /// <summary>
        /// 获取图文素材ID
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetMassSendRecordListId(PagedQueryEntity entity, int sendId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = string.Format(@"
                   dbo.WMassSendRecord a ,
                        dbo.WNewsMapping b ,
                        dbo.WNews c ");
            entity.QueryFieldName = string.Format("c.*");
            StringBuilder strcondition = new StringBuilder();
            strcondition.AppendFormat(" AND b.ObjectID = a.ID AND b.NewsID = c.ID ");
            strcondition.AppendFormat(" AND a.IsDelete = 0 AND b.IsDelete = 0 AND c.IsDelete = 0 ");
            strcondition.AppendFormat(" AND a.ReplyTypeID=2 AND b.TypeID=5 ");
            strcondition.AppendFormat(" AND a.ID={0}", sendId);
            entity.QueryCondition = strcondition.ToString();
            entity.SortField = "c." + entity.SortField;
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

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
    }
}
