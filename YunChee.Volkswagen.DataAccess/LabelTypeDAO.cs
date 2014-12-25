/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/26 9:42:16
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
    /// 数据访问： 1602标签类型表 LabelType 
    /// 表LabelType的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class LabelTypeDAO : BaseDAO<BasicUserInfo>, ICRUDable<LabelTypeEntity>, IQueryable<LabelTypeEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetLableTypeList(PagedQueryEntity entity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.LabelType";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            //entity.QueryCondition += string.Format(" AND ClientID = {0} ", this.CurrentUserInfo.ClientID);
            entity.SortField = " SortIndex";
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 删除标签类型

        /// <summary>
        /// 删除标签类型
        /// </summary>
        /// <param name="lableTypeId">标签类型ID集合  "1,2,3"</param>
        public void DeleteLableType(string lableTypeId)
        {
            if (!string.IsNullOrEmpty(lableTypeId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.LabelType SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", lableTypeId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion
    }
}
