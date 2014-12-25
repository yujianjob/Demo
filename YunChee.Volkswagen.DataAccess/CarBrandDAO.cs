/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/11 9:49:44
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
    /// 数据访问： 0301车系表 CarBrand 
    /// 表CarBrand的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CarBrandDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarBrandEntity>, IQueryable<CarBrandEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetCarBrandList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.CarBrand";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            //entity.QueryCondition += string.Format("ORDER BY CreateTime ASC ");
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%' OR EnglishName LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 删除车系

        /// <summary>
        /// 删除车系
        /// </summary>
        /// <param name="quesIds">车系ID集合  "1,2,3"</param>
        public void DeleteCarBrand(string quesIds)
        {
            if (!string.IsNullOrEmpty(quesIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.CarBrand SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", quesIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 根据车系ID获取车系信息

        /// <summary>
        /// 根据车系ID获取车系信息
        /// </summary>
        public DataSet GetCarBrandByCustomerId(int carBrandId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.CarBrand WHERE ID = {0} AND IsDelete = 0", carBrandId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
