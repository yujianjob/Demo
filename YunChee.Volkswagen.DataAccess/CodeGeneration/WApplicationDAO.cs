/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/2 18:13:21
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
    /// 数据访问： 1001微信公众账号表 WApplication 
    /// 表WApplication的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WApplicationDAO : BaseDAO<BasicUserInfo>, ICRUDable<WApplicationEntity>, IQueryable<WApplicationEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public WApplicationDAO(BasicUserInfo pUserInfo)
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
        public void Create(WApplicationEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(WApplicationEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [WApplication](");
            strSql.Append("[Name],[Type],[WeixinID],[WeixinNumber],[DevelopUrl],[DevelopToken],[AppID],[AppSecret],[LoginName],[LoginPassword],[OAuthUrl],[AccessToken],[ExpirationTime],[QrcodeSeq],[QrcodeUrl],[ClientID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@Name,@Type,@WeixinID,@WeixinNumber,@DevelopUrl,@DevelopToken,@AppID,@AppSecret,@LoginName,@LoginPassword,@OAuthUrl,@AccessToken,@ExpirationTime,@QrcodeSeq,@QrcodeUrl,@ClientID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@WeixinID",SqlDbType.NVarChar),
					new SqlParameter("@WeixinNumber",SqlDbType.NVarChar),
					new SqlParameter("@DevelopUrl",SqlDbType.NVarChar),
					new SqlParameter("@DevelopToken",SqlDbType.NVarChar),
					new SqlParameter("@AppID",SqlDbType.NVarChar),
					new SqlParameter("@AppSecret",SqlDbType.NVarChar),
					new SqlParameter("@LoginName",SqlDbType.NVarChar),
					new SqlParameter("@LoginPassword",SqlDbType.NVarChar),
					new SqlParameter("@OAuthUrl",SqlDbType.NVarChar),
					new SqlParameter("@AccessToken",SqlDbType.NVarChar),
					new SqlParameter("@ExpirationTime",SqlDbType.DateTime),
					new SqlParameter("@QrcodeSeq",SqlDbType.Int),
					new SqlParameter("@QrcodeUrl",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Name;
			parameters[1].Value = pEntity.Type;
			parameters[2].Value = pEntity.WeixinID;
			parameters[3].Value = pEntity.WeixinNumber;
			parameters[4].Value = pEntity.DevelopUrl;
			parameters[5].Value = pEntity.DevelopToken;
			parameters[6].Value = pEntity.AppID;
			parameters[7].Value = pEntity.AppSecret;
			parameters[8].Value = pEntity.LoginName;
			parameters[9].Value = pEntity.LoginPassword;
			parameters[10].Value = pEntity.OAuthUrl;
			parameters[11].Value = pEntity.AccessToken;
			parameters[12].Value = pEntity.ExpirationTime;
			parameters[13].Value = pEntity.QrcodeSeq;
			parameters[14].Value = pEntity.QrcodeUrl;
			parameters[15].Value = pEntity.ClientID;
			parameters[16].Value = pEntity.CreateBy;
			parameters[17].Value = pEntity.CreateTime;
			parameters[18].Value = pEntity.LastUpdateBy;
			parameters[19].Value = pEntity.LastUpdateTime;
			parameters[20].Value = pEntity.IsDelete;

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
        public WApplicationEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [WApplication] where ID='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            WApplicationEntity m = null;
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
        public WApplicationEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [WApplication] where isdelete=0");
            //读取数据
            List<WApplicationEntity> list = new List<WApplicationEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    WApplicationEntity m;
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
        public void Update(WApplicationEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(WApplicationEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [WApplication] set ");
                        if (pIsUpdateNullField || pEntity.Name!=null)
                strSql.Append( "[Name]=@Name,");
            if (pIsUpdateNullField || pEntity.Type!=null)
                strSql.Append( "[Type]=@Type,");
            if (pIsUpdateNullField || pEntity.WeixinID!=null)
                strSql.Append( "[WeixinID]=@WeixinID,");
            if (pIsUpdateNullField || pEntity.WeixinNumber!=null)
                strSql.Append( "[WeixinNumber]=@WeixinNumber,");
            if (pIsUpdateNullField || pEntity.DevelopUrl!=null)
                strSql.Append( "[DevelopUrl]=@DevelopUrl,");
            if (pIsUpdateNullField || pEntity.DevelopToken!=null)
                strSql.Append( "[DevelopToken]=@DevelopToken,");
            if (pIsUpdateNullField || pEntity.AppID!=null)
                strSql.Append( "[AppID]=@AppID,");
            if (pIsUpdateNullField || pEntity.AppSecret!=null)
                strSql.Append( "[AppSecret]=@AppSecret,");
            if (pIsUpdateNullField || pEntity.LoginName!=null)
                strSql.Append( "[LoginName]=@LoginName,");
            if (pIsUpdateNullField || pEntity.LoginPassword!=null)
                strSql.Append( "[LoginPassword]=@LoginPassword,");
            if (pIsUpdateNullField || pEntity.OAuthUrl!=null)
                strSql.Append( "[OAuthUrl]=@OAuthUrl,");
            if (pIsUpdateNullField || pEntity.AccessToken!=null)
                strSql.Append( "[AccessToken]=@AccessToken,");
            if (pIsUpdateNullField || pEntity.ExpirationTime!=null)
                strSql.Append( "[ExpirationTime]=@ExpirationTime,");
            if (pIsUpdateNullField || pEntity.QrcodeSeq!=null)
                strSql.Append( "[QrcodeSeq]=@QrcodeSeq,");
            if (pIsUpdateNullField || pEntity.QrcodeUrl!=null)
                strSql.Append( "[QrcodeUrl]=@QrcodeUrl,");
            if (pIsUpdateNullField || pEntity.ClientID!=null)
                strSql.Append( "[ClientID]=@ClientID,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@WeixinID",SqlDbType.NVarChar),
					new SqlParameter("@WeixinNumber",SqlDbType.NVarChar),
					new SqlParameter("@DevelopUrl",SqlDbType.NVarChar),
					new SqlParameter("@DevelopToken",SqlDbType.NVarChar),
					new SqlParameter("@AppID",SqlDbType.NVarChar),
					new SqlParameter("@AppSecret",SqlDbType.NVarChar),
					new SqlParameter("@LoginName",SqlDbType.NVarChar),
					new SqlParameter("@LoginPassword",SqlDbType.NVarChar),
					new SqlParameter("@OAuthUrl",SqlDbType.NVarChar),
					new SqlParameter("@AccessToken",SqlDbType.NVarChar),
					new SqlParameter("@ExpirationTime",SqlDbType.DateTime),
					new SqlParameter("@QrcodeSeq",SqlDbType.Int),
					new SqlParameter("@QrcodeUrl",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Name;
			parameters[1].Value = pEntity.Type;
			parameters[2].Value = pEntity.WeixinID;
			parameters[3].Value = pEntity.WeixinNumber;
			parameters[4].Value = pEntity.DevelopUrl;
			parameters[5].Value = pEntity.DevelopToken;
			parameters[6].Value = pEntity.AppID;
			parameters[7].Value = pEntity.AppSecret;
			parameters[8].Value = pEntity.LoginName;
			parameters[9].Value = pEntity.LoginPassword;
			parameters[10].Value = pEntity.OAuthUrl;
			parameters[11].Value = pEntity.AccessToken;
			parameters[12].Value = pEntity.ExpirationTime;
			parameters[13].Value = pEntity.QrcodeSeq;
			parameters[14].Value = pEntity.QrcodeUrl;
			parameters[15].Value = pEntity.ClientID;
			parameters[16].Value = pEntity.LastUpdateBy;
			parameters[17].Value = pEntity.LastUpdateTime;
			parameters[18].Value = pEntity.ID;

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
        public void Update(WApplicationEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(WApplicationEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(WApplicationEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [WApplication] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(WApplicationEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(WApplicationEntity[] pEntities)
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
            sql.AppendLine("update [WApplication] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public WApplicationEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [WApplication] where isdelete=0 ");
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
            List<WApplicationEntity> list = new List<WApplicationEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    WApplicationEntity m;
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
        public PagedQueryResult<WApplicationEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [WApplication] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [WApplication] where isdelete=0 ");
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
            PagedQueryResult<WApplicationEntity> result = new PagedQueryResult<WApplicationEntity>();
            List<WApplicationEntity> list = new List<WApplicationEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    WApplicationEntity m;
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
        public WApplicationEntity[] QueryByEntity(WApplicationEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<WApplicationEntity> PagedQueryByEntity(WApplicationEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(WApplicationEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.Name!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Name", Value = pQueryEntity.Name });
            if (pQueryEntity.Type!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Type", Value = pQueryEntity.Type });
            if (pQueryEntity.WeixinID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WeixinID", Value = pQueryEntity.WeixinID });
            if (pQueryEntity.WeixinNumber!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WeixinNumber", Value = pQueryEntity.WeixinNumber });
            if (pQueryEntity.DevelopUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DevelopUrl", Value = pQueryEntity.DevelopUrl });
            if (pQueryEntity.DevelopToken!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DevelopToken", Value = pQueryEntity.DevelopToken });
            if (pQueryEntity.AppID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AppID", Value = pQueryEntity.AppID });
            if (pQueryEntity.AppSecret!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AppSecret", Value = pQueryEntity.AppSecret });
            if (pQueryEntity.LoginName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LoginName", Value = pQueryEntity.LoginName });
            if (pQueryEntity.LoginPassword!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LoginPassword", Value = pQueryEntity.LoginPassword });
            if (pQueryEntity.OAuthUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "OAuthUrl", Value = pQueryEntity.OAuthUrl });
            if (pQueryEntity.AccessToken!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AccessToken", Value = pQueryEntity.AccessToken });
            if (pQueryEntity.ExpirationTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ExpirationTime", Value = pQueryEntity.ExpirationTime });
            if (pQueryEntity.QrcodeSeq!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "QrcodeSeq", Value = pQueryEntity.QrcodeSeq });
            if (pQueryEntity.QrcodeUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "QrcodeUrl", Value = pQueryEntity.QrcodeUrl });
            if (pQueryEntity.ClientID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientID", Value = pQueryEntity.ClientID });
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
        protected void Load(SqlDataReader pReader, out WApplicationEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new WApplicationEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ID"] != DBNull.Value)
			{
				pInstance.ID =   Convert.ToInt32(pReader["ID"]);
			}
			if (pReader["Name"] != DBNull.Value)
			{
				pInstance.Name =  Convert.ToString(pReader["Name"]);
			}
			if (pReader["Type"] != DBNull.Value)
			{
				pInstance.Type =  Convert.ToString(pReader["Type"]);
			}
			if (pReader["WeixinID"] != DBNull.Value)
			{
				pInstance.WeixinID =  Convert.ToString(pReader["WeixinID"]);
			}
			if (pReader["WeixinNumber"] != DBNull.Value)
			{
				pInstance.WeixinNumber =  Convert.ToString(pReader["WeixinNumber"]);
			}
			if (pReader["DevelopUrl"] != DBNull.Value)
			{
				pInstance.DevelopUrl =  Convert.ToString(pReader["DevelopUrl"]);
			}
			if (pReader["DevelopToken"] != DBNull.Value)
			{
				pInstance.DevelopToken =  Convert.ToString(pReader["DevelopToken"]);
			}
			if (pReader["AppID"] != DBNull.Value)
			{
				pInstance.AppID =  Convert.ToString(pReader["AppID"]);
			}
			if (pReader["AppSecret"] != DBNull.Value)
			{
				pInstance.AppSecret =  Convert.ToString(pReader["AppSecret"]);
			}
			if (pReader["LoginName"] != DBNull.Value)
			{
				pInstance.LoginName =  Convert.ToString(pReader["LoginName"]);
			}
			if (pReader["LoginPassword"] != DBNull.Value)
			{
				pInstance.LoginPassword =  Convert.ToString(pReader["LoginPassword"]);
			}
			if (pReader["OAuthUrl"] != DBNull.Value)
			{
				pInstance.OAuthUrl =  Convert.ToString(pReader["OAuthUrl"]);
			}
			if (pReader["AccessToken"] != DBNull.Value)
			{
				pInstance.AccessToken =  Convert.ToString(pReader["AccessToken"]);
			}
			if (pReader["ExpirationTime"] != DBNull.Value)
			{
				pInstance.ExpirationTime =  Convert.ToDateTime(pReader["ExpirationTime"]);
			}
			if (pReader["QrcodeSeq"] != DBNull.Value)
			{
				pInstance.QrcodeSeq =   Convert.ToInt32(pReader["QrcodeSeq"]);
			}
			if (pReader["QrcodeUrl"] != DBNull.Value)
			{
				pInstance.QrcodeUrl =  Convert.ToString(pReader["QrcodeUrl"]);
			}
			if (pReader["ClientID"] != DBNull.Value)
			{
				pInstance.ClientID =   Convert.ToInt32(pReader["ClientID"]);
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
