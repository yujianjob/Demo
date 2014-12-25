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
    /// 数据访问： 0806对象图片表 ObjectImages 
    /// 表ObjectImages的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ObjectImagesDAO : BaseDAO<BasicUserInfo>, ICRUDable<ObjectImagesEntity>, IQueryable<ObjectImagesEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页车型图片列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetCarTypeIamgeList(PagedQueryEntity pageEntity,  ObjectImagesEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format("dbo.ObjectImages");
            pageEntity.QueryFieldName =
            string.Format("*");
            pageEntity.QueryCondition = string.Format("AND IsDelete = 0 AND ObjectID = {0} AND ObjectType = {1} ",queryEntity.ObjectID,queryEntity.ObjectType);//删除状态不显示            
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion
    }
}
