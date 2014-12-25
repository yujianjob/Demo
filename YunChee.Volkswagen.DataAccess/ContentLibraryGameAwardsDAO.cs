/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/19 13:40:50
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
    /// 数据访问： 0406内容库游戏奖项表  ContentLibraryGameAwards 
    /// 表ContentLibraryGameAwards的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ContentLibraryGameAwardsDAO : BaseDAO<BasicUserInfo>, ICRUDable<ContentLibraryGameAwardsEntity>, IQueryable<ContentLibraryGameAwardsEntity>
    {
        #region 根据游戏ID获取游戏奖项列表

        /// <summary>
        /// 根据游戏ID获取游戏奖项列表
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        public DataSet GetContentGameAwardsListByMappingId(int mappingId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.* ");
            sql.AppendFormat(" FROM dbo.ContentLibraryGameAwards a ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.MappingID = {0} ", mappingId);
            sql.AppendFormat(" ORDER BY Grade ASC  ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 删除游戏奖项列表

        /// <summary>
        /// 删除游戏奖项列表
        /// </summary>
        /// <param name="gameAwardsIds">奖项ID集合  "1,2,3"</param>
        public void DeleteContentGameAwards(string gameAwardsIds)
        {
            var sql = new StringBuilder();

            //更新奖项表
            sql.AppendFormat(" UPDATE dbo.ContentLibraryGameAwards SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", gameAwardsIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region   根据关系获取游戏奖项ID

        public DataSet GetContentLibraryGameAwardsEntityByGameId(int mappingId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.ContentLibraryGameAwards ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND MappingID = {0} ", mappingId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
