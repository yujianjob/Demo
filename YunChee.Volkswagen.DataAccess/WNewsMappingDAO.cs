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
using Yunchee.Volkswagen.Common.Const;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// 数据访问： 1003图文素材关系表 WNewsMapping 
    /// 表WNewsMapping的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WNewsMappingDAO : BaseDAO<BasicUserInfo>, ICRUDable<WNewsMappingEntity>, IQueryable<WNewsMappingEntity>
    {
        /// <summary>
        /// 获取未选中图文列表
        /// </summary>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetUnderWSubscriptionReply(PagedQueryEntity entity, string selectType, int clientID, string loginType, string newsMappingType, Int32? objectID)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            //区域登陆
            if (loginType == C_ClientType.REGIONAL)
            {
                //选择区域
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews news";
                    entity.QueryCondition = "  and news.IsDelete = 0 ";
                    entity.QueryCondition += "  AND news.ClientID =" + clientID + "";
                    entity.QueryCondition += " AND NOT EXISTS (SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND news.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID="+objectID+" and TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "news.SortIndex";
                    entity.QueryFieldName = " news.* ";
                }
                //选择经销商
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " And a.IsDelete = 0 AND b.ParentID = " + clientID + "  AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = " a.* ";
                }
            }
            //经销商登陆
            else
            {
                //选择区域
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " and a.IsDelete = 0 AND b.ID = (SELECT c.ParentID FROM dbo.Client c WHERE c.IsDelete = 0 AND c.ID = " + clientID + ") ";
                    entity.QueryCondition += " AND NOT EXISTS( SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE  ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";
                }

                //选择经销商
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a ";
                    entity.QueryCondition = " and a.IsDelete=0 and a.ClientID = " + clientID + " ";
                    entity.QueryCondition += " AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (Select ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";
                }

            }


            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);
            return result;
        }

        #region 获取指定角色的用户列表（包含已删除的用户）

        /// <summary>
        /// 获取_图文素材关系表 （包含已删除的用户）
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public DataSet GetWnewMappinByObjectId(int type,int ObjectId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT w.NewsID FROM dbo.WNewsMapping w ");
            sql.AppendFormat(" WHERE w.ObjectID = {0} and typeid={1}", ObjectId,type);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取消息列表的消息数量
        /// <summary>
        /// 获取消息列表的消息数量
        /// </summary>
        /// <param name="objectid">对象id</param>
        /// <param name="type">类型</param>
        /// <param name="isdelete">是否删除</param>
        /// <returns></returns>
        public int newMapping(int objectid, string type, int isdelete)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" select COUNT(*) FROM WNewsMapping where");
            sql.AppendFormat(" typeid={0} and objectid={1} and isdelete={2}", type, objectid, isdelete);
            DataSet ds = SQLHelper.ExecuteDataset(sql.ToString());
            DataTable da = ds.Tables[0];
            if (da != null && da.Rows[0][0].ToString() != "")
                return Convert.ToInt32(da.Rows[0][0]);
            else
                return 0;
        }

        #endregion

        public int maxnewMappingIndex(int objectid, string type, int isdelete)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" select MAX(SortIndex) FROM WNewsMapping where");
            sql.AppendFormat(" typeid={0} and objectid={1} and isdelete={2}", type, objectid, isdelete);
            DataSet ds = SQLHelper.ExecuteDataset(sql.ToString());
            DataTable da = ds.Tables[0];
            if (da != null && da.Rows[0][0].ToString()!="" )
                return Convert.ToInt32(da.Rows[0][0]);
            else
                return 0;
        }

      

        #region 更新图文素材表

     

        ///<summary>
        /// 更新图文素材表
        ///</summary>
        ///<param name="roleId">角色ID</param>
        /// <param name="usersIds">用户ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateWnewMappingIsDelete(int roleId, string usersId,int sortIndex, int isDelete)
        {             
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.WNewsMapping SET IsDelete = {0}, SortIndex={1},", isDelete, sortIndex);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (roleId != -1)
            {
                sql.AppendFormat(" AND ObjectID = {0} ", roleId);
            }
            if (!string.IsNullOrEmpty(usersId))
            {
                sql.AppendFormat(" AND NewsID IN ({0}) ", usersId);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 更新角色用户表是否删除

        ///<summary>
        /// 更新角色用户表是否删除
        ///</summary>
        ///<param name="roleId">角色ID</param>
        /// <param name="usersIds">用户ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateNewMappingIsDelete(int? objectID, string updateIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.WNewsMapping SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (objectID != -1)
            {
                sql.AppendFormat(" AND ObjectID = {0} ", objectID);
            }
            if (!string.IsNullOrEmpty(updateIds))
            {
                sql.AppendFormat(" AND ID IN ({0}) ", updateIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 更新微信菜单图文素材是否删除

        ///<summary>
        /// 更新微信菜单图文素材是否删除
        ///</summary>
        ///<param name="wMenuId">微信菜单ID</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateNewMappingIsDeletes(int? objectID, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.WNewsMapping SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (objectID != -1)
            {
                sql.AppendFormat(" AND ID = {0} ", objectID);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region

        public PagedQueryObjectResult<DataSet> GetUnderWSubscriptionReplyLists(PagedQueryEntity entity, string selectType, int clientID, string loginType, string newsMappingType, Int32? objectID, string wNewsId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            //区域登陆
            if (loginType == C_ClientType.REGIONAL)
            {
                //选择区域
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews news";
                    entity.QueryCondition = "  and news.IsDelete = 0 ";
                    entity.QueryCondition += "  AND news.ClientID =" + clientID + "";
                    entity.QueryCondition += " AND NOT EXISTS (SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND news.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "news.SortIndex";
                    entity.QueryFieldName = " news.* ";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND news.ID NOT IN({0}) ", wNewsId);
                    }
                }
                //选择经销商
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " And a.IsDelete = 0 AND b.ParentID = " + clientID + "  AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = " a.* ";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND a.ID NOT IN({0}) ", wNewsId);
                    }
                }
            }
            //经销商登陆
            else
            {
                //选择区域
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " and a.IsDelete = 0 AND b.ID = (SELECT c.ParentID FROM dbo.Client c WHERE c.IsDelete = 0 AND c.ID = " + clientID + ") ";
                    entity.QueryCondition += " AND NOT EXISTS( SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE  ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND a.ID NOT IN({0}) ", wNewsId);
                    }
                }

                //选择经销商
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a ";
                    entity.QueryCondition = " and a.IsDelete=0 and a.ClientID = " + clientID + " ";
                    entity.QueryCondition += " AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (Select ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND a.ID NOT IN({0}) ", wNewsId);
                    }
                }

            }


            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);
            return result;
        }

        #endregion

    }
}
