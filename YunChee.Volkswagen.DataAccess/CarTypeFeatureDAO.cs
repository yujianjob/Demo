/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/20 10:54:10
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
    /// 数据访问： 0303车型特点表  CarTypeFeature 
    /// 表CarTypeFeature的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CarTypeFeatureDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarTypeFeatureEntity>, IQueryable<CarTypeFeatureEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页车型特点列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetCarTypeFeatureList(PagedQueryEntity pageEntity, CarTypeFeatureEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format("dbo.CarTypeFeature");
            pageEntity.QueryFieldName =
            string.Format("*");
            pageEntity.QueryCondition = string.Format("AND IsDelete = 0 AND CarTypeID = {0} ",queryEntity.CarTypeID);//删除状态不显示            
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion

        #region 获取车型特点列表(根据车型ID)

        /// <summary>
        /// 获取车型特点列表(根据车型ID)
        /// </summary>
        /// <param name="carTypeID">车型ID</param>
        /// <returns></returns>
        public DataSet GetCarTypeFeatureList(int carTypeID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT FeatureID=id,Title,ImageUrl,Description,CarTypeID ");
            sql.AppendFormat(" FROM dbo.CarTypeFeature ");
            sql.AppendFormat(" WHERE IsDelete=0 ");
            sql.AppendFormat(" AND CarTypeID = {0} ", carTypeID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
