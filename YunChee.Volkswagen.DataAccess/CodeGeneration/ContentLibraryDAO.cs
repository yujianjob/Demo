/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/13 10:15:35
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
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ContentLibraryDAO : BaseDAO<BasicUserInfo>, ICRUDable<ContentLibraryEntity>, IQueryable<ContentLibraryEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public ContentLibraryDAO(BasicUserInfo pUserInfo)
            : base(pUserInfo, ConfigInfo.CURRENT_CONNECTION_STRING_MANAGER)
        {
            this.SQLHelper.OnExecuted += new EventHandler<SqlCommandExecutionEventArgs>(SQLHelper_OnExecuted);
        }
        #endregion

        #region 事件处理
        /// <summary>
        /// SQL助手执行完毕后，记录日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SQLHelper_OnExecuted(object sender, SqlCommandExecutionEventArgs e)
        {
            if (e != null)
            {
                var log = new DatabaseLogInfo();
                //获取用户信息
                if (e.UserInfo != null)
                {
                    log.ClientID = e.UserInfo.ClientID;
                    log.UserID = e.UserInfo.UserID;
                }
                //获取T-SQL相关信息
                if (e.Command != null)
                {
                    TSQL tsql = new TSQL();
                    tsql.CommandText = e.Command.GenerateTSQLText();
                    if (e.Command.Connection != null)
                    {
                        tsql.DatabaseName = e.Command.Connection.Database;
                        tsql.ServerName = e.Command.Connection.DataSource;
                    }
                    tsql.ExecutionTime = e.ExecutionTime;
                    log.TSQL = tsql;
                }
                Loggers.DEFAULT.Database(log);
            }
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(ContentLibraryEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(ContentLibraryEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //初始化固定字段
            pEntity.CreateTime = DateTime.Now;
            pEntity.CreateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.LastUpdateTime = pEntity.CreateTime;
            pEntity.LastUpdateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.IsDelete = 0;

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
               result= this.SQLHelper.ExecuteScalar((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.ID = Convert.ToInt32(result);
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public ContentLibraryEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [ContentLibrary] where ID='{0}' and IsDelete=0 ", id.ToString());
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

        /// <summary>
        /// 获取所有实例
        /// </summary>
        /// <returns></returns>
        public ContentLibraryEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [ContentLibrary] where isdelete=0");
            //读取数据
            List<ContentLibraryEntity> list = new List<ContentLibraryEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    ContentLibraryEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            //返回
            return list.ToArray();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(ContentLibraryEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(ContentLibraryEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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

            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [ContentLibrary] set ");
                        if (pIsUpdateNullField || pEntity.ClientType!=null)
                strSql.Append( "[ClientType]=@ClientType,");
            if (pIsUpdateNullField || pEntity.RegionFirstTypeID!=null)
                strSql.Append( "[RegionFirstTypeID]=@RegionFirstTypeID,");
            if (pIsUpdateNullField || pEntity.RegionSecondTypeID!=null)
                strSql.Append( "[RegionSecondTypeID]=@RegionSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerFirstTypeID!=null)
                strSql.Append( "[DealerFirstTypeID]=@DealerFirstTypeID,");
            if (pIsUpdateNullField || pEntity.DealerSecondTypeID!=null)
                strSql.Append( "[DealerSecondTypeID]=@DealerSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerLastTypeID!=null)
                strSql.Append( "[DealerLastTypeID]=@DealerLastTypeID,");
            if (pIsUpdateNullField || pEntity.ContentApplicationScope!=null)
                strSql.Append( "[ContentApplicationScope]=@ContentApplicationScope,");
            if (pIsUpdateNullField || pEntity.ContentShowScope!=null)
                strSql.Append( "[ContentShowScope]=@ContentShowScope,");
            if (pIsUpdateNullField || pEntity.Title!=null)
                strSql.Append( "[Title]=@Title,");
            if (pIsUpdateNullField || pEntity.Description!=null)
                strSql.Append( "[Description]=@Description,");
            if (pIsUpdateNullField || pEntity.PictureUrl!=null)
                strSql.Append( "[PictureUrl]=@PictureUrl,");
            if (pIsUpdateNullField || pEntity.Content!=null)
                strSql.Append( "[Content]=@Content,");
            if (pIsUpdateNullField || pEntity.SortIndex!=null)
                strSql.Append( "[SortIndex]=@SortIndex,");
            if (pIsUpdateNullField || pEntity.Enabled!=null)
                strSql.Append( "[Enabled]=@Enabled,");
            if (pIsUpdateNullField || pEntity.OriginalUrl!=null)
                strSql.Append( "[OriginalUrl]=@OriginalUrl,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ContentScenario!=null)
                strSql.Append( "[ContentScenario]=@ContentScenario,");
            if (pIsUpdateNullField || pEntity.VideoUrl!=null)
                strSql.Append( "[VideoUrl]=@VideoUrl,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
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
			parameters[20].Value = pEntity.ID;

            //执行语句
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Update(ContentLibraryEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(ContentLibraryEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(ContentLibraryEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.ID.HasValue)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.ID.Value, pTran);           
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pID">标识符的值</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(object pID, IDbTransaction pTran)
        {
            if (pID == null)
                return ;   
            //组织参数化SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [ContentLibrary] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@LastUpdateTime",SqlDbType=SqlDbType.DateTime,Value=DateTime.Now},
                new SqlParameter{ParameterName="@LastUpdateBy",SqlDbType=SqlDbType.Int,Value=Convert.ToInt32(CurrentUserInfo.UserID)},
                new SqlParameter{ParameterName="@ID",SqlDbType=SqlDbType.VarChar,Value=pID}
            };
            //执行语句
            int result = 0;
            if (pTran != null)
                result=this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, sql.ToString(), parameters);
            else
                result=this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters);
            return ;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(ContentLibraryEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (!pEntity.ID.HasValue)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.ID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(ContentLibraryEntity[] pEntities)
        { 
            Delete(pEntities, null);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pIDs">标识符值数组</param>
        public void Delete(object[] pIDs)
        {
            Delete(pIDs,null);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pIDs">标识符值数组</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(object[] pIDs, IDbTransaction pTran) 
        {
            if (pIDs == null || pIDs.Length==0)
                return ;
            //组织参数化SQL
            StringBuilder primaryKeys = new StringBuilder();
            foreach (object item in pIDs)
            {
                primaryKeys.AppendFormat("{0},",item.ToString());
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [ContentLibrary] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
            //执行语句
            int result = 0;   
            if (pTran == null)
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), null);
            else
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran,CommandType.Text, sql.ToString());       
        }
        #endregion

        #region IQueryable 成员
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="pWhereConditions">筛选条件</param>
        /// <param name="pOrderBys">排序</param>
        /// <returns></returns>
        public ContentLibraryEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [ContentLibrary] where isdelete=0 ");
            if (pWhereConditions != null)
            {
                foreach (var item in pWhereConditions)
                {
                    sql.AppendFormat(" and {0}", item.GetExpression());
                }
            }
            if (pOrderBys != null && pOrderBys.Length > 0)
            {
                sql.AppendFormat(" order by ");
                foreach (var item in pOrderBys)
                {
                    sql.AppendFormat(" {0} {1},", item.FieldName, item.Direction == OrderByDirections.Asc ? "asc" : "desc");
                }
                sql.Remove(sql.Length - 1, 1);
            }
            //执行SQL
            List<ContentLibraryEntity> list = new List<ContentLibraryEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    ContentLibraryEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            //返回结果
            return list.ToArray();
        }
        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="pWhereConditions">筛选条件</param>
        /// <param name="pOrderBys">排序</param>
        /// <param name="pPageSize">每页的记录数</param>
        /// <param name="pCurrentPageIndex">以0开始的当前页码</param>
        /// <returns></returns>
        public PagedQueryResult<ContentLibraryEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            //组织SQL
            StringBuilder pagedSql = new StringBuilder();
            StringBuilder totalCountSql = new StringBuilder();
            //分页SQL
            pagedSql.AppendFormat("select * from (select row_number()over( order by ");
            if (pOrderBys != null && pOrderBys.Length > 0)
            {
                foreach (var item in pOrderBys)
                {
                    if(item!=null)
                    {
                        pagedSql.AppendFormat(" {0} {1},", StringUtils.WrapperSQLServerObject(item.FieldName), item.Direction == OrderByDirections.Asc ? "asc" : "desc");
                    }
                }
                pagedSql.Remove(pagedSql.Length - 1, 1);
            }
            else
            {
                pagedSql.AppendFormat(" [ID] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [ContentLibrary] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [ContentLibrary] where isdelete=0 ");
            //过滤条件
            if (pWhereConditions != null)
            {
                foreach (var item in pWhereConditions)
                {
                    if(item!=null)
                    {
                        pagedSql.AppendFormat(" and {0}", item.GetExpression());
                        totalCountSql.AppendFormat(" and {0}", item.GetExpression());
                    }
                }
            }
            pagedSql.AppendFormat(") as A ");
            //取指定页的数据
            pagedSql.AppendFormat(" where ___rn >{0} and ___rn <={1}", pPageSize * (pCurrentPageIndex-1), pPageSize * (pCurrentPageIndex));
            //执行语句并返回结果
            PagedQueryResult<ContentLibraryEntity> result = new PagedQueryResult<ContentLibraryEntity>();
            List<ContentLibraryEntity> list = new List<ContentLibraryEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    ContentLibraryEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            result.Entities = list.ToArray();
            int totalCount = Convert.ToInt32(this.SQLHelper.ExecuteScalar(totalCountSql.ToString()));    //计算总行数
            result.RowCount = totalCount;
            int remainder = 0;
            result.PageCount = Math.DivRem(totalCount, pPageSize, out remainder);
            if (remainder > 0)
                result.PageCount++;
            return result;
        }

        /// <summary>
        /// 根据实体条件查询实体
        /// </summary>
        /// <param name="pQueryEntity">以实体形式传入的参数</param>
        /// <param name="pOrderBys">排序组合</param>
        /// <returns>符合条件的实体集</returns>
        public ContentLibraryEntity[] QueryByEntity(ContentLibraryEntity pQueryEntity, OrderBy[] pOrderBys)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity(pQueryEntity);
            return Query(queryWhereCondition,  pOrderBys);            
        }

        /// <summary>
        /// 分页根据实体条件查询实体
        /// </summary>
        /// <param name="pQueryEntity">以实体形式传入的参数</param>
        /// <param name="pOrderBys">排序组合</param>
        /// <returns>符合条件的实体集</returns>
        public PagedQueryResult<ContentLibraryEntity> PagedQueryByEntity(ContentLibraryEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity( pQueryEntity);
            return PagedQuery(queryWhereCondition, pOrderBys, pPageSize, pCurrentPageIndex);
        }

        #endregion

        #region 工具方法
        /// <summary>
        /// 根据实体非Null属性生成查询条件。
        /// </summary>
        /// <returns></returns>
        protected IWhereCondition[] GetWhereConditionByEntity(ContentLibraryEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.ClientType!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientType", Value = pQueryEntity.ClientType });
            if (pQueryEntity.RegionFirstTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "RegionFirstTypeID", Value = pQueryEntity.RegionFirstTypeID });
            if (pQueryEntity.RegionSecondTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "RegionSecondTypeID", Value = pQueryEntity.RegionSecondTypeID });
            if (pQueryEntity.DealerFirstTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DealerFirstTypeID", Value = pQueryEntity.DealerFirstTypeID });
            if (pQueryEntity.DealerSecondTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DealerSecondTypeID", Value = pQueryEntity.DealerSecondTypeID });
            if (pQueryEntity.DealerLastTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DealerLastTypeID", Value = pQueryEntity.DealerLastTypeID });
            if (pQueryEntity.ContentApplicationScope!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ContentApplicationScope", Value = pQueryEntity.ContentApplicationScope });
            if (pQueryEntity.ContentShowScope!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ContentShowScope", Value = pQueryEntity.ContentShowScope });
            if (pQueryEntity.Title!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Title", Value = pQueryEntity.Title });
            if (pQueryEntity.Description!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Description", Value = pQueryEntity.Description });
            if (pQueryEntity.PictureUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PictureUrl", Value = pQueryEntity.PictureUrl });
            if (pQueryEntity.Content!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Content", Value = pQueryEntity.Content });
            if (pQueryEntity.SortIndex!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SortIndex", Value = pQueryEntity.SortIndex });
            if (pQueryEntity.Enabled!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Enabled", Value = pQueryEntity.Enabled });
            if (pQueryEntity.OriginalUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "OriginalUrl", Value = pQueryEntity.OriginalUrl });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
            if (pQueryEntity.ContentScenario!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ContentScenario", Value = pQueryEntity.ContentScenario });
            if (pQueryEntity.VideoUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VideoUrl", Value = pQueryEntity.VideoUrl });
            if (pQueryEntity.CreateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
            if (pQueryEntity.CreateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.LastUpdateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateBy", Value = pQueryEntity.LastUpdateBy });
            if (pQueryEntity.LastUpdateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateTime", Value = pQueryEntity.LastUpdateTime });
            if (pQueryEntity.IsDelete!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(SqlDataReader pReader, out ContentLibraryEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new ContentLibraryEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ID"] != DBNull.Value)
			{
				pInstance.ID =   Convert.ToInt32(pReader["ID"]);
			}
			if (pReader["ClientType"] != DBNull.Value)
			{
				pInstance.ClientType =  Convert.ToString(pReader["ClientType"]);
			}
			if (pReader["RegionFirstTypeID"] != DBNull.Value)
			{
				pInstance.RegionFirstTypeID =   Convert.ToInt32(pReader["RegionFirstTypeID"]);
			}
			if (pReader["RegionSecondTypeID"] != DBNull.Value)
			{
				pInstance.RegionSecondTypeID =   Convert.ToInt32(pReader["RegionSecondTypeID"]);
			}
			if (pReader["DealerFirstTypeID"] != DBNull.Value)
			{
				pInstance.DealerFirstTypeID =   Convert.ToInt32(pReader["DealerFirstTypeID"]);
			}
			if (pReader["DealerSecondTypeID"] != DBNull.Value)
			{
				pInstance.DealerSecondTypeID =   Convert.ToInt32(pReader["DealerSecondTypeID"]);
			}
			if (pReader["DealerLastTypeID"] != DBNull.Value)
			{
				pInstance.DealerLastTypeID =   Convert.ToInt32(pReader["DealerLastTypeID"]);
			}
			if (pReader["ContentApplicationScope"] != DBNull.Value)
			{
				pInstance.ContentApplicationScope =  Convert.ToString(pReader["ContentApplicationScope"]);
			}
			if (pReader["ContentShowScope"] != DBNull.Value)
			{
				pInstance.ContentShowScope =  Convert.ToString(pReader["ContentShowScope"]);
			}
			if (pReader["Title"] != DBNull.Value)
			{
				pInstance.Title =  Convert.ToString(pReader["Title"]);
			}
			if (pReader["Description"] != DBNull.Value)
			{
				pInstance.Description =  Convert.ToString(pReader["Description"]);
			}
			if (pReader["PictureUrl"] != DBNull.Value)
			{
				pInstance.PictureUrl =  Convert.ToString(pReader["PictureUrl"]);
			}
			if (pReader["Content"] != DBNull.Value)
			{
				pInstance.Content =  Convert.ToString(pReader["Content"]);
			}
			if (pReader["SortIndex"] != DBNull.Value)
			{
				pInstance.SortIndex =   Convert.ToInt32(pReader["SortIndex"]);
			}
			if (pReader["Enabled"] != DBNull.Value)
			{
				pInstance.Enabled =  Convert.ToString(pReader["Enabled"]);
			}
			if (pReader["OriginalUrl"] != DBNull.Value)
			{
				pInstance.OriginalUrl =  Convert.ToString(pReader["OriginalUrl"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
			}
			if (pReader["ContentScenario"] != DBNull.Value)
			{
				pInstance.ContentScenario =  Convert.ToString(pReader["ContentScenario"]);
			}
			if (pReader["VideoUrl"] != DBNull.Value)
			{
				pInstance.VideoUrl =  Convert.ToString(pReader["VideoUrl"]);
			}
			if (pReader["CreateBy"] != DBNull.Value)
			{
				pInstance.CreateBy =   Convert.ToInt32(pReader["CreateBy"]);
			}
			if (pReader["CreateTime"] != DBNull.Value)
			{
				pInstance.CreateTime =  Convert.ToDateTime(pReader["CreateTime"]);
			}
			if (pReader["LastUpdateBy"] != DBNull.Value)
			{
				pInstance.LastUpdateBy =   Convert.ToInt32(pReader["LastUpdateBy"]);
			}
			if (pReader["LastUpdateTime"] != DBNull.Value)
			{
				pInstance.LastUpdateTime =  Convert.ToDateTime(pReader["LastUpdateTime"]);
			}
			if (pReader["IsDelete"] != DBNull.Value)
			{
				pInstance.IsDelete =   Convert.ToInt32(pReader["IsDelete"]);
			}

        }
        #endregion
    }
}
