/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/10 14:03:22
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
    /// 数据访问： 1502资讯 Information 
    /// 表Information的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class InformationDAO : BaseDAO<BasicUserInfo>, ICRUDable<InformationEntity>, IQueryable<InformationEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetInformationList(PagedQueryEntity entity, InformationEntity informationEntity, int parentId, int id, int clientId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Information a";
            entity.TableName += " LEFT JOIN dbo.InformationType b ON a.InformationTypeID = b.ID AND b.IsDelete=0  ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , InformationTypeIDName = b.Name ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            entity.SortField = "a." + entity.SortField;

            if (!string.IsNullOrEmpty(informationEntity.Title))
            {
                entity.QueryCondition += string.Format(" AND a.Title LIKE '%{0}%' ", informationEntity.Title);
            }
            if (clientId != 0)
            {
                if (clientId != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.ClientID={0} ", clientId);
                }
            }
            if (informationEntity.InformationTypeID != 0)
            {
                if (informationEntity.InformationTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.InformationTypeID = {0} ", informationEntity.InformationTypeID);
                }
                else
                {
                    if (parentId != 0)
                    {
                        entity.QueryCondition += string.Format(" AND a.InformationTypeID IN (SELECT c.ID FROM dbo.InformationType c WHERE c.IsDelete=0  AND c.ParentID = {0} OR c.ID = {1}) ", parentId, id);
                    }
                }
            }
            //entity.QueryCondition += string.Format(" AND b.ParentID={0} OR b.ID={1}  ", parentId, id);
            entity.SortField = "a.SortIndex";

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 删除资讯

        /// <summary>
        /// 删除资讯
        /// </summary>
        /// <param name="informationId">资讯ID</param>
        public void DeleteInformation(string informationId)
        {
            if (!string.IsNullOrEmpty(informationId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Information SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", informationId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
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

        #region 根据区域经销商获取分页列表

        public PagedQueryObjectResult<DataSet> GetInformationLists(PagedQueryEntity entity, InformationEntity informationEntity, int businessId, int dealerId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Information a";
            entity.TableName += " LEFT JOIN dbo.InformationType b ON a.InformationTypeID = b.ID AND b.IsDelete=0  ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , InformationTypeIDName = b.Name ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            entity.SortField = "a." + entity.SortField;

            if (!string.IsNullOrEmpty(informationEntity.Title))
            {
                entity.QueryCondition += string.Format(" AND a.Title LIKE '%{0}%' ", informationEntity.Title);
            }
          
            if (informationEntity.InformationTypeID != 0)
            {
                if (informationEntity.InformationTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.InformationTypeID = {0} ", informationEntity.InformationTypeID);
                }
            }

            if (businessId != -1)
            {
                if (dealerId != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.ClientID={0} ", dealerId);
                }
                if (dealerId == -1)
                {
                    entity.QueryCondition += string.Format(" AND a.ClientID IN (SELECT t.ID FROM dbo.Client t WHERE t.ParentID={0} OR t.ID ={0} AND t.IsDelete=0)", businessId);
                }
            }

            entity.SortField = "a.SortIndex";

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 

        /// <summary>
        /// 根据父类ID获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetInformationsList(PagedQueryEntity entity, InformationEntity informationEntity, int parentId, int id)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Information a";
            entity.TableName += " LEFT JOIN dbo.InformationType b ON a.InformationTypeID = b.ID AND b.IsDelete=0  ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , InformationTypeIDName = b.Name ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            entity.SortField = "a." + entity.SortField;

            if (!string.IsNullOrEmpty(informationEntity.Title))
            {
                entity.QueryCondition += string.Format(" AND a.Title LIKE '%{0}%' ", informationEntity.Title);
            }
            if (informationEntity.InformationTypeID != 0)
            {
                if (informationEntity.InformationTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.InformationTypeID = {0} ", informationEntity.InformationTypeID);
                }
                else
                {
                    if (parentId != 0)
                    {
                        entity.QueryCondition += string.Format(" AND a.InformationTypeID IN (SELECT c.ID FROM dbo.InformationType c WHERE c.IsDelete=0  AND c.ParentID = {0} OR c.ID = {1}) ", parentId, id);
                    }
                }
            }

            entity.QueryCondition += string.Format(" AND a.ClientID={0} ", CurrentUserInfo.ClientID);

            entity.SortField = "a.SortIndex";

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 获取资讯表里面数据

        /// <summary>
        /// 获取资讯表里面数据
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetInformationTypeList(PagedQueryEntity entity, string searchText, string clientType, string informationType)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Information a";
            entity.TableName += " LEFT JOIN dbo.InformationType b ON a.InformationTypeID = b.ID AND b.IsDelete=0  ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , InformationTypeIDName = b.Name ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            entity.SortField = "a." + entity.SortField;
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (a.Title LIKE '%{0}%') ", searchText);
            }
            if (!string.IsNullOrEmpty(clientType))
            {
                if (clientType == "-1")
                {
                    entity.QueryCondition += string.Format(" AND a.ClientID IN ( SELECT ID FROM dbo.Client c WHERE c.IsDelete=0 AND c.Type=1 ) ");
                }
                if (clientType != "-1")
                {
                    entity.QueryCondition += string.Format(" AND a.ClientID={0} ", clientType);
                }
            }
            if (!string.IsNullOrEmpty(informationType))
            {
                if (informationType != "-1")
                {
                    entity.QueryCondition += string.Format(" AND a.InformationTypeID={0} ", informationType);
                }
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

    }
}
