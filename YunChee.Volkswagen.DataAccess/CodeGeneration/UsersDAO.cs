/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/10 17:57:50
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
    /// 数据访问： 0901用户表 Users 
    /// 表Users的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class UsersDAO : BaseDAO<BasicUserInfo>, ICRUDable<UsersEntity>, IQueryable<UsersEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public UsersDAO(BasicUserInfo pUserInfo)
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
        public void Create(UsersEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(UsersEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [Users](");
            strSql.Append("[Name],[Password],[Email],[Phone],[Enabled],[Gender],[ChineseName],[EnglishName],[Photo],[QQ],[Address],[Remark],[Birthday],[Department],[JobTitle],[BeOnDuty],[ClientID],[WxId],[WxOpenId],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@Name,@Password,@Email,@Phone,@Enabled,@Gender,@ChineseName,@EnglishName,@Photo,@QQ,@Address,@Remark,@Birthday,@Department,@JobTitle,@BeOnDuty,@ClientID,@WxId,@WxOpenId,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@Password",SqlDbType.NVarChar),
					new SqlParameter("@Email",SqlDbType.NVarChar),
					new SqlParameter("@Phone",SqlDbType.NVarChar),
					new SqlParameter("@Enabled",SqlDbType.Int),
					new SqlParameter("@Gender",SqlDbType.NVarChar),
					new SqlParameter("@ChineseName",SqlDbType.NVarChar),
					new SqlParameter("@EnglishName",SqlDbType.NVarChar),
					new SqlParameter("@Photo",SqlDbType.NVarChar),
					new SqlParameter("@QQ",SqlDbType.NVarChar),
					new SqlParameter("@Address",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@Birthday",SqlDbType.DateTime),
					new SqlParameter("@Department",SqlDbType.NVarChar),
					new SqlParameter("@JobTitle",SqlDbType.NVarChar),
					new SqlParameter("@BeOnDuty",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Name;
			parameters[1].Value = pEntity.Password;
			parameters[2].Value = pEntity.Email;
			parameters[3].Value = pEntity.Phone;
			parameters[4].Value = pEntity.Enabled;
			parameters[5].Value = pEntity.Gender;
			parameters[6].Value = pEntity.ChineseName;
			parameters[7].Value = pEntity.EnglishName;
			parameters[8].Value = pEntity.Photo;
			parameters[9].Value = pEntity.QQ;
			parameters[10].Value = pEntity.Address;
			parameters[11].Value = pEntity.Remark;
			parameters[12].Value = pEntity.Birthday;
			parameters[13].Value = pEntity.Department;
			parameters[14].Value = pEntity.JobTitle;
			parameters[15].Value = pEntity.BeOnDuty;
			parameters[16].Value = pEntity.ClientID;
			parameters[17].Value = pEntity.WxId;
			parameters[18].Value = pEntity.WxOpenId;
			parameters[19].Value = pEntity.CreateBy;
			parameters[20].Value = pEntity.CreateTime;
			parameters[21].Value = pEntity.LastUpdateBy;
			parameters[22].Value = pEntity.LastUpdateTime;
			parameters[23].Value = pEntity.IsDelete;

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
        public UsersEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Users] where ID='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            UsersEntity m = null;
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
        public UsersEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Users] where isdelete=0");
            //读取数据
            List<UsersEntity> list = new List<UsersEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    UsersEntity m;
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
        public void Update(UsersEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(UsersEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [Users] set ");
                        if (pIsUpdateNullField || pEntity.Name!=null)
                strSql.Append( "[Name]=@Name,");
            if (pIsUpdateNullField || pEntity.Password!=null)
                strSql.Append( "[Password]=@Password,");
            if (pIsUpdateNullField || pEntity.Email!=null)
                strSql.Append( "[Email]=@Email,");
            if (pIsUpdateNullField || pEntity.Phone!=null)
                strSql.Append( "[Phone]=@Phone,");
            if (pIsUpdateNullField || pEntity.Enabled!=null)
                strSql.Append( "[Enabled]=@Enabled,");
            if (pIsUpdateNullField || pEntity.Gender!=null)
                strSql.Append( "[Gender]=@Gender,");
            if (pIsUpdateNullField || pEntity.ChineseName!=null)
                strSql.Append( "[ChineseName]=@ChineseName,");
            if (pIsUpdateNullField || pEntity.EnglishName!=null)
                strSql.Append( "[EnglishName]=@EnglishName,");
            if (pIsUpdateNullField || pEntity.Photo!=null)
                strSql.Append( "[Photo]=@Photo,");
            if (pIsUpdateNullField || pEntity.QQ!=null)
                strSql.Append( "[QQ]=@QQ,");
            if (pIsUpdateNullField || pEntity.Address!=null)
                strSql.Append( "[Address]=@Address,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.Birthday!=null)
                strSql.Append( "[Birthday]=@Birthday,");
            if (pIsUpdateNullField || pEntity.Department!=null)
                strSql.Append( "[Department]=@Department,");
            if (pIsUpdateNullField || pEntity.JobTitle!=null)
                strSql.Append( "[JobTitle]=@JobTitle,");
            if (pIsUpdateNullField || pEntity.BeOnDuty!=null)
                strSql.Append( "[BeOnDuty]=@BeOnDuty,");
            if (pIsUpdateNullField || pEntity.ClientID!=null)
                strSql.Append( "[ClientID]=@ClientID,");
            if (pIsUpdateNullField || pEntity.WxId!=null)
                strSql.Append( "[WxId]=@WxId,");
            if (pIsUpdateNullField || pEntity.WxOpenId!=null)
                strSql.Append( "[WxOpenId]=@WxOpenId,");
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
					new SqlParameter("@Password",SqlDbType.NVarChar),
					new SqlParameter("@Email",SqlDbType.NVarChar),
					new SqlParameter("@Phone",SqlDbType.NVarChar),
					new SqlParameter("@Enabled",SqlDbType.Int),
					new SqlParameter("@Gender",SqlDbType.NVarChar),
					new SqlParameter("@ChineseName",SqlDbType.NVarChar),
					new SqlParameter("@EnglishName",SqlDbType.NVarChar),
					new SqlParameter("@Photo",SqlDbType.NVarChar),
					new SqlParameter("@QQ",SqlDbType.NVarChar),
					new SqlParameter("@Address",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@Birthday",SqlDbType.DateTime),
					new SqlParameter("@Department",SqlDbType.NVarChar),
					new SqlParameter("@JobTitle",SqlDbType.NVarChar),
					new SqlParameter("@BeOnDuty",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Name;
			parameters[1].Value = pEntity.Password;
			parameters[2].Value = pEntity.Email;
			parameters[3].Value = pEntity.Phone;
			parameters[4].Value = pEntity.Enabled;
			parameters[5].Value = pEntity.Gender;
			parameters[6].Value = pEntity.ChineseName;
			parameters[7].Value = pEntity.EnglishName;
			parameters[8].Value = pEntity.Photo;
			parameters[9].Value = pEntity.QQ;
			parameters[10].Value = pEntity.Address;
			parameters[11].Value = pEntity.Remark;
			parameters[12].Value = pEntity.Birthday;
			parameters[13].Value = pEntity.Department;
			parameters[14].Value = pEntity.JobTitle;
			parameters[15].Value = pEntity.BeOnDuty;
			parameters[16].Value = pEntity.ClientID;
			parameters[17].Value = pEntity.WxId;
			parameters[18].Value = pEntity.WxOpenId;
			parameters[19].Value = pEntity.LastUpdateBy;
			parameters[20].Value = pEntity.LastUpdateTime;
			parameters[21].Value = pEntity.ID;

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
        public void Update(UsersEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(UsersEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(UsersEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [Users] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(UsersEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(UsersEntity[] pEntities)
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
            sql.AppendLine("update [Users] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public UsersEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Users] where isdelete=0 ");
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
            List<UsersEntity> list = new List<UsersEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    UsersEntity m;
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
        public PagedQueryResult<UsersEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [Users] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [Users] where isdelete=0 ");
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
            PagedQueryResult<UsersEntity> result = new PagedQueryResult<UsersEntity>();
            List<UsersEntity> list = new List<UsersEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    UsersEntity m;
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
        public UsersEntity[] QueryByEntity(UsersEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<UsersEntity> PagedQueryByEntity(UsersEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(UsersEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.Name!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Name", Value = pQueryEntity.Name });
            if (pQueryEntity.Password!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Password", Value = pQueryEntity.Password });
            if (pQueryEntity.Email!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Email", Value = pQueryEntity.Email });
            if (pQueryEntity.Phone!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Phone", Value = pQueryEntity.Phone });
            if (pQueryEntity.Enabled!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Enabled", Value = pQueryEntity.Enabled });
            if (pQueryEntity.Gender!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Gender", Value = pQueryEntity.Gender });
            if (pQueryEntity.ChineseName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ChineseName", Value = pQueryEntity.ChineseName });
            if (pQueryEntity.EnglishName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EnglishName", Value = pQueryEntity.EnglishName });
            if (pQueryEntity.Photo!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Photo", Value = pQueryEntity.Photo });
            if (pQueryEntity.QQ!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "QQ", Value = pQueryEntity.QQ });
            if (pQueryEntity.Address!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Address", Value = pQueryEntity.Address });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
            if (pQueryEntity.Birthday!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Birthday", Value = pQueryEntity.Birthday });
            if (pQueryEntity.Department!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Department", Value = pQueryEntity.Department });
            if (pQueryEntity.JobTitle!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "JobTitle", Value = pQueryEntity.JobTitle });
            if (pQueryEntity.BeOnDuty!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BeOnDuty", Value = pQueryEntity.BeOnDuty });
            if (pQueryEntity.ClientID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientID", Value = pQueryEntity.ClientID });
            if (pQueryEntity.WxId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxId", Value = pQueryEntity.WxId });
            if (pQueryEntity.WxOpenId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxOpenId", Value = pQueryEntity.WxOpenId });
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
        protected void Load(SqlDataReader pReader, out UsersEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new UsersEntity();
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
			if (pReader["Password"] != DBNull.Value)
			{
				pInstance.Password =  Convert.ToString(pReader["Password"]);
			}
			if (pReader["Email"] != DBNull.Value)
			{
				pInstance.Email =  Convert.ToString(pReader["Email"]);
			}
			if (pReader["Phone"] != DBNull.Value)
			{
				pInstance.Phone =  Convert.ToString(pReader["Phone"]);
			}
			if (pReader["Enabled"] != DBNull.Value)
			{
				pInstance.Enabled =   Convert.ToInt32(pReader["Enabled"]);
			}
			if (pReader["Gender"] != DBNull.Value)
			{
				pInstance.Gender =  Convert.ToString(pReader["Gender"]);
			}
			if (pReader["ChineseName"] != DBNull.Value)
			{
				pInstance.ChineseName =  Convert.ToString(pReader["ChineseName"]);
			}
			if (pReader["EnglishName"] != DBNull.Value)
			{
				pInstance.EnglishName =  Convert.ToString(pReader["EnglishName"]);
			}
			if (pReader["Photo"] != DBNull.Value)
			{
				pInstance.Photo =  Convert.ToString(pReader["Photo"]);
			}
			if (pReader["QQ"] != DBNull.Value)
			{
				pInstance.QQ =  Convert.ToString(pReader["QQ"]);
			}
			if (pReader["Address"] != DBNull.Value)
			{
				pInstance.Address =  Convert.ToString(pReader["Address"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
			}
			if (pReader["Birthday"] != DBNull.Value)
			{
				pInstance.Birthday =  Convert.ToDateTime(pReader["Birthday"]);
			}
			if (pReader["Department"] != DBNull.Value)
			{
				pInstance.Department =  Convert.ToString(pReader["Department"]);
			}
			if (pReader["JobTitle"] != DBNull.Value)
			{
				pInstance.JobTitle =  Convert.ToString(pReader["JobTitle"]);
			}
			if (pReader["BeOnDuty"] != DBNull.Value)
			{
				pInstance.BeOnDuty =  Convert.ToString(pReader["BeOnDuty"]);
			}
			if (pReader["ClientID"] != DBNull.Value)
			{
				pInstance.ClientID =   Convert.ToInt32(pReader["ClientID"]);
			}
			if (pReader["WxId"] != DBNull.Value)
			{
				pInstance.WxId =  Convert.ToString(pReader["WxId"]);
			}
			if (pReader["WxOpenId"] != DBNull.Value)
			{
				pInstance.WxOpenId =  Convert.ToString(pReader["WxOpenId"]);
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
