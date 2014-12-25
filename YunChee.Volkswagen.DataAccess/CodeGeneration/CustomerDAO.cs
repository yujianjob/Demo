/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/27 16:54:25
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
    /// 数据访问： 0101客户信息表 Customer 
    /// 表Customer的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CustomerDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerEntity>, IQueryable<CustomerEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public CustomerDAO(BasicUserInfo pUserInfo)
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
        public void Create(CustomerEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(CustomerEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [Customer](");
            strSql.Append("[Name],[Code],[Type],[Phone],[LicensePlateNumber],[CarFrameNumber],[EngineNumber],[WxId],[WxOpenId],[WxNickName],[WxSex],[WxCity],[WxCountry],[WxProvince],[WxLanguage],[WxHeadImgUrl],[WxSubscribeTime],[SubscribeStatus],[IdCard],[Email],[Birthday],[IntentionCarTypeID],[IntentionCarStyleID],[BuyCarTypeID],[BuyCarStyleID],[StartFansDate],[EndFansDate],[StartPotentialDate],[EndPotentialDate],[StartCustomerDate],[UnSubscribeDate],[Remark],[ClientID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@Name,@Code,@Type,@Phone,@LicensePlateNumber,@CarFrameNumber,@EngineNumber,@WxId,@WxOpenId,@WxNickName,@WxSex,@WxCity,@WxCountry,@WxProvince,@WxLanguage,@WxHeadImgUrl,@WxSubscribeTime,@SubscribeStatus,@IdCard,@Email,@Birthday,@IntentionCarTypeID,@IntentionCarStyleID,@BuyCarTypeID,@BuyCarStyleID,@StartFansDate,@EndFansDate,@StartPotentialDate,@EndPotentialDate,@StartCustomerDate,@UnSubscribeDate,@Remark,@ClientID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@Code",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@Phone",SqlDbType.NVarChar),
					new SqlParameter("@LicensePlateNumber",SqlDbType.NVarChar),
					new SqlParameter("@CarFrameNumber",SqlDbType.NVarChar),
					new SqlParameter("@EngineNumber",SqlDbType.NVarChar),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@WxNickName",SqlDbType.NVarChar),
					new SqlParameter("@WxSex",SqlDbType.NVarChar),
					new SqlParameter("@WxCity",SqlDbType.NVarChar),
					new SqlParameter("@WxCountry",SqlDbType.NVarChar),
					new SqlParameter("@WxProvince",SqlDbType.NVarChar),
					new SqlParameter("@WxLanguage",SqlDbType.NVarChar),
					new SqlParameter("@WxHeadImgUrl",SqlDbType.NVarChar),
					new SqlParameter("@WxSubscribeTime",SqlDbType.NVarChar),
					new SqlParameter("@SubscribeStatus",SqlDbType.NVarChar),
					new SqlParameter("@IdCard",SqlDbType.NVarChar),
					new SqlParameter("@Email",SqlDbType.NVarChar),
					new SqlParameter("@Birthday",SqlDbType.DateTime),
					new SqlParameter("@IntentionCarTypeID",SqlDbType.Int),
					new SqlParameter("@IntentionCarStyleID",SqlDbType.Int),
					new SqlParameter("@BuyCarTypeID",SqlDbType.Int),
					new SqlParameter("@BuyCarStyleID",SqlDbType.Int),
					new SqlParameter("@StartFansDate",SqlDbType.DateTime),
					new SqlParameter("@EndFansDate",SqlDbType.DateTime),
					new SqlParameter("@StartPotentialDate",SqlDbType.DateTime),
					new SqlParameter("@EndPotentialDate",SqlDbType.DateTime),
					new SqlParameter("@StartCustomerDate",SqlDbType.DateTime),
					new SqlParameter("@UnSubscribeDate",SqlDbType.DateTime),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Name;
			parameters[1].Value = pEntity.Code;
			parameters[2].Value = pEntity.Type;
			parameters[3].Value = pEntity.Phone;
			parameters[4].Value = pEntity.LicensePlateNumber;
			parameters[5].Value = pEntity.CarFrameNumber;
			parameters[6].Value = pEntity.EngineNumber;
			parameters[7].Value = pEntity.WxId;
			parameters[8].Value = pEntity.WxOpenId;
			parameters[9].Value = pEntity.WxNickName;
			parameters[10].Value = pEntity.WxSex;
			parameters[11].Value = pEntity.WxCity;
			parameters[12].Value = pEntity.WxCountry;
			parameters[13].Value = pEntity.WxProvince;
			parameters[14].Value = pEntity.WxLanguage;
			parameters[15].Value = pEntity.WxHeadImgUrl;
			parameters[16].Value = pEntity.WxSubscribeTime;
			parameters[17].Value = pEntity.SubscribeStatus;
			parameters[18].Value = pEntity.IdCard;
			parameters[19].Value = pEntity.Email;
			parameters[20].Value = pEntity.Birthday;
			parameters[21].Value = pEntity.IntentionCarTypeID;
			parameters[22].Value = pEntity.IntentionCarStyleID;
			parameters[23].Value = pEntity.BuyCarTypeID;
			parameters[24].Value = pEntity.BuyCarStyleID;
			parameters[25].Value = pEntity.StartFansDate;
			parameters[26].Value = pEntity.EndFansDate;
			parameters[27].Value = pEntity.StartPotentialDate;
			parameters[28].Value = pEntity.EndPotentialDate;
			parameters[29].Value = pEntity.StartCustomerDate;
			parameters[30].Value = pEntity.UnSubscribeDate;
			parameters[31].Value = pEntity.Remark;
			parameters[32].Value = pEntity.ClientID;
			parameters[33].Value = pEntity.CreateBy;
			parameters[34].Value = pEntity.CreateTime;
			parameters[35].Value = pEntity.LastUpdateBy;
			parameters[36].Value = pEntity.LastUpdateTime;
			parameters[37].Value = pEntity.IsDelete;

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
        public CustomerEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Customer] where ID='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            CustomerEntity m = null;
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
        public CustomerEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Customer] where isdelete=0");
            //读取数据
            List<CustomerEntity> list = new List<CustomerEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CustomerEntity m;
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
        public void Update(CustomerEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(CustomerEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [Customer] set ");
                        if (pIsUpdateNullField || pEntity.Name!=null)
                strSql.Append( "[Name]=@Name,");
            if (pIsUpdateNullField || pEntity.Code!=null)
                strSql.Append( "[Code]=@Code,");
            if (pIsUpdateNullField || pEntity.Type!=null)
                strSql.Append( "[Type]=@Type,");
            if (pIsUpdateNullField || pEntity.Phone!=null)
                strSql.Append( "[Phone]=@Phone,");
            if (pIsUpdateNullField || pEntity.LicensePlateNumber!=null)
                strSql.Append( "[LicensePlateNumber]=@LicensePlateNumber,");
            if (pIsUpdateNullField || pEntity.CarFrameNumber!=null)
                strSql.Append( "[CarFrameNumber]=@CarFrameNumber,");
            if (pIsUpdateNullField || pEntity.EngineNumber!=null)
                strSql.Append( "[EngineNumber]=@EngineNumber,");
            if (pIsUpdateNullField || pEntity.WxId!=null)
                strSql.Append( "[WxId]=@WxId,");
            if (pIsUpdateNullField || pEntity.WxOpenId!=null)
                strSql.Append( "[WxOpenId]=@WxOpenId,");
            if (pIsUpdateNullField || pEntity.WxNickName!=null)
                strSql.Append( "[WxNickName]=@WxNickName,");
            if (pIsUpdateNullField || pEntity.WxSex!=null)
                strSql.Append( "[WxSex]=@WxSex,");
            if (pIsUpdateNullField || pEntity.WxCity!=null)
                strSql.Append( "[WxCity]=@WxCity,");
            if (pIsUpdateNullField || pEntity.WxCountry!=null)
                strSql.Append( "[WxCountry]=@WxCountry,");
            if (pIsUpdateNullField || pEntity.WxProvince!=null)
                strSql.Append( "[WxProvince]=@WxProvince,");
            if (pIsUpdateNullField || pEntity.WxLanguage!=null)
                strSql.Append( "[WxLanguage]=@WxLanguage,");
            if (pIsUpdateNullField || pEntity.WxHeadImgUrl!=null)
                strSql.Append( "[WxHeadImgUrl]=@WxHeadImgUrl,");
            if (pIsUpdateNullField || pEntity.WxSubscribeTime!=null)
                strSql.Append( "[WxSubscribeTime]=@WxSubscribeTime,");
            if (pIsUpdateNullField || pEntity.SubscribeStatus!=null)
                strSql.Append( "[SubscribeStatus]=@SubscribeStatus,");
            if (pIsUpdateNullField || pEntity.IdCard!=null)
                strSql.Append( "[IdCard]=@IdCard,");
            if (pIsUpdateNullField || pEntity.Email!=null)
                strSql.Append( "[Email]=@Email,");
            if (pIsUpdateNullField || pEntity.Birthday!=null)
                strSql.Append( "[Birthday]=@Birthday,");
            if (pIsUpdateNullField || pEntity.IntentionCarTypeID!=null)
                strSql.Append( "[IntentionCarTypeID]=@IntentionCarTypeID,");
            if (pIsUpdateNullField || pEntity.IntentionCarStyleID!=null)
                strSql.Append( "[IntentionCarStyleID]=@IntentionCarStyleID,");
            if (pIsUpdateNullField || pEntity.BuyCarTypeID!=null)
                strSql.Append( "[BuyCarTypeID]=@BuyCarTypeID,");
            if (pIsUpdateNullField || pEntity.BuyCarStyleID!=null)
                strSql.Append( "[BuyCarStyleID]=@BuyCarStyleID,");
            if (pIsUpdateNullField || pEntity.StartFansDate!=null)
                strSql.Append( "[StartFansDate]=@StartFansDate,");
            if (pIsUpdateNullField || pEntity.EndFansDate!=null)
                strSql.Append( "[EndFansDate]=@EndFansDate,");
            if (pIsUpdateNullField || pEntity.StartPotentialDate!=null)
                strSql.Append( "[StartPotentialDate]=@StartPotentialDate,");
            if (pIsUpdateNullField || pEntity.EndPotentialDate!=null)
                strSql.Append( "[EndPotentialDate]=@EndPotentialDate,");
            if (pIsUpdateNullField || pEntity.StartCustomerDate!=null)
                strSql.Append( "[StartCustomerDate]=@StartCustomerDate,");
            if (pIsUpdateNullField || pEntity.UnSubscribeDate!=null)
                strSql.Append( "[UnSubscribeDate]=@UnSubscribeDate,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
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
					new SqlParameter("@Code",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@Phone",SqlDbType.NVarChar),
					new SqlParameter("@LicensePlateNumber",SqlDbType.NVarChar),
					new SqlParameter("@CarFrameNumber",SqlDbType.NVarChar),
					new SqlParameter("@EngineNumber",SqlDbType.NVarChar),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@WxNickName",SqlDbType.NVarChar),
					new SqlParameter("@WxSex",SqlDbType.NVarChar),
					new SqlParameter("@WxCity",SqlDbType.NVarChar),
					new SqlParameter("@WxCountry",SqlDbType.NVarChar),
					new SqlParameter("@WxProvince",SqlDbType.NVarChar),
					new SqlParameter("@WxLanguage",SqlDbType.NVarChar),
					new SqlParameter("@WxHeadImgUrl",SqlDbType.NVarChar),
					new SqlParameter("@WxSubscribeTime",SqlDbType.NVarChar),
					new SqlParameter("@SubscribeStatus",SqlDbType.NVarChar),
					new SqlParameter("@IdCard",SqlDbType.NVarChar),
					new SqlParameter("@Email",SqlDbType.NVarChar),
					new SqlParameter("@Birthday",SqlDbType.DateTime),
					new SqlParameter("@IntentionCarTypeID",SqlDbType.Int),
					new SqlParameter("@IntentionCarStyleID",SqlDbType.Int),
					new SqlParameter("@BuyCarTypeID",SqlDbType.Int),
					new SqlParameter("@BuyCarStyleID",SqlDbType.Int),
					new SqlParameter("@StartFansDate",SqlDbType.DateTime),
					new SqlParameter("@EndFansDate",SqlDbType.DateTime),
					new SqlParameter("@StartPotentialDate",SqlDbType.DateTime),
					new SqlParameter("@EndPotentialDate",SqlDbType.DateTime),
					new SqlParameter("@StartCustomerDate",SqlDbType.DateTime),
					new SqlParameter("@UnSubscribeDate",SqlDbType.DateTime),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.Name;
			parameters[1].Value = pEntity.Code;
			parameters[2].Value = pEntity.Type;
			parameters[3].Value = pEntity.Phone;
			parameters[4].Value = pEntity.LicensePlateNumber;
			parameters[5].Value = pEntity.CarFrameNumber;
			parameters[6].Value = pEntity.EngineNumber;
			parameters[7].Value = pEntity.WxId;
			parameters[8].Value = pEntity.WxOpenId;
			parameters[9].Value = pEntity.WxNickName;
			parameters[10].Value = pEntity.WxSex;
			parameters[11].Value = pEntity.WxCity;
			parameters[12].Value = pEntity.WxCountry;
			parameters[13].Value = pEntity.WxProvince;
			parameters[14].Value = pEntity.WxLanguage;
			parameters[15].Value = pEntity.WxHeadImgUrl;
			parameters[16].Value = pEntity.WxSubscribeTime;
			parameters[17].Value = pEntity.SubscribeStatus;
			parameters[18].Value = pEntity.IdCard;
			parameters[19].Value = pEntity.Email;
			parameters[20].Value = pEntity.Birthday;
			parameters[21].Value = pEntity.IntentionCarTypeID;
			parameters[22].Value = pEntity.IntentionCarStyleID;
			parameters[23].Value = pEntity.BuyCarTypeID;
			parameters[24].Value = pEntity.BuyCarStyleID;
			parameters[25].Value = pEntity.StartFansDate;
			parameters[26].Value = pEntity.EndFansDate;
			parameters[27].Value = pEntity.StartPotentialDate;
			parameters[28].Value = pEntity.EndPotentialDate;
			parameters[29].Value = pEntity.StartCustomerDate;
			parameters[30].Value = pEntity.UnSubscribeDate;
			parameters[31].Value = pEntity.Remark;
			parameters[32].Value = pEntity.ClientID;
			parameters[33].Value = pEntity.LastUpdateBy;
			parameters[34].Value = pEntity.LastUpdateTime;
			parameters[35].Value = pEntity.ID;

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
        public void Update(CustomerEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(CustomerEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(CustomerEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [Customer] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(CustomerEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(CustomerEntity[] pEntities)
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
            sql.AppendLine("update [Customer] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public CustomerEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Customer] where isdelete=0 ");
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
            List<CustomerEntity> list = new List<CustomerEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CustomerEntity m;
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
        public PagedQueryResult<CustomerEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [Customer] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [Customer] where isdelete=0 ");
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
            PagedQueryResult<CustomerEntity> result = new PagedQueryResult<CustomerEntity>();
            List<CustomerEntity> list = new List<CustomerEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    CustomerEntity m;
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
        public CustomerEntity[] QueryByEntity(CustomerEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<CustomerEntity> PagedQueryByEntity(CustomerEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(CustomerEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.Name!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Name", Value = pQueryEntity.Name });
            if (pQueryEntity.Code!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Code", Value = pQueryEntity.Code });
            if (pQueryEntity.Type!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Type", Value = pQueryEntity.Type });
            if (pQueryEntity.Phone!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Phone", Value = pQueryEntity.Phone });
            if (pQueryEntity.LicensePlateNumber!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LicensePlateNumber", Value = pQueryEntity.LicensePlateNumber });
            if (pQueryEntity.CarFrameNumber!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CarFrameNumber", Value = pQueryEntity.CarFrameNumber });
            if (pQueryEntity.EngineNumber!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EngineNumber", Value = pQueryEntity.EngineNumber });
            if (pQueryEntity.WxId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxId", Value = pQueryEntity.WxId });
            if (pQueryEntity.WxOpenId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxOpenId", Value = pQueryEntity.WxOpenId });
            if (pQueryEntity.WxNickName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxNickName", Value = pQueryEntity.WxNickName });
            if (pQueryEntity.WxSex!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxSex", Value = pQueryEntity.WxSex });
            if (pQueryEntity.WxCity!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxCity", Value = pQueryEntity.WxCity });
            if (pQueryEntity.WxCountry!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxCountry", Value = pQueryEntity.WxCountry });
            if (pQueryEntity.WxProvince!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxProvince", Value = pQueryEntity.WxProvince });
            if (pQueryEntity.WxLanguage!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxLanguage", Value = pQueryEntity.WxLanguage });
            if (pQueryEntity.WxHeadImgUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxHeadImgUrl", Value = pQueryEntity.WxHeadImgUrl });
            if (pQueryEntity.WxSubscribeTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxSubscribeTime", Value = pQueryEntity.WxSubscribeTime });
            if (pQueryEntity.SubscribeStatus!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SubscribeStatus", Value = pQueryEntity.SubscribeStatus });
            if (pQueryEntity.IdCard!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IdCard", Value = pQueryEntity.IdCard });
            if (pQueryEntity.Email!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Email", Value = pQueryEntity.Email });
            if (pQueryEntity.Birthday!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Birthday", Value = pQueryEntity.Birthday });
            if (pQueryEntity.IntentionCarTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IntentionCarTypeID", Value = pQueryEntity.IntentionCarTypeID });
            if (pQueryEntity.IntentionCarStyleID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IntentionCarStyleID", Value = pQueryEntity.IntentionCarStyleID });
            if (pQueryEntity.BuyCarTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BuyCarTypeID", Value = pQueryEntity.BuyCarTypeID });
            if (pQueryEntity.BuyCarStyleID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BuyCarStyleID", Value = pQueryEntity.BuyCarStyleID });
            if (pQueryEntity.StartFansDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "StartFansDate", Value = pQueryEntity.StartFansDate });
            if (pQueryEntity.EndFansDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EndFansDate", Value = pQueryEntity.EndFansDate });
            if (pQueryEntity.StartPotentialDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "StartPotentialDate", Value = pQueryEntity.StartPotentialDate });
            if (pQueryEntity.EndPotentialDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EndPotentialDate", Value = pQueryEntity.EndPotentialDate });
            if (pQueryEntity.StartCustomerDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "StartCustomerDate", Value = pQueryEntity.StartCustomerDate });
            if (pQueryEntity.UnSubscribeDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "UnSubscribeDate", Value = pQueryEntity.UnSubscribeDate });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
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
        protected void Load(SqlDataReader pReader, out CustomerEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new CustomerEntity();
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
			if (pReader["Code"] != DBNull.Value)
			{
				pInstance.Code =  Convert.ToString(pReader["Code"]);
			}
			if (pReader["Type"] != DBNull.Value)
			{
				pInstance.Type =  Convert.ToString(pReader["Type"]);
			}
			if (pReader["Phone"] != DBNull.Value)
			{
				pInstance.Phone =  Convert.ToString(pReader["Phone"]);
			}
			if (pReader["LicensePlateNumber"] != DBNull.Value)
			{
				pInstance.LicensePlateNumber =  Convert.ToString(pReader["LicensePlateNumber"]);
			}
			if (pReader["CarFrameNumber"] != DBNull.Value)
			{
				pInstance.CarFrameNumber =  Convert.ToString(pReader["CarFrameNumber"]);
			}
			if (pReader["EngineNumber"] != DBNull.Value)
			{
				pInstance.EngineNumber =  Convert.ToString(pReader["EngineNumber"]);
			}
			if (pReader["WxId"] != DBNull.Value)
			{
				pInstance.WxId =  Convert.ToString(pReader["WxId"]);
			}
			if (pReader["WxOpenId"] != DBNull.Value)
			{
				pInstance.WxOpenId =  Convert.ToString(pReader["WxOpenId"]);
			}
			if (pReader["WxNickName"] != DBNull.Value)
			{
				pInstance.WxNickName =  Convert.ToString(pReader["WxNickName"]);
			}
			if (pReader["WxSex"] != DBNull.Value)
			{
				pInstance.WxSex =  Convert.ToString(pReader["WxSex"]);
			}
			if (pReader["WxCity"] != DBNull.Value)
			{
				pInstance.WxCity =  Convert.ToString(pReader["WxCity"]);
			}
			if (pReader["WxCountry"] != DBNull.Value)
			{
				pInstance.WxCountry =  Convert.ToString(pReader["WxCountry"]);
			}
			if (pReader["WxProvince"] != DBNull.Value)
			{
				pInstance.WxProvince =  Convert.ToString(pReader["WxProvince"]);
			}
			if (pReader["WxLanguage"] != DBNull.Value)
			{
				pInstance.WxLanguage =  Convert.ToString(pReader["WxLanguage"]);
			}
			if (pReader["WxHeadImgUrl"] != DBNull.Value)
			{
				pInstance.WxHeadImgUrl =  Convert.ToString(pReader["WxHeadImgUrl"]);
			}
			if (pReader["WxSubscribeTime"] != DBNull.Value)
			{
				pInstance.WxSubscribeTime =  Convert.ToString(pReader["WxSubscribeTime"]);
			}
			if (pReader["SubscribeStatus"] != DBNull.Value)
			{
				pInstance.SubscribeStatus =  Convert.ToString(pReader["SubscribeStatus"]);
			}
			if (pReader["IdCard"] != DBNull.Value)
			{
				pInstance.IdCard =  Convert.ToString(pReader["IdCard"]);
			}
			if (pReader["Email"] != DBNull.Value)
			{
				pInstance.Email =  Convert.ToString(pReader["Email"]);
			}
			if (pReader["Birthday"] != DBNull.Value)
			{
				pInstance.Birthday =  Convert.ToDateTime(pReader["Birthday"]);
			}
			if (pReader["IntentionCarTypeID"] != DBNull.Value)
			{
				pInstance.IntentionCarTypeID =   Convert.ToInt32(pReader["IntentionCarTypeID"]);
			}
			if (pReader["IntentionCarStyleID"] != DBNull.Value)
			{
				pInstance.IntentionCarStyleID =   Convert.ToInt32(pReader["IntentionCarStyleID"]);
			}
			if (pReader["BuyCarTypeID"] != DBNull.Value)
			{
				pInstance.BuyCarTypeID =   Convert.ToInt32(pReader["BuyCarTypeID"]);
			}
			if (pReader["BuyCarStyleID"] != DBNull.Value)
			{
				pInstance.BuyCarStyleID =   Convert.ToInt32(pReader["BuyCarStyleID"]);
			}
			if (pReader["StartFansDate"] != DBNull.Value)
			{
				pInstance.StartFansDate =  Convert.ToDateTime(pReader["StartFansDate"]);
			}
			if (pReader["EndFansDate"] != DBNull.Value)
			{
				pInstance.EndFansDate =  Convert.ToDateTime(pReader["EndFansDate"]);
			}
			if (pReader["StartPotentialDate"] != DBNull.Value)
			{
				pInstance.StartPotentialDate =  Convert.ToDateTime(pReader["StartPotentialDate"]);
			}
			if (pReader["EndPotentialDate"] != DBNull.Value)
			{
				pInstance.EndPotentialDate =  Convert.ToDateTime(pReader["EndPotentialDate"]);
			}
			if (pReader["StartCustomerDate"] != DBNull.Value)
			{
				pInstance.StartCustomerDate =  Convert.ToDateTime(pReader["StartCustomerDate"]);
			}
			if (pReader["UnSubscribeDate"] != DBNull.Value)
			{
				pInstance.UnSubscribeDate =  Convert.ToDateTime(pReader["UnSubscribeDate"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
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
