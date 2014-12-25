/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/23 18:09:09
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
    /// 数据访问： 0316车款座椅表 CarStyleSeat 
    /// 表CarStyleSeat的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CarStyleSeatDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarStyleSeatEntity>, IQueryable<CarStyleSeatEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public CarStyleSeatDAO(BasicUserInfo pUserInfo)
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
        public void Create(CarStyleSeatEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(CarStyleSeatEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [CarStyleSeat](");
            strSql.Append("[CarStyleID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete],[Column1],[Column2],[Column3],[Column4],[Column5],[Column6],[Column7],[Column8],[Column9],[Column10],[Column11],[Column12],[Column13],[Column14],[Column15],[Column16],[Column17],[Column18],[Column19],[Column20])");
            strSql.Append(" values (");
            strSql.Append("@CarStyleID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete,@Column1,@Column2,@Column3,@Column4,@Column5,@Column6,@Column7,@Column8,@Column9,@Column10,@Column11,@Column12,@Column13,@Column14,@Column15,@Column16,@Column17,@Column18,@Column19,@Column20)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@CarStyleID",SqlDbType.Int),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@Column1",SqlDbType.NVarChar),
					new SqlParameter("@Column2",SqlDbType.NVarChar),
					new SqlParameter("@Column3",SqlDbType.NVarChar),
					new SqlParameter("@Column4",SqlDbType.NVarChar),
					new SqlParameter("@Column5",SqlDbType.NVarChar),
					new SqlParameter("@Column6",SqlDbType.NVarChar),
					new SqlParameter("@Column7",SqlDbType.NVarChar),
					new SqlParameter("@Column8",SqlDbType.NVarChar),
					new SqlParameter("@Column9",SqlDbType.NVarChar),
					new SqlParameter("@Column10",SqlDbType.NVarChar),
					new SqlParameter("@Column11",SqlDbType.NVarChar),
					new SqlParameter("@Column12",SqlDbType.NVarChar),
					new SqlParameter("@Column13",SqlDbType.NVarChar),
					new SqlParameter("@Column14",SqlDbType.NVarChar),
					new SqlParameter("@Column15",SqlDbType.NVarChar),
					new SqlParameter("@Column16",SqlDbType.NVarChar),
					new SqlParameter("@Column17",SqlDbType.NVarChar),
					new SqlParameter("@Column18",SqlDbType.NVarChar),
					new SqlParameter("@Column19",SqlDbType.NVarChar),
					new SqlParameter("@Column20",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.CarStyleID;
			parameters[1].Value = pEntity.CreateBy;
			parameters[2].Value = pEntity.CreateTime;
			parameters[3].Value = pEntity.LastUpdateBy;
			parameters[4].Value = pEntity.LastUpdateTime;
			parameters[5].Value = pEntity.IsDelete;
			parameters[6].Value = pEntity.Column1;
			parameters[7].Value = pEntity.Column2;
			parameters[8].Value = pEntity.Column3;
			parameters[9].Value = pEntity.Column4;
			parameters[10].Value = pEntity.Column5;
			parameters[11].Value = pEntity.Column6;
			parameters[12].Value = pEntity.Column7;
			parameters[13].Value = pEntity.Column8;
			parameters[14].Value = pEntity.Column9;
			parameters[15].Value = pEntity.Column10;
			parameters[16].Value = pEntity.Column11;
			parameters[17].Value = pEntity.Column12;
			parameters[18].Value = pEntity.Column13;
			parameters[19].Value = pEntity.Column14;
			parameters[20].Value = pEntity.Column15;
			parameters[21].Value = pEntity.Column16;
			parameters[22].Value = pEntity.Column17;
			parameters[23].Value = pEntity.Column18;
			parameters[24].Value = pEntity.Column19;
			parameters[25].Value = pEntity.Column20;

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
        public CarStyleSeatEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleSeat] where ID='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            CarStyleSeatEntity m = null;
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
        public CarStyleSeatEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleSeat] where isdelete=0");
            //读取数据
            List<CarStyleSeatEntity> list = new List<CarStyleSeatEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleSeatEntity m;
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
        public void Update(CarStyleSeatEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(CarStyleSeatEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [CarStyleSeat] set ");
                        if (pIsUpdateNullField || pEntity.CarStyleID!=null)
                strSql.Append( "[CarStyleID]=@CarStyleID,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.Column1!=null)
                strSql.Append( "[Column1]=@Column1,");
            if (pIsUpdateNullField || pEntity.Column2!=null)
                strSql.Append( "[Column2]=@Column2,");
            if (pIsUpdateNullField || pEntity.Column3!=null)
                strSql.Append( "[Column3]=@Column3,");
            if (pIsUpdateNullField || pEntity.Column4!=null)
                strSql.Append( "[Column4]=@Column4,");
            if (pIsUpdateNullField || pEntity.Column5!=null)
                strSql.Append( "[Column5]=@Column5,");
            if (pIsUpdateNullField || pEntity.Column6!=null)
                strSql.Append( "[Column6]=@Column6,");
            if (pIsUpdateNullField || pEntity.Column7!=null)
                strSql.Append( "[Column7]=@Column7,");
            if (pIsUpdateNullField || pEntity.Column8!=null)
                strSql.Append( "[Column8]=@Column8,");
            if (pIsUpdateNullField || pEntity.Column9!=null)
                strSql.Append( "[Column9]=@Column9,");
            if (pIsUpdateNullField || pEntity.Column10!=null)
                strSql.Append( "[Column10]=@Column10,");
            if (pIsUpdateNullField || pEntity.Column11!=null)
                strSql.Append( "[Column11]=@Column11,");
            if (pIsUpdateNullField || pEntity.Column12!=null)
                strSql.Append( "[Column12]=@Column12,");
            if (pIsUpdateNullField || pEntity.Column13!=null)
                strSql.Append( "[Column13]=@Column13,");
            if (pIsUpdateNullField || pEntity.Column14!=null)
                strSql.Append( "[Column14]=@Column14,");
            if (pIsUpdateNullField || pEntity.Column15!=null)
                strSql.Append( "[Column15]=@Column15,");
            if (pIsUpdateNullField || pEntity.Column16!=null)
                strSql.Append( "[Column16]=@Column16,");
            if (pIsUpdateNullField || pEntity.Column17!=null)
                strSql.Append( "[Column17]=@Column17,");
            if (pIsUpdateNullField || pEntity.Column18!=null)
                strSql.Append( "[Column18]=@Column18,");
            if (pIsUpdateNullField || pEntity.Column19!=null)
                strSql.Append( "[Column19]=@Column19,");
            if (pIsUpdateNullField || pEntity.Column20!=null)
                strSql.Append( "[Column20]=@Column20");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@CarStyleID",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@Column1",SqlDbType.NVarChar),
					new SqlParameter("@Column2",SqlDbType.NVarChar),
					new SqlParameter("@Column3",SqlDbType.NVarChar),
					new SqlParameter("@Column4",SqlDbType.NVarChar),
					new SqlParameter("@Column5",SqlDbType.NVarChar),
					new SqlParameter("@Column6",SqlDbType.NVarChar),
					new SqlParameter("@Column7",SqlDbType.NVarChar),
					new SqlParameter("@Column8",SqlDbType.NVarChar),
					new SqlParameter("@Column9",SqlDbType.NVarChar),
					new SqlParameter("@Column10",SqlDbType.NVarChar),
					new SqlParameter("@Column11",SqlDbType.NVarChar),
					new SqlParameter("@Column12",SqlDbType.NVarChar),
					new SqlParameter("@Column13",SqlDbType.NVarChar),
					new SqlParameter("@Column14",SqlDbType.NVarChar),
					new SqlParameter("@Column15",SqlDbType.NVarChar),
					new SqlParameter("@Column16",SqlDbType.NVarChar),
					new SqlParameter("@Column17",SqlDbType.NVarChar),
					new SqlParameter("@Column18",SqlDbType.NVarChar),
					new SqlParameter("@Column19",SqlDbType.NVarChar),
					new SqlParameter("@Column20",SqlDbType.NVarChar),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.CarStyleID;
			parameters[1].Value = pEntity.LastUpdateBy;
			parameters[2].Value = pEntity.LastUpdateTime;
			parameters[3].Value = pEntity.Column1;
			parameters[4].Value = pEntity.Column2;
			parameters[5].Value = pEntity.Column3;
			parameters[6].Value = pEntity.Column4;
			parameters[7].Value = pEntity.Column5;
			parameters[8].Value = pEntity.Column6;
			parameters[9].Value = pEntity.Column7;
			parameters[10].Value = pEntity.Column8;
			parameters[11].Value = pEntity.Column9;
			parameters[12].Value = pEntity.Column10;
			parameters[13].Value = pEntity.Column11;
			parameters[14].Value = pEntity.Column12;
			parameters[15].Value = pEntity.Column13;
			parameters[16].Value = pEntity.Column14;
			parameters[17].Value = pEntity.Column15;
			parameters[18].Value = pEntity.Column16;
			parameters[19].Value = pEntity.Column17;
			parameters[20].Value = pEntity.Column18;
			parameters[21].Value = pEntity.Column19;
			parameters[22].Value = pEntity.Column20;
			parameters[23].Value = pEntity.ID;

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
        public void Update(CarStyleSeatEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(CarStyleSeatEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(CarStyleSeatEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [CarStyleSeat] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(CarStyleSeatEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(CarStyleSeatEntity[] pEntities)
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
            sql.AppendLine("update [CarStyleSeat] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public CarStyleSeatEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleSeat] where isdelete=0 ");
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
            List<CarStyleSeatEntity> list = new List<CarStyleSeatEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleSeatEntity m;
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
        public PagedQueryResult<CarStyleSeatEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [CarStyleSeat] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [CarStyleSeat] where isdelete=0 ");
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
            PagedQueryResult<CarStyleSeatEntity> result = new PagedQueryResult<CarStyleSeatEntity>();
            List<CarStyleSeatEntity> list = new List<CarStyleSeatEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleSeatEntity m;
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
        public CarStyleSeatEntity[] QueryByEntity(CarStyleSeatEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<CarStyleSeatEntity> PagedQueryByEntity(CarStyleSeatEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(CarStyleSeatEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.CarStyleID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CarStyleID", Value = pQueryEntity.CarStyleID });
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
            if (pQueryEntity.Column1!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column1", Value = pQueryEntity.Column1 });
            if (pQueryEntity.Column2!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column2", Value = pQueryEntity.Column2 });
            if (pQueryEntity.Column3!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column3", Value = pQueryEntity.Column3 });
            if (pQueryEntity.Column4!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column4", Value = pQueryEntity.Column4 });
            if (pQueryEntity.Column5!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column5", Value = pQueryEntity.Column5 });
            if (pQueryEntity.Column6!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column6", Value = pQueryEntity.Column6 });
            if (pQueryEntity.Column7!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column7", Value = pQueryEntity.Column7 });
            if (pQueryEntity.Column8!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column8", Value = pQueryEntity.Column8 });
            if (pQueryEntity.Column9!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column9", Value = pQueryEntity.Column9 });
            if (pQueryEntity.Column10!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column10", Value = pQueryEntity.Column10 });
            if (pQueryEntity.Column11!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column11", Value = pQueryEntity.Column11 });
            if (pQueryEntity.Column12!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column12", Value = pQueryEntity.Column12 });
            if (pQueryEntity.Column13!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column13", Value = pQueryEntity.Column13 });
            if (pQueryEntity.Column14!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column14", Value = pQueryEntity.Column14 });
            if (pQueryEntity.Column15!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column15", Value = pQueryEntity.Column15 });
            if (pQueryEntity.Column16!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column16", Value = pQueryEntity.Column16 });
            if (pQueryEntity.Column17!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column17", Value = pQueryEntity.Column17 });
            if (pQueryEntity.Column18!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column18", Value = pQueryEntity.Column18 });
            if (pQueryEntity.Column19!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column19", Value = pQueryEntity.Column19 });
            if (pQueryEntity.Column20!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column20", Value = pQueryEntity.Column20 });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(SqlDataReader pReader, out CarStyleSeatEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new CarStyleSeatEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ID"] != DBNull.Value)
			{
				pInstance.ID =   Convert.ToInt32(pReader["ID"]);
			}
			if (pReader["CarStyleID"] != DBNull.Value)
			{
				pInstance.CarStyleID =   Convert.ToInt32(pReader["CarStyleID"]);
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
			if (pReader["Column1"] != DBNull.Value)
			{
				pInstance.Column1 =  Convert.ToString(pReader["Column1"]);
			}
			if (pReader["Column2"] != DBNull.Value)
			{
				pInstance.Column2 =  Convert.ToString(pReader["Column2"]);
			}
			if (pReader["Column3"] != DBNull.Value)
			{
				pInstance.Column3 =  Convert.ToString(pReader["Column3"]);
			}
			if (pReader["Column4"] != DBNull.Value)
			{
				pInstance.Column4 =  Convert.ToString(pReader["Column4"]);
			}
			if (pReader["Column5"] != DBNull.Value)
			{
				pInstance.Column5 =  Convert.ToString(pReader["Column5"]);
			}
			if (pReader["Column6"] != DBNull.Value)
			{
				pInstance.Column6 =  Convert.ToString(pReader["Column6"]);
			}
			if (pReader["Column7"] != DBNull.Value)
			{
				pInstance.Column7 =  Convert.ToString(pReader["Column7"]);
			}
			if (pReader["Column8"] != DBNull.Value)
			{
				pInstance.Column8 =  Convert.ToString(pReader["Column8"]);
			}
			if (pReader["Column9"] != DBNull.Value)
			{
				pInstance.Column9 =  Convert.ToString(pReader["Column9"]);
			}
			if (pReader["Column10"] != DBNull.Value)
			{
				pInstance.Column10 =  Convert.ToString(pReader["Column10"]);
			}
			if (pReader["Column11"] != DBNull.Value)
			{
				pInstance.Column11 =  Convert.ToString(pReader["Column11"]);
			}
			if (pReader["Column12"] != DBNull.Value)
			{
				pInstance.Column12 =  Convert.ToString(pReader["Column12"]);
			}
			if (pReader["Column13"] != DBNull.Value)
			{
				pInstance.Column13 =  Convert.ToString(pReader["Column13"]);
			}
			if (pReader["Column14"] != DBNull.Value)
			{
				pInstance.Column14 =  Convert.ToString(pReader["Column14"]);
			}
			if (pReader["Column15"] != DBNull.Value)
			{
				pInstance.Column15 =  Convert.ToString(pReader["Column15"]);
			}
			if (pReader["Column16"] != DBNull.Value)
			{
				pInstance.Column16 =  Convert.ToString(pReader["Column16"]);
			}
			if (pReader["Column17"] != DBNull.Value)
			{
				pInstance.Column17 =  Convert.ToString(pReader["Column17"]);
			}
			if (pReader["Column18"] != DBNull.Value)
			{
				pInstance.Column18 =  Convert.ToString(pReader["Column18"]);
			}
			if (pReader["Column19"] != DBNull.Value)
			{
				pInstance.Column19 =  Convert.ToString(pReader["Column19"]);
			}
			if (pReader["Column20"] != DBNull.Value)
			{
				pInstance.Column20 =  Convert.ToString(pReader["Column20"]);
			}

        }
        #endregion
    }
}
