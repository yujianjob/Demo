/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/19 11:14:27
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
    /// 数据访问： 0809游戏奖项表  GameAwards 
    /// 表GameAwards的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class GameAwardsDAO : BaseDAO<BasicUserInfo>, ICRUDable<GameAwardsEntity>, IQueryable<GameAwardsEntity>
    {
        #region 获取分页权限游戏列表

        /// <summary>
        /// 获取分页列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetGameList(PagedQueryEntity pageEntity, GameAwardsEntity gameEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = "dbo.GameAwards g";
            pageEntity.QueryFieldName = "g.*";
            pageEntity.QueryCondition = " AND g.IsDelete = 0 ";

            if (gameEntity.GameID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND g.GameID={0} ", gameEntity.GameID);
            }

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }


        #endregion

        #region 根据游戏ID获取游戏奖项列表

        /// <summary>
        /// 根据游戏ID获取游戏奖项列表
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        public DataSet GetGameAwardsListByGameId(int gameId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.* ");
            sql.AppendFormat(" FROM dbo.GameAwards a ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.GameID = {0} ", gameId);
            sql.AppendFormat(" ORDER BY Grade ASC ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 删除游戏奖项列表

        /// <summary>
        /// 删除游戏奖项列表
        /// </summary>
        /// <param name="gameAwardsIds">奖项ID集合  "1,2,3"</param>
        public void DeleteGameAwards(string gameAwardsIds)
        {
            var sql = new StringBuilder();

            //更新奖项表
            sql.AppendFormat(" UPDATE dbo.GameAwards SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", gameAwardsIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }
}
