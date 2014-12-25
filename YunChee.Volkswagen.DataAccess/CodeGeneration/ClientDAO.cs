/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/23 10:17:01
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
    /// 数据访问： 0909企业客户信息 Client 
    /// 表Client的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ClientDAO : BaseDAO<BasicUserInfo>, ICRUDable<ClientEntity>, IQueryable<ClientEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public ClientDAO(BasicUserInfo pUserInfo)
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
        public void Create(ClientEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(ClientEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [Client](");
            strSql.Append("[Code],[Name],[CarBrandID],[RegionPartition],[Type],[ParentID],[ShortName],[ServiceCode],[DealerRegion],[Address],[Website],[Email],[SaleTel],[AfterSaleTel],[CustomerServiceTel],[ServiceHotline],[Longitude],[Latitude],[ProvinceID],[CityID],[DistrictID],[ZipCode],[Remark],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@Code,@Name,@CarBrandID,@RegionPartition,@Type,@ParentID,@ShortName,@ServiceCode,@DealerRegion,@Address,@Website,@Email,@SaleTel,@AfterSaleTel,@CustomerServiceTel,@ServiceHotline,@Longitude,@Latitude,@ProvinceID,@CityID,@DistrictID,@ZipCode,@Remark,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@Code",SqlDbType.NVarChar),
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@CarBrandID",SqlDbType.Int),
					new SqlParameter("@RegionPartition",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@ParentID",SqlDbType.Int),
					new SqlParameter("@ShortName",SqlDbType.NVarChar),
					new SqlParameter("@ServiceCode",SqlDbType.NVarChar),
					new SqlParameter("@DealerRegion",SqlDbType.NVarChar),
					new SqlParameter("@Address",SqlDbType.NVarChar),
					new SqlParameter("@Website",SqlDbType.NVarChar),
					new SqlParameter("@Email",SqlDbType.NVarChar),
					new SqlParameter("@SaleTel",SqlDbType.NVarChar),
					new SqlParameter("@AfterSaleTel",SqlDbType.NVarChar),
					new SqlParameter("@CustomerServiceTel",SqlDbType.NVarChar),
					new SqlParameter("@ServiceHotline",SqlDbType.NVarChar),
					new SqlParameter("@Longitude",SqlDbType.NVarChar),
					new SqlParameter("@Latitude",SqlDbType.NVarChar),
					new SqlParameter("@ProvinceID",SqlDbType.Int),
					new SqlParameter("@CityID",SqlDbType.Int),
					new SqlParameter("@DistrictID",SqlDbType.Int),
					new SqlParameter("@ZipCode",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Code;
			parameters[1].Value = pEntity.Name;
			parameters[2].Value = pEntity.CarBrandID;
			parameters[3].Value = pEntity.RegionPartition;
			parameters[4].Value = pEntity.Type;
			parameters[5].Value = pEntity.ParentID;
			parameters[6].Value = pEntity.ShortName;
			parameters[7].Value = pEntity.ServiceCode;
			parameters[8].Value = pEntity.DealerRegion;
			parameters[9].Value = pEntity.Address;
			parameters[10].Value = pEntity.Website;
			parameters[11].Value = pEntity.Email;
			parameters[12].Value = pEntity.SaleTel;
			parameters[13].Value = pEntity.AfterSaleTel;
			parameters[14].Value = pEntity.CustomerServiceTel;
			parameters[15].Value = pEntity.ServiceHotline;
			parameters[16].Value = pEntity.Longitude;
			parameters[17].Value = pEntity.Latitude;
			parameters[18].Value = pEntity.ProvinceID;
			parameters[19].Value = pEntity.CityID;
			parameters[20].Value = pEntity.DistrictID;
			parameters[21].Value = pEntity.ZipCode;
			parameters[22].Value = pEntity.Remark;
			parameters[23].Value = pEntity.CreateBy;
			parameters[24].Value = pEntity.CreateTime;
			parameters[25].Value = pEntity.LastUpdateBy;
			parameters[26].Value = pEntity.LastUpdateTime;
			parameters[27].Value = pEntity.IsDelete;

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
        public ClientEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Client] where ID='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            ClientEntity m = null;
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
        public ClientEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Client] where isdelete=0");
            //读取数据
            List<ClientEntity> list = new List<ClientEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    ClientEntity m;
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
        public void Update(ClientEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(ClientEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [Client] set ");
                        if (pIsUpdateNullField || pEntity.Code!=null)
                strSql.Append( "[Code]=@Code,");
            if (pIsUpdateNullField || pEntity.Name!=null)
                strSql.Append( "[Name]=@Name,");
            if (pIsUpdateNullField || pEntity.CarBrandID!=null)
                strSql.Append( "[CarBrandID]=@CarBrandID,");
            if (pIsUpdateNullField || pEntity.RegionPartition!=null)
                strSql.Append( "[RegionPartition]=@RegionPartition,");
            if (pIsUpdateNullField || pEntity.Type!=null)
                strSql.Append( "[Type]=@Type,");
            if (pIsUpdateNullField || pEntity.ParentID!=null)
                strSql.Append( "[ParentID]=@ParentID,");
            if (pIsUpdateNullField || pEntity.ShortName!=null)
                strSql.Append( "[ShortName]=@ShortName,");
            if (pIsUpdateNullField || pEntity.ServiceCode!=null)
                strSql.Append( "[ServiceCode]=@ServiceCode,");
            if (pIsUpdateNullField || pEntity.DealerRegion!=null)
                strSql.Append( "[DealerRegion]=@DealerRegion,");
            if (pIsUpdateNullField || pEntity.Address!=null)
                strSql.Append( "[Address]=@Address,");
            if (pIsUpdateNullField || pEntity.Website!=null)
                strSql.Append( "[Website]=@Website,");
            if (pIsUpdateNullField || pEntity.Email!=null)
                strSql.Append( "[Email]=@Email,");
            if (pIsUpdateNullField || pEntity.SaleTel!=null)
                strSql.Append( "[SaleTel]=@SaleTel,");
            if (pIsUpdateNullField || pEntity.AfterSaleTel!=null)
                strSql.Append( "[AfterSaleTel]=@AfterSaleTel,");
            if (pIsUpdateNullField || pEntity.CustomerServiceTel!=null)
                strSql.Append( "[CustomerServiceTel]=@CustomerServiceTel,");
            if (pIsUpdateNullField || pEntity.ServiceHotline!=null)
                strSql.Append( "[ServiceHotline]=@ServiceHotline,");
            if (pIsUpdateNullField || pEntity.Longitude!=null)
                strSql.Append( "[Longitude]=@Longitude,");
            if (pIsUpdateNullField || pEntity.Latitude!=null)
                strSql.Append( "[Latitude]=@Latitude,");
            if (pIsUpdateNullField || pEntity.ProvinceID!=null)
                strSql.Append( "[ProvinceID]=@ProvinceID,");
            if (pIsUpdateNullField || pEntity.CityID!=null)
                strSql.Append( "[CityID]=@CityID,");
            if (pIsUpdateNullField || pEntity.DistrictID!=null)
                strSql.Append( "[DistrictID]=@DistrictID,");
            if (pIsUpdateNullField || pEntity.ZipCode!=null)
                strSql.Append( "[ZipCode]=@ZipCode,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@Code",SqlDbType.NVarChar),
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@CarBrandID",SqlDbType.Int),
					new SqlParameter("@RegionPartition",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@ParentID",SqlDbType.Int),
					new SqlParameter("@ShortName",SqlDbType.NVarChar),
					new SqlParameter("@ServiceCode",SqlDbType.NVarChar),
					new SqlParameter("@DealerRegion",SqlDbType.NVarChar),
					new SqlParameter("@Address",SqlDbType.NVarChar),
					new SqlParameter("@Website",SqlDbType.NVarChar),
					new SqlParameter("@Email",SqlDbType.NVarChar),
					new SqlParameter("@SaleTel",SqlDbType.NVarChar),
					new SqlParameter("@AfterSaleTel",SqlDbType.NVarChar),
					new SqlParameter("@CustomerServiceTel",SqlDbType.NVarChar),
					new SqlParameter("@ServiceHotline",SqlDbType.NVarChar),
					new SqlParameter("@Longitude",SqlDbType.NVarChar),
					new SqlParameter("@Latitude",SqlDbType.NVarChar),
					new SqlParameter("@ProvinceID",SqlDbType.Int),
					new SqlParameter("@CityID",SqlDbType.Int),
					new SqlParameter("@DistrictID",SqlDbType.Int),
					new SqlParameter("@ZipCode",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Code;
			parameters[1].Value = pEntity.Name;
			parameters[2].Value = pEntity.CarBrandID;
			parameters[3].Value = pEntity.RegionPartition;
			parameters[4].Value = pEntity.Type;
			parameters[5].Value = pEntity.ParentID;
			parameters[6].Value = pEntity.ShortName;
			parameters[7].Value = pEntity.ServiceCode;
			parameters[8].Value = pEntity.DealerRegion;
			parameters[9].Value = pEntity.Address;
			parameters[10].Value = pEntity.Website;
			parameters[11].Value = pEntity.Email;
			parameters[12].Value = pEntity.SaleTel;
			parameters[13].Value = pEntity.AfterSaleTel;
			parameters[14].Value = pEntity.CustomerServiceTel;
			parameters[15].Value = pEntity.ServiceHotline;
			parameters[16].Value = pEntity.Longitude;
			parameters[17].Value = pEntity.Latitude;
			parameters[18].Value = pEntity.ProvinceID;
			parameters[19].Value = pEntity.CityID;
			parameters[20].Value = pEntity.DistrictID;
			parameters[21].Value = pEntity.ZipCode;
			parameters[22].Value = pEntity.Remark;
			parameters[23].Value = pEntity.LastUpdateBy;
			parameters[24].Value = pEntity.LastUpdateTime;
			parameters[25].Value = pEntity.ID;

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
        public void Update(ClientEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(ClientEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(ClientEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [Client] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(ClientEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(ClientEntity[] pEntities)
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
            sql.AppendLine("update [Client] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public ClientEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Client] where isdelete=0 ");
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
            List<ClientEntity> list = new List<ClientEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    ClientEntity m;
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
        public PagedQueryResult<ClientEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [Client] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [Client] where isdelete=0 ");
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
            PagedQueryResult<ClientEntity> result = new PagedQueryResult<ClientEntity>();
            List<ClientEntity> list = new List<ClientEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    ClientEntity m;
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
        public ClientEntity[] QueryByEntity(ClientEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<ClientEntity> PagedQueryByEntity(ClientEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(ClientEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.Code!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Code", Value = pQueryEntity.Code });
            if (pQueryEntity.Name!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Name", Value = pQueryEntity.Name });
            if (pQueryEntity.CarBrandID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CarBrandID", Value = pQueryEntity.CarBrandID });
            if (pQueryEntity.RegionPartition!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "RegionPartition", Value = pQueryEntity.RegionPartition });
            if (pQueryEntity.Type!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Type", Value = pQueryEntity.Type });
            if (pQueryEntity.ParentID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ParentID", Value = pQueryEntity.ParentID });
            if (pQueryEntity.ShortName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ShortName", Value = pQueryEntity.ShortName });
            if (pQueryEntity.ServiceCode!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ServiceCode", Value = pQueryEntity.ServiceCode });
            if (pQueryEntity.DealerRegion!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DealerRegion", Value = pQueryEntity.DealerRegion });
            if (pQueryEntity.Address!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Address", Value = pQueryEntity.Address });
            if (pQueryEntity.Website!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Website", Value = pQueryEntity.Website });
            if (pQueryEntity.Email!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Email", Value = pQueryEntity.Email });
            if (pQueryEntity.SaleTel!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SaleTel", Value = pQueryEntity.SaleTel });
            if (pQueryEntity.AfterSaleTel!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AfterSaleTel", Value = pQueryEntity.AfterSaleTel });
            if (pQueryEntity.CustomerServiceTel!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerServiceTel", Value = pQueryEntity.CustomerServiceTel });
            if (pQueryEntity.ServiceHotline!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ServiceHotline", Value = pQueryEntity.ServiceHotline });
            if (pQueryEntity.Longitude!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Longitude", Value = pQueryEntity.Longitude });
            if (pQueryEntity.Latitude!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Latitude", Value = pQueryEntity.Latitude });
            if (pQueryEntity.ProvinceID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ProvinceID", Value = pQueryEntity.ProvinceID });
            if (pQueryEntity.CityID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CityID", Value = pQueryEntity.CityID });
            if (pQueryEntity.DistrictID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DistrictID", Value = pQueryEntity.DistrictID });
            if (pQueryEntity.ZipCode!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ZipCode", Value = pQueryEntity.ZipCode });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
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
        protected void Load(SqlDataReader pReader, out ClientEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new ClientEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ID"] != DBNull.Value)
			{
				pInstance.ID =   Convert.ToInt32(pReader["ID"]);
			}
			if (pReader["Code"] != DBNull.Value)
			{
				pInstance.Code =  Convert.ToString(pReader["Code"]);
			}
			if (pReader["Name"] != DBNull.Value)
			{
				pInstance.Name =  Convert.ToString(pReader["Name"]);
			}
			if (pReader["CarBrandID"] != DBNull.Value)
			{
				pInstance.CarBrandID =   Convert.ToInt32(pReader["CarBrandID"]);
			}
			if (pReader["RegionPartition"] != DBNull.Value)
			{
				pInstance.RegionPartition =  Convert.ToString(pReader["RegionPartition"]);
			}
			if (pReader["Type"] != DBNull.Value)
			{
				pInstance.Type =  Convert.ToString(pReader["Type"]);
			}
			if (pReader["ParentID"] != DBNull.Value)
			{
				pInstance.ParentID =   Convert.ToInt32(pReader["ParentID"]);
			}
			if (pReader["ShortName"] != DBNull.Value)
			{
				pInstance.ShortName =  Convert.ToString(pReader["ShortName"]);
			}
			if (pReader["ServiceCode"] != DBNull.Value)
			{
				pInstance.ServiceCode =  Convert.ToString(pReader["ServiceCode"]);
			}
			if (pReader["DealerRegion"] != DBNull.Value)
			{
				pInstance.DealerRegion =  Convert.ToString(pReader["DealerRegion"]);
			}
			if (pReader["Address"] != DBNull.Value)
			{
				pInstance.Address =  Convert.ToString(pReader["Address"]);
			}
			if (pReader["Website"] != DBNull.Value)
			{
				pInstance.Website =  Convert.ToString(pReader["Website"]);
			}
			if (pReader["Email"] != DBNull.Value)
			{
				pInstance.Email =  Convert.ToString(pReader["Email"]);
			}
			if (pReader["SaleTel"] != DBNull.Value)
			{
				pInstance.SaleTel =  Convert.ToString(pReader["SaleTel"]);
			}
			if (pReader["AfterSaleTel"] != DBNull.Value)
			{
				pInstance.AfterSaleTel =  Convert.ToString(pReader["AfterSaleTel"]);
			}
			if (pReader["CustomerServiceTel"] != DBNull.Value)
			{
				pInstance.CustomerServiceTel =  Convert.ToString(pReader["CustomerServiceTel"]);
			}
			if (pReader["ServiceHotline"] != DBNull.Value)
			{
				pInstance.ServiceHotline =  Convert.ToString(pReader["ServiceHotline"]);
			}
			if (pReader["Longitude"] != DBNull.Value)
			{
				pInstance.Longitude =  Convert.ToString(pReader["Longitude"]);
			}
			if (pReader["Latitude"] != DBNull.Value)
			{
				pInstance.Latitude =  Convert.ToString(pReader["Latitude"]);
			}
			if (pReader["ProvinceID"] != DBNull.Value)
			{
				pInstance.ProvinceID =   Convert.ToInt32(pReader["ProvinceID"]);
			}
			if (pReader["CityID"] != DBNull.Value)
			{
				pInstance.CityID =   Convert.ToInt32(pReader["CityID"]);
			}
			if (pReader["DistrictID"] != DBNull.Value)
			{
				pInstance.DistrictID =   Convert.ToInt32(pReader["DistrictID"]);
			}
			if (pReader["ZipCode"] != DBNull.Value)
			{
				pInstance.ZipCode =  Convert.ToString(pReader["ZipCode"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
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
