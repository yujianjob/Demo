/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/8 14:14:40
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
    /// 数据访问： 0602投票选项表 VoteOption 
    /// 表VoteOption的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class VoteOptionDAO : BaseDAO<BasicUserInfo>, ICRUDable<VoteOptionEntity>, IQueryable<VoteOptionEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页投票选项列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetVoteOptionList(PagedQueryEntity pageEntity, int voteId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = "dbo.VoteOption a";
            pageEntity.TableName += " LEFT JOIN dbo.Vote b ON a.VoteID=b.ID AND b.IsDelete=0 ";
            pageEntity.QueryFieldName = "a.*";
            pageEntity.QueryCondition = " AND a.IsDelete = 0 ";
            pageEntity.QueryCondition += string.Format(" AND a.VoteID = {0} ", voteId);
            pageEntity.SortField = "a." + pageEntity.SortField;
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;
        }

        #endregion
    }
}
