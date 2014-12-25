/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/13 13:42:54
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
    /// 数据访问： 0808游戏信息表  Game 
    /// 表Game的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class GameDAO : BaseDAO<BasicUserInfo>, ICRUDable<GameEntity>, IQueryable<GameEntity>
    {
        #region 获取分页权限游戏列表

        /// <summary>
        /// 获取分页列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetGameList(PagedQueryEntity pageEntity, GameEntity gameEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = "dbo.Game g";
            pageEntity.TableName += " LEFT JOIN dbo.BasicData b ON g.Type = b.Value AND b.IsDelete=0  AND b.TypeCode='GameType' ";
            pageEntity.QueryFieldName = "g.*";
            pageEntity.QueryFieldName += " , TypeName = b.Name ";
            pageEntity.QueryCondition=" AND g.IsDelete = 0 ";
            pageEntity.SortField = " g.sortindex";

            if (!string.IsNullOrEmpty(gameEntity.Name))
            {
                pageEntity.QueryCondition += string.Format(" AND g.Name like '%{0}%' ", gameEntity.Name);
            }

            if (!string.IsNullOrEmpty(gameEntity.Type))
            {
                if (gameEntity.Type != "-1")
                {
                    pageEntity.QueryCondition += string.Format(" AND g.Type={0} ", gameEntity.Type);
                }
            }

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }


        #endregion

        #region 获取游戏分页权限

        /// <summary>
        /// 获取游戏分页权限
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetGameLists(PagedQueryEntity pageEntity, string searchText, GameEntity gameEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " Game t";
            pageEntity.QueryCondition = " and t.IsDelete = 0 ";
            pageEntity.QueryFieldName = " t.*  ";

            if (!string.IsNullOrEmpty(searchText))
            {
                pageEntity.QueryCondition += string.Format(" AND t.Name like '%{0}%' ", searchText);
            }

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }


        #endregion

        #region 根据内容库ID，获取游戏列表

        /// <summary>
        /// 根据内容库ID，获取游戏列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetGameLists(PagedQueryEntity pageEntity, string searchText, GameEntity gameEntity, int selectIds)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " dbo.Game g ";
            pageEntity.TableName += " LEFT JOIN dbo.ContentLibraryGameMapping c ON c.GameID=g.ID AND c.IsDelete = 0 ";

            pageEntity.QueryCondition = " AND g.IsDelete = 0 ";
            pageEntity.QueryFieldName = " g.*  ";
            pageEntity.SortField = "g." + pageEntity.SortField;

            if (!string.IsNullOrEmpty(searchText))
            {
                pageEntity.QueryCondition += string.Format(" AND g.Name like '%{0}%' ", searchText);
            }
            pageEntity.QueryCondition += string.Format(" AND c.ContentLibraryID={0} ", selectIds);

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }


        #endregion

        #region 根据ContentLibraryID获取关系信息

        /// <summary>
        /// 根据ContentLibraryID获取关系信息
        /// </summary>
        public DataSet GetContentLibraryGameMappingByContentLibraryID(int contentLibraryId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.ContentLibraryGameMapping WHERE IsDelete=0 AND ContentLibraryID = {0} ", contentLibraryId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }


        public DataSet GetContentLibraryGameMappingByContentLibraryIDs(int contentLibraryId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT GameID FROM dbo.ContentLibraryGameMapping ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND ContentLibraryID = {0} ", contentLibraryId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 删除游戏列表
        /// <summary>
        /// 删除游戏列表
        /// </summary>
        /// <param name="id">游戏ID集合</param>
        public void DeleteGame(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" UPDATE dbo.Game SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", id);
                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }


        #endregion

        #region 查询重复数据
        public DataTable queryGame(string inputName, int gameId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("select * from game where Name ='{0}'", inputName);
            sql.AppendFormat(" and ID!={0} and IsDelete=0 ", gameId);

            DataSet ds = SQLHelper.ExecuteDataset(sql.ToString());
            return ds.Tables[0];

        }
        #endregion

        #region 全部数据包括为1的

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public GameEntity GetByIDs(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Game] where ID='{0}' ", id.ToString());
            //读取数据
            GameEntity m = null;
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    this.Load(rdr, out m);
                    break;
                }
            }
            //返回
            return m;
        }

        #endregion
    }
}
