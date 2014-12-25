/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/5/28 18:58:54
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
    /// 数据访问： 0905客户权限关系表 ClientPowers 
    /// 表ClientPowers的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ClientPowersDAO : BaseDAO<BasicUserInfo>, ICRUDable<ClientPowersEntity>, IQueryable<ClientPowersEntity>
    {
        #region 获取区域经销商没有的权限列表

        /// <summary>
        /// 获取区域经销商没有的权限列表
        /// </summary>
        /// <param name="entity">查询条件</param>
        /// <param name="searchText">按权限名称模糊查询</param>
        /// <param name="powersId">区域经销商ID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetPowersList(PagedQueryEntity entity, string searchText, int powersId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

           // SELECT a.* FROM dbo.Powers a WHERE a.IsDelete=0
           //AND NOT EXISTS(SELECT * FROM dbo.ClientPowers b WHERE b.IsDelete=0 AND b.ClientID=1 AND b.PowersID=a.ID) 

            entity.TableName = "dbo.Powers a";
            entity.QueryFieldName = "a.*";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            // 排除已经属于本区域经销商的权限
            entity.QueryCondition += string.Format(" AND NOT EXISTS(SELECT * FROM dbo.ClientPowers b WHERE b.IsDelete=0 AND b.ClientID={0} AND b.PowersID=a.ID ) ", powersId);


            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (a.Name LIKE '%{0}%' OR a.GroupName LIKE '%{0}%' OR a.Title LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 获取区域经销商的权限列表

        /// <summary>
        /// 获取区域经销商的权限列表
        /// </summary>
        /// <param name="entity">查询条件</param>
        /// <param name="searchText">按权限名称模糊查询</param>
        /// <param name="powersId">区域经销商ID</param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetIsPowersList(PagedQueryEntity entity, string searchText, int powersId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            //SELECT b.* FROM dbo.ClientPowers a, dbo.Powers b WHERE b.ID=a.PowersID
            //AND a.IsDelete=0 AND b.IsDelete=0 AND a.ClientID=2

            entity.TableName = "dbo.ClientPowers a INNER JOIN dbo.Powers b ON a.PowersID = b.ID AND b.IsDelete = 0";
            entity.QueryFieldName = "b.*";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            // 本区域经销商的权限
            entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", powersId);


            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (b.Name LIKE '%{0}%' OR b.GroupName LIKE '%{0}%' OR b.Title LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 更新区域经销商权限关系表

        ///<summary>
        /// 更新区域经销商权限关系表
        ///</summary>
        ///<param name="clientId">区域经销商ID</param>
        /// <param name="powersIds">权限ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateClientPowersIsDelete(int clientId, string powersIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ClientPowers SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (clientId != -1)
            {
                sql.AppendFormat(" AND ClientID = {0} ", clientId);
            }
            if (!string.IsNullOrEmpty(powersIds))
            {
                sql.AppendFormat(" AND PowersID IN ({0}) ", powersIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取指定权限列表（包含已删除的权限）

        /// <summary>
        /// 获取指定权限列表（包含已删除的权限）
        /// </summary>
        /// <param name="clientId">区域经销商ID</param>
        /// <returns></returns>
        public DataSet GetClientPowersListByPowersId(int clientId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.PowersID FROM dbo.ClientPowers a ");
            sql.AppendFormat(" WHERE a.ClientID = {0} ", clientId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
