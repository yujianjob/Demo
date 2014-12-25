/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/1 0:13:02
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
    /// 数据访问： 0401内容库 ContentLibrary 
    /// 表ContentLibrary的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ContentLibraryDAO : BaseDAO<BasicUserInfo>, ICRUDable<ContentLibraryEntity>, IQueryable<ContentLibraryEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetContentLibraryList(PagedQueryEntity entity, ContentLibraryEntity contentLibraryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ContentLibrary a";
            entity.TableName += " LEFT JOIN dbo.BasicData b ON a.ClientType = b.Value AND b.IsDelete=0  AND b.TypeCode='ClientType' ";
            entity.TableName += " LEFT JOIN dbo.BasicData c ON a.Enabled = c.Value AND c.IsDelete=0  AND c.TypeCode='YesOrNo' ";
            entity.TableName += " LEFT JOIN dbo.BasicData e ON a.ContentShowScope = e.Value AND e.IsDelete=0  AND e.TypeCode='ContentShowScope' ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , ClientTypeName = b.Name ";
            entity.QueryFieldName += " , EnabledName = c.Name ";
            entity.QueryFieldName += " , TemplateName = e.Name ";
            entity.QueryFieldName += " , RegionSecondName = (CASE a.ClientType  ";
            entity.QueryFieldName += " WHEN '1' THEN  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID) END  ";
            entity.QueryFieldName += " WHEN '2' THEN ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID) END  ";
            entity.QueryFieldName += " END) ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            entity.SortField = "a." + entity.SortField;

            if (contentLibraryEntity.ClientType != "-1")
            {
                if (contentLibraryEntity.ClientType != "0")
                {
                    entity.QueryCondition += string.Format(" AND a.ClientType = {0} ", contentLibraryEntity.ClientType);
                }
            }
            if (contentLibraryEntity.RegionFirstTypeID != 0)
            {
                if (contentLibraryEntity.RegionFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionFirstTypeID = {0} ", contentLibraryEntity.RegionFirstTypeID);
                }
            }
            if (contentLibraryEntity.RegionSecondTypeID != 0)
            {
                if (contentLibraryEntity.RegionSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionSecondTypeID = {0} ", contentLibraryEntity.RegionSecondTypeID);
                }
            }
            if (contentLibraryEntity.DealerFirstTypeID != 0)
            {
                if (contentLibraryEntity.DealerFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerFirstTypeID = {0} ", contentLibraryEntity.DealerFirstTypeID);
                }
            }
            if (contentLibraryEntity.DealerSecondTypeID != 0)
            {
                if (contentLibraryEntity.DealerSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerSecondTypeID = {0} ", contentLibraryEntity.DealerSecondTypeID);
                }
            }
            if (contentLibraryEntity.DealerLastTypeID != 0)
            {
                if (contentLibraryEntity.DealerLastTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerLastTypeID = {0} ", contentLibraryEntity.DealerLastTypeID);
                }
            }
            if (contentLibraryEntity.Enabled != "-1")
            {
                entity.QueryCondition += string.Format(" AND a.Enabled = {0} ", contentLibraryEntity.Enabled);
            }
            if (contentLibraryEntity.ContentApplicationScope != "-1")
            {
                entity.QueryCondition += string.Format(" AND (a.ContentApplicationScope LIKE '%{0}%') ", contentLibraryEntity.ContentApplicationScope);
            }

            //entity.QueryCondition += string.Format("ORDER BY CreateTime ASC ");
            if (!string.IsNullOrEmpty(contentLibraryEntity.Title))
            {
                entity.QueryCondition += string.Format(" AND (a.Title LIKE '%{0}%') ", contentLibraryEntity.Title);
            }

            entity.SortField = "a.SortIndex";
            //entity.QueryCondition += string.Format(" AND a.CreateBy = {0} ", this.CurrentUserInfo.UserID);

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 删除内容库

        /// <summary>
        /// 删除内容库
        /// </summary>
        /// <param name="contentLibraryId">内容库ID</param>
        public void DeleteContentLibrary(string contentLibraryId)
        {
            if (!string.IsNullOrEmpty(contentLibraryId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.ContentLibrary SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", contentLibraryId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 获取内容表里面数据

        /// <summary>
        /// 获取内容表里面数据
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetContentLibraryList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ContentLibrary ";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Title LIKE '%{0}%') ", searchText);
            }
            entity.QueryCondition += string.Format(" AND ContentApplicationScope LIKE '%2%' ");
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
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

        #region 获取指定活动报名

        /// <summary>
        /// 获取指定活动报名
        /// </summary>
        /// <param name="contentLibraryId">内容库ID</param>
        /// <returns></returns>
        public DataSet GetContentLibraryApplyByContentLibraryId(int contentLibraryId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.OptionValue FROM dbo.ContentLibraryApply a ");
            sql.AppendFormat(" WHERE a.ContentLibraryID = {0} ", contentLibraryId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 更新活动报名表是否删除

        ///<summary>
        /// 更新活动报名表是否删除
        ///</summary>
        ///<param name="contentLibraryId">内容库ID</param>
        /// <param name="eventApplyIds">活动报名ID集合 "1,2,3"</param>
        /// <param name="isDelete">删除标志  0=否  1=是</param>
        ///<returns>总的记录数</returns>
        public void UpdateContentLibraryApplyIsDelete(int contentLibraryId, string eventApplyIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ContentLibraryApply SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (contentLibraryId != -1)
            {
                sql.AppendFormat(" AND ContentLibraryID = {0} ", contentLibraryId);
            }
            if (!string.IsNullOrEmpty(eventApplyIds))
            {
                sql.AppendFormat(" AND OptionValue IN ({0}) ", eventApplyIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region   获取IsDelete=1操作

        /// <summary>
        /// 获取IsDelete=1操作
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public ContentLibraryEntity GetByIDs(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [ContentLibrary] where ID='{0}' ", id.ToString());
            //读取数据
            ContentLibraryEntity m = null;
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

        #region  更新IsDelete=1的操作

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Updates(ContentLibraryEntity pEntity, IDbTransaction pTran, bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.ID.HasValue)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
            //初始化固定字段
            pEntity.LastUpdateTime = DateTime.Now;
            pEntity.LastUpdateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            //pEntity.IsDelete = 1;

            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [ContentLibrary] set ");
            if (pIsUpdateNullField || pEntity.ClientType != null)
                strSql.Append("[ClientType]=@ClientType,");
            if (pIsUpdateNullField || pEntity.RegionFirstTypeID != null)
                strSql.Append("[RegionFirstTypeID]=@RegionFirstTypeID,");
            if (pIsUpdateNullField || pEntity.RegionSecondTypeID != null)
                strSql.Append("[RegionSecondTypeID]=@RegionSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerFirstTypeID != null)
                strSql.Append("[DealerFirstTypeID]=@DealerFirstTypeID,");
            if (pIsUpdateNullField || pEntity.DealerSecondTypeID != null)
                strSql.Append("[DealerSecondTypeID]=@DealerSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerLastTypeID != null)
                strSql.Append("[DealerLastTypeID]=@DealerLastTypeID,");
            if (pIsUpdateNullField || pEntity.ContentApplicationScope != null)
                strSql.Append("[ContentApplicationScope]=@ContentApplicationScope,");
            if (pIsUpdateNullField || pEntity.ContentShowScope != null)
                strSql.Append("[ContentShowScope]=@ContentShowScope,");
            if (pIsUpdateNullField || pEntity.Title != null)
                strSql.Append("[Title]=@Title,");
            if (pIsUpdateNullField || pEntity.Description != null)
                strSql.Append("[Description]=@Description,");
            if (pIsUpdateNullField || pEntity.PictureUrl != null)
                strSql.Append("[PictureUrl]=@PictureUrl,");
            if (pIsUpdateNullField || pEntity.Content != null)
                strSql.Append("[Content]=@Content,");
            if (pIsUpdateNullField || pEntity.SortIndex != null)
                strSql.Append("[SortIndex]=@SortIndex,");
            if (pIsUpdateNullField || pEntity.Enabled != null)
                strSql.Append("[Enabled]=@Enabled,");
            if (pIsUpdateNullField || pEntity.OriginalUrl != null)
                strSql.Append("[OriginalUrl]=@OriginalUrl,");
            if (pIsUpdateNullField || pEntity.Remark != null)
                strSql.Append("[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ContentScenario != null)
                strSql.Append("[ContentScenario]=@ContentScenario,");
            if (pIsUpdateNullField || pEntity.VideoUrl != null)
                strSql.Append("[VideoUrl]=@VideoUrl,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy != null)
                strSql.Append("[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime != null)
                strSql.Append("[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.IsDelete != null)
                strSql.Append("[IsDelete]=@IsDelete,");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@ClientType",SqlDbType.NVarChar),
					new SqlParameter("@RegionFirstTypeID",SqlDbType.Int),
					new SqlParameter("@RegionSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerFirstTypeID",SqlDbType.Int),
					new SqlParameter("@DealerSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerLastTypeID",SqlDbType.Int),
					new SqlParameter("@ContentApplicationScope",SqlDbType.NVarChar),
					new SqlParameter("@ContentShowScope",SqlDbType.NVarChar),
					new SqlParameter("@Title",SqlDbType.NVarChar),
					new SqlParameter("@Description",SqlDbType.NVarChar),
					new SqlParameter("@PictureUrl",SqlDbType.NVarChar),
					new SqlParameter("@Content",SqlDbType.NVarChar),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@Enabled",SqlDbType.NVarChar),
					new SqlParameter("@OriginalUrl",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ContentScenario",SqlDbType.NVarChar),
					new SqlParameter("@VideoUrl",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
                    new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@ID",SqlDbType.Int)
            };
            parameters[0].Value = pEntity.ClientType;
            parameters[1].Value = pEntity.RegionFirstTypeID;
            parameters[2].Value = pEntity.RegionSecondTypeID;
            parameters[3].Value = pEntity.DealerFirstTypeID;
            parameters[4].Value = pEntity.DealerSecondTypeID;
            parameters[5].Value = pEntity.DealerLastTypeID;
            parameters[6].Value = pEntity.ContentApplicationScope;
            parameters[7].Value = pEntity.ContentShowScope;
            parameters[8].Value = pEntity.Title;
            parameters[9].Value = pEntity.Description;
            parameters[10].Value = pEntity.PictureUrl;
            parameters[11].Value = pEntity.Content;
            parameters[12].Value = pEntity.SortIndex;
            parameters[13].Value = pEntity.Enabled;
            parameters[14].Value = pEntity.OriginalUrl;
            parameters[15].Value = pEntity.Remark;
            parameters[16].Value = pEntity.ContentScenario;
            parameters[17].Value = pEntity.VideoUrl;
            parameters[18].Value = pEntity.LastUpdateBy;
            parameters[19].Value = pEntity.LastUpdateTime;
            parameters[20].Value = pEntity.IsDelete;
            parameters[21].Value = pEntity.ID;

            //执行语句
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion

        #region 添加IsDelete=1的操作

        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Creates(ContentLibraryEntity pEntity)
        {
            this.Creates(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Creates(ContentLibraryEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");

            //初始化固定字段
            pEntity.CreateTime = DateTime.Now;
            pEntity.CreateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.LastUpdateTime = pEntity.CreateTime;
            pEntity.LastUpdateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.IsDelete = 1;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [ContentLibrary](");
            strSql.Append("[ClientType],[RegionFirstTypeID],[RegionSecondTypeID],[DealerFirstTypeID],[DealerSecondTypeID],[DealerLastTypeID],[ContentApplicationScope],[ContentShowScope],[Title],[Description],[PictureUrl],[Content],[SortIndex],[Enabled],[OriginalUrl],[Remark],[ContentScenario],[VideoUrl],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@ClientType,@RegionFirstTypeID,@RegionSecondTypeID,@DealerFirstTypeID,@DealerSecondTypeID,@DealerLastTypeID,@ContentApplicationScope,@ContentShowScope,@Title,@Description,@PictureUrl,@Content,@SortIndex,@Enabled,@OriginalUrl,@Remark,@ContentScenario,@VideoUrl,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");
            strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@ClientType",SqlDbType.NVarChar),
					new SqlParameter("@RegionFirstTypeID",SqlDbType.Int),
					new SqlParameter("@RegionSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerFirstTypeID",SqlDbType.Int),
					new SqlParameter("@DealerSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerLastTypeID",SqlDbType.Int),
					new SqlParameter("@ContentApplicationScope",SqlDbType.NVarChar),
					new SqlParameter("@ContentShowScope",SqlDbType.NVarChar),
					new SqlParameter("@Title",SqlDbType.NVarChar),
					new SqlParameter("@Description",SqlDbType.NVarChar),
					new SqlParameter("@PictureUrl",SqlDbType.NVarChar),
					new SqlParameter("@Content",SqlDbType.NVarChar),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@Enabled",SqlDbType.NVarChar),
					new SqlParameter("@OriginalUrl",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ContentScenario",SqlDbType.NVarChar),
					new SqlParameter("@VideoUrl",SqlDbType.NVarChar),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
            parameters[0].Value = pEntity.ClientType;
            parameters[1].Value = pEntity.RegionFirstTypeID;
            parameters[2].Value = pEntity.RegionSecondTypeID;
            parameters[3].Value = pEntity.DealerFirstTypeID;
            parameters[4].Value = pEntity.DealerSecondTypeID;
            parameters[5].Value = pEntity.DealerLastTypeID;
            parameters[6].Value = pEntity.ContentApplicationScope;
            parameters[7].Value = pEntity.ContentShowScope;
            parameters[8].Value = pEntity.Title;
            parameters[9].Value = pEntity.Description;
            parameters[10].Value = pEntity.PictureUrl;
            parameters[11].Value = pEntity.Content;
            parameters[12].Value = pEntity.SortIndex;
            parameters[13].Value = pEntity.Enabled;
            parameters[14].Value = pEntity.OriginalUrl;
            parameters[15].Value = pEntity.Remark;
            parameters[16].Value = pEntity.ContentScenario;
            parameters[17].Value = pEntity.VideoUrl;
            parameters[18].Value = pEntity.CreateBy;
            parameters[19].Value = pEntity.CreateTime;
            parameters[20].Value = pEntity.LastUpdateBy;
            parameters[21].Value = pEntity.LastUpdateTime;
            parameters[22].Value = pEntity.IsDelete;

            //执行并将结果回写
            object result;
            if (pTran != null)
                result = this.SQLHelper.ExecuteScalar((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            pEntity.ID = Convert.ToInt32(result);
        }

        #endregion
    }
}
