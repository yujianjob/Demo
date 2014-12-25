/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/21 17:24:02
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
    /// ���ݷ��ʣ� 1301��������� BuyCarCalculation 
    /// ��BuyCarCalculation�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class BuyCarCalculationDAO : BaseDAO<BasicUserInfo>, ICRUDable<BuyCarCalculationEntity>, IQueryable<BuyCarCalculationEntity>
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public BuyCarCalculationDAO(BasicUserInfo pUserInfo)
            : base(pUserInfo, ConfigInfo.CURRENT_CONNECTION_STRING_MANAGER)
        {
            this.SQLHelper.OnExecuted += new EventHandler<SqlCommandExecutionEventArgs>(SQLHelper_OnExecuted);
        }
        #endregion

        #region �¼�����
        /// <summary>
        /// SQL����ִ����Ϻ󣬼�¼��־
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SQLHelper_OnExecuted(object sender, SqlCommandExecutionEventArgs e)
        {
            if (e != null)
            {
                var log = new DatabaseLogInfo();
                //��ȡ�û���Ϣ
                if (e.UserInfo != null)
                {
                    log.ClientID = e.UserInfo.ClientID;
                    log.UserID = e.UserInfo.UserID;
                }
                //��ȡT-SQL�����Ϣ
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

        #region ICRUDable ��Ա
        /// <summary>
        /// ����һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        public void Create(BuyCarCalculationEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(BuyCarCalculationEntity pEntity, IDbTransaction pTran)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //��ʼ���̶��ֶ�
            pEntity.CreateTime = DateTime.Now;
            pEntity.CreateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.LastUpdateTime = pEntity.CreateTime;
            pEntity.LastUpdateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.IsDelete = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BuyCarCalculation](");
            strSql.Append("[CalculationType],[CarStyleID],[Price],[TotalPrice],[CompulsoryInsurance],[VehicleUseTax],[ThirdParty],[VehicleDamage],[WholeVehiclePilfer],[BreakageGlass],[SpontaneousCombustion],[NonDeductible],[NoLiability],[PassengerLiability],[BodyScratch],[ShoufuRatio],[RepaymentPeriod],[WxId],[WxOpenId],[ClientID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@CalculationType,@CarStyleID,@Price,@TotalPrice,@CompulsoryInsurance,@VehicleUseTax,@ThirdParty,@VehicleDamage,@WholeVehiclePilfer,@BreakageGlass,@SpontaneousCombustion,@NonDeductible,@NoLiability,@PassengerLiability,@BodyScratch,@ShoufuRatio,@RepaymentPeriod,@WxId,@WxOpenId,@ClientID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@CalculationType",SqlDbType.NVarChar),
					new SqlParameter("@CarStyleID",SqlDbType.Int),
					new SqlParameter("@Price",SqlDbType.Int),
					new SqlParameter("@TotalPrice",SqlDbType.Int),
					new SqlParameter("@CompulsoryInsurance",SqlDbType.Int),
					new SqlParameter("@VehicleUseTax",SqlDbType.Int),
					new SqlParameter("@ThirdParty",SqlDbType.Int),
					new SqlParameter("@VehicleDamage",SqlDbType.Int),
					new SqlParameter("@WholeVehiclePilfer",SqlDbType.Int),
					new SqlParameter("@BreakageGlass",SqlDbType.Int),
					new SqlParameter("@SpontaneousCombustion",SqlDbType.Int),
					new SqlParameter("@NonDeductible",SqlDbType.Int),
					new SqlParameter("@NoLiability",SqlDbType.Int),
					new SqlParameter("@PassengerLiability",SqlDbType.Int),
					new SqlParameter("@BodyScratch",SqlDbType.Int),
					new SqlParameter("@ShoufuRatio",SqlDbType.Int),
					new SqlParameter("@RepaymentPeriod",SqlDbType.Int),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.CalculationType;
			parameters[1].Value = pEntity.CarStyleID;
			parameters[2].Value = pEntity.Price;
			parameters[3].Value = pEntity.TotalPrice;
			parameters[4].Value = pEntity.CompulsoryInsurance;
			parameters[5].Value = pEntity.VehicleUseTax;
			parameters[6].Value = pEntity.ThirdParty;
			parameters[7].Value = pEntity.VehicleDamage;
			parameters[8].Value = pEntity.WholeVehiclePilfer;
			parameters[9].Value = pEntity.BreakageGlass;
			parameters[10].Value = pEntity.SpontaneousCombustion;
			parameters[11].Value = pEntity.NonDeductible;
			parameters[12].Value = pEntity.NoLiability;
			parameters[13].Value = pEntity.PassengerLiability;
			parameters[14].Value = pEntity.BodyScratch;
			parameters[15].Value = pEntity.ShoufuRatio;
			parameters[16].Value = pEntity.RepaymentPeriod;
			parameters[17].Value = pEntity.WxId;
			parameters[18].Value = pEntity.WxOpenId;
			parameters[19].Value = pEntity.ClientID;
			parameters[20].Value = pEntity.CreateBy;
			parameters[21].Value = pEntity.CreateTime;
			parameters[22].Value = pEntity.LastUpdateBy;
			parameters[23].Value = pEntity.LastUpdateTime;
			parameters[24].Value = pEntity.IsDelete;

            //ִ�в��������д
            object result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteScalar((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.ID = Convert.ToInt32(result);
        }

        /// <summary>
        /// ���ݱ�ʶ����ȡʵ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        public BuyCarCalculationEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [BuyCarCalculation] where ID='{0}' and IsDelete=0 ", id.ToString());
            //��ȡ����
            BuyCarCalculationEntity m = null;
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    this.Load(rdr, out m);
                    break;
                }
            }
            //����
            return m;
        }

        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <returns></returns>
        public BuyCarCalculationEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [BuyCarCalculation] where isdelete=0");
            //��ȡ����
            List<BuyCarCalculationEntity> list = new List<BuyCarCalculationEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    BuyCarCalculationEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            //����
            return list.ToArray();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(BuyCarCalculationEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(BuyCarCalculationEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.ID.HasValue)
            {
                throw new ArgumentException("ִ�и���ʱ,ʵ�����������ֵ����Ϊnull.");
            }
             //��ʼ���̶��ֶ�
            pEntity.LastUpdateTime = DateTime.Now;
            pEntity.LastUpdateBy = Convert.ToInt32(CurrentUserInfo.UserID);

            //��֯������SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BuyCarCalculation] set ");
                        if (pIsUpdateNullField || pEntity.CalculationType!=null)
                strSql.Append( "[CalculationType]=@CalculationType,");
            if (pIsUpdateNullField || pEntity.CarStyleID!=null)
                strSql.Append( "[CarStyleID]=@CarStyleID,");
            if (pIsUpdateNullField || pEntity.Price!=null)
                strSql.Append( "[Price]=@Price,");
            if (pIsUpdateNullField || pEntity.TotalPrice!=null)
                strSql.Append( "[TotalPrice]=@TotalPrice,");
            if (pIsUpdateNullField || pEntity.CompulsoryInsurance!=null)
                strSql.Append( "[CompulsoryInsurance]=@CompulsoryInsurance,");
            if (pIsUpdateNullField || pEntity.VehicleUseTax!=null)
                strSql.Append( "[VehicleUseTax]=@VehicleUseTax,");
            if (pIsUpdateNullField || pEntity.ThirdParty!=null)
                strSql.Append( "[ThirdParty]=@ThirdParty,");
            if (pIsUpdateNullField || pEntity.VehicleDamage!=null)
                strSql.Append( "[VehicleDamage]=@VehicleDamage,");
            if (pIsUpdateNullField || pEntity.WholeVehiclePilfer!=null)
                strSql.Append( "[WholeVehiclePilfer]=@WholeVehiclePilfer,");
            if (pIsUpdateNullField || pEntity.BreakageGlass!=null)
                strSql.Append( "[BreakageGlass]=@BreakageGlass,");
            if (pIsUpdateNullField || pEntity.SpontaneousCombustion!=null)
                strSql.Append( "[SpontaneousCombustion]=@SpontaneousCombustion,");
            if (pIsUpdateNullField || pEntity.NonDeductible!=null)
                strSql.Append( "[NonDeductible]=@NonDeductible,");
            if (pIsUpdateNullField || pEntity.NoLiability!=null)
                strSql.Append( "[NoLiability]=@NoLiability,");
            if (pIsUpdateNullField || pEntity.PassengerLiability!=null)
                strSql.Append( "[PassengerLiability]=@PassengerLiability,");
            if (pIsUpdateNullField || pEntity.BodyScratch!=null)
                strSql.Append( "[BodyScratch]=@BodyScratch,");
            if (pIsUpdateNullField || pEntity.ShoufuRatio!=null)
                strSql.Append( "[ShoufuRatio]=@ShoufuRatio,");
            if (pIsUpdateNullField || pEntity.RepaymentPeriod!=null)
                strSql.Append( "[RepaymentPeriod]=@RepaymentPeriod,");
            if (pIsUpdateNullField || pEntity.WxId!=null)
                strSql.Append( "[WxId]=@WxId,");
            if (pIsUpdateNullField || pEntity.WxOpenId!=null)
                strSql.Append( "[WxOpenId]=@WxOpenId,");
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
					new SqlParameter("@CalculationType",SqlDbType.NVarChar),
					new SqlParameter("@CarStyleID",SqlDbType.Int),
					new SqlParameter("@Price",SqlDbType.Int),
					new SqlParameter("@TotalPrice",SqlDbType.Int),
					new SqlParameter("@CompulsoryInsurance",SqlDbType.Int),
					new SqlParameter("@VehicleUseTax",SqlDbType.Int),
					new SqlParameter("@ThirdParty",SqlDbType.Int),
					new SqlParameter("@VehicleDamage",SqlDbType.Int),
					new SqlParameter("@WholeVehiclePilfer",SqlDbType.Int),
					new SqlParameter("@BreakageGlass",SqlDbType.Int),
					new SqlParameter("@SpontaneousCombustion",SqlDbType.Int),
					new SqlParameter("@NonDeductible",SqlDbType.Int),
					new SqlParameter("@NoLiability",SqlDbType.Int),
					new SqlParameter("@PassengerLiability",SqlDbType.Int),
					new SqlParameter("@BodyScratch",SqlDbType.Int),
					new SqlParameter("@ShoufuRatio",SqlDbType.Int),
					new SqlParameter("@RepaymentPeriod",SqlDbType.Int),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.CalculationType;
			parameters[1].Value = pEntity.CarStyleID;
			parameters[2].Value = pEntity.Price;
			parameters[3].Value = pEntity.TotalPrice;
			parameters[4].Value = pEntity.CompulsoryInsurance;
			parameters[5].Value = pEntity.VehicleUseTax;
			parameters[6].Value = pEntity.ThirdParty;
			parameters[7].Value = pEntity.VehicleDamage;
			parameters[8].Value = pEntity.WholeVehiclePilfer;
			parameters[9].Value = pEntity.BreakageGlass;
			parameters[10].Value = pEntity.SpontaneousCombustion;
			parameters[11].Value = pEntity.NonDeductible;
			parameters[12].Value = pEntity.NoLiability;
			parameters[13].Value = pEntity.PassengerLiability;
			parameters[14].Value = pEntity.BodyScratch;
			parameters[15].Value = pEntity.ShoufuRatio;
			parameters[16].Value = pEntity.RepaymentPeriod;
			parameters[17].Value = pEntity.WxId;
			parameters[18].Value = pEntity.WxOpenId;
			parameters[19].Value = pEntity.ClientID;
			parameters[20].Value = pEntity.LastUpdateBy;
			parameters[21].Value = pEntity.LastUpdateTime;
			parameters[22].Value = pEntity.ID;

            //ִ�����
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        public void Update(BuyCarCalculationEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(BuyCarCalculationEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(BuyCarCalculationEntity pEntity, IDbTransaction pTran)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.ID.HasValue)
            {
                throw new ArgumentException("ִ��ɾ��ʱ,ʵ�����������ֵ����Ϊnull.");
            }
            //ִ�� 
            this.Delete(pEntity.ID.Value, pTran);           
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(object pID, IDbTransaction pTran)
        {
            if (pID == null)
                return ;   
            //��֯������SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [BuyCarCalculation] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@LastUpdateTime",SqlDbType=SqlDbType.DateTime,Value=DateTime.Now},
                new SqlParameter{ParameterName="@LastUpdateBy",SqlDbType=SqlDbType.Int,Value=Convert.ToInt32(CurrentUserInfo.UserID)},
                new SqlParameter{ParameterName="@ID",SqlDbType=SqlDbType.VarChar,Value=pID}
            };
            //ִ�����
            int result = 0;
            if (pTran != null)
                result=this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, sql.ToString(), parameters);
            else
                result=this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters);
            return ;
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(BuyCarCalculationEntity[] pEntities, IDbTransaction pTran)
        {
            //��������ֵ
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //����У��
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (!pEntity.ID.HasValue)
                {
                    throw new ArgumentException("ִ��ɾ��ʱ,ʵ�����������ֵ����Ϊnull.");
                }
                entityIDs[i] = pEntity.ID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        public void Delete(BuyCarCalculationEntity[] pEntities)
        { 
            Delete(pEntities, null);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        public void Delete(object[] pIDs)
        {
            Delete(pIDs,null);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(object[] pIDs, IDbTransaction pTran) 
        {
            if (pIDs == null || pIDs.Length==0)
                return ;
            //��֯������SQL
            StringBuilder primaryKeys = new StringBuilder();
            foreach (object item in pIDs)
            {
                primaryKeys.AppendFormat("{0},",item.ToString());
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [BuyCarCalculation] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
            //ִ�����
            int result = 0;   
            if (pTran == null)
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), null);
            else
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran,CommandType.Text, sql.ToString());       
        }
        #endregion

        #region IQueryable ��Ա
        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <param name="pWhereConditions">ɸѡ����</param>
        /// <param name="pOrderBys">����</param>
        /// <returns></returns>
        public BuyCarCalculationEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [BuyCarCalculation] where isdelete=0 ");
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
            //ִ��SQL
            List<BuyCarCalculationEntity> list = new List<BuyCarCalculationEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    BuyCarCalculationEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            //���ؽ��
            return list.ToArray();
        }
        /// <summary>
        /// ִ�з�ҳ��ѯ
        /// </summary>
        /// <param name="pWhereConditions">ɸѡ����</param>
        /// <param name="pOrderBys">����</param>
        /// <param name="pPageSize">ÿҳ�ļ�¼��</param>
        /// <param name="pCurrentPageIndex">��0��ʼ�ĵ�ǰҳ��</param>
        /// <returns></returns>
        public PagedQueryResult<BuyCarCalculationEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            //��֯SQL
            StringBuilder pagedSql = new StringBuilder();
            StringBuilder totalCountSql = new StringBuilder();
            //��ҳSQL
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
                pagedSql.AppendFormat(" [ID] desc"); //Ĭ��Ϊ����ֵ����
            }
            pagedSql.AppendFormat(") as ___rn,* from [BuyCarCalculation] where isdelete=0 ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [BuyCarCalculation] where isdelete=0 ");
            //��������
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
            //ȡָ��ҳ������
            pagedSql.AppendFormat(" where ___rn >{0} and ___rn <={1}", pPageSize * (pCurrentPageIndex-1), pPageSize * (pCurrentPageIndex));
            //ִ����䲢���ؽ��
            PagedQueryResult<BuyCarCalculationEntity> result = new PagedQueryResult<BuyCarCalculationEntity>();
            List<BuyCarCalculationEntity> list = new List<BuyCarCalculationEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    BuyCarCalculationEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            result.Entities = list.ToArray();
            int totalCount = Convert.ToInt32(this.SQLHelper.ExecuteScalar(totalCountSql.ToString()));    //����������
            result.RowCount = totalCount;
            int remainder = 0;
            result.PageCount = Math.DivRem(totalCount, pPageSize, out remainder);
            if (remainder > 0)
                result.PageCount++;
            return result;
        }

        /// <summary>
        /// ����ʵ��������ѯʵ��
        /// </summary>
        /// <param name="pQueryEntity">��ʵ����ʽ����Ĳ���</param>
        /// <param name="pOrderBys">�������</param>
        /// <returns>����������ʵ�弯</returns>
        public BuyCarCalculationEntity[] QueryByEntity(BuyCarCalculationEntity pQueryEntity, OrderBy[] pOrderBys)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity(pQueryEntity);
            return Query(queryWhereCondition,  pOrderBys);            
        }

        /// <summary>
        /// ��ҳ����ʵ��������ѯʵ��
        /// </summary>
        /// <param name="pQueryEntity">��ʵ����ʽ����Ĳ���</param>
        /// <param name="pOrderBys">�������</param>
        /// <returns>����������ʵ�弯</returns>
        public PagedQueryResult<BuyCarCalculationEntity> PagedQueryByEntity(BuyCarCalculationEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity( pQueryEntity);
            return PagedQuery(queryWhereCondition, pOrderBys, pPageSize, pCurrentPageIndex);
        }

        #endregion

        #region ���߷���
        /// <summary>
        /// ����ʵ���Null�������ɲ�ѯ������
        /// </summary>
        /// <returns></returns>
        protected IWhereCondition[] GetWhereConditionByEntity(BuyCarCalculationEntity pQueryEntity)
        { 
            //��ȡ�ǿ���������
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.CalculationType!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CalculationType", Value = pQueryEntity.CalculationType });
            if (pQueryEntity.CarStyleID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CarStyleID", Value = pQueryEntity.CarStyleID });
            if (pQueryEntity.Price!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Price", Value = pQueryEntity.Price });
            if (pQueryEntity.TotalPrice!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "TotalPrice", Value = pQueryEntity.TotalPrice });
            if (pQueryEntity.CompulsoryInsurance!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CompulsoryInsurance", Value = pQueryEntity.CompulsoryInsurance });
            if (pQueryEntity.VehicleUseTax!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VehicleUseTax", Value = pQueryEntity.VehicleUseTax });
            if (pQueryEntity.ThirdParty!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ThirdParty", Value = pQueryEntity.ThirdParty });
            if (pQueryEntity.VehicleDamage!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VehicleDamage", Value = pQueryEntity.VehicleDamage });
            if (pQueryEntity.WholeVehiclePilfer!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WholeVehiclePilfer", Value = pQueryEntity.WholeVehiclePilfer });
            if (pQueryEntity.BreakageGlass!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BreakageGlass", Value = pQueryEntity.BreakageGlass });
            if (pQueryEntity.SpontaneousCombustion!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SpontaneousCombustion", Value = pQueryEntity.SpontaneousCombustion });
            if (pQueryEntity.NonDeductible!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "NonDeductible", Value = pQueryEntity.NonDeductible });
            if (pQueryEntity.NoLiability!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "NoLiability", Value = pQueryEntity.NoLiability });
            if (pQueryEntity.PassengerLiability!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PassengerLiability", Value = pQueryEntity.PassengerLiability });
            if (pQueryEntity.BodyScratch!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BodyScratch", Value = pQueryEntity.BodyScratch });
            if (pQueryEntity.ShoufuRatio!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ShoufuRatio", Value = pQueryEntity.ShoufuRatio });
            if (pQueryEntity.RepaymentPeriod!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "RepaymentPeriod", Value = pQueryEntity.RepaymentPeriod });
            if (pQueryEntity.WxId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxId", Value = pQueryEntity.WxId });
            if (pQueryEntity.WxOpenId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxOpenId", Value = pQueryEntity.WxOpenId });
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
        /// װ��ʵ��
        /// </summary>
        /// <param name="pReader">��ǰֻ����</param>
        /// <param name="pInstance">ʵ��ʵ��</param>
        protected void Load(SqlDataReader pReader, out BuyCarCalculationEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
            pInstance = new BuyCarCalculationEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ID"] != DBNull.Value)
			{
				pInstance.ID =   Convert.ToInt32(pReader["ID"]);
			}
			if (pReader["CalculationType"] != DBNull.Value)
			{
				pInstance.CalculationType =  Convert.ToString(pReader["CalculationType"]);
			}
			if (pReader["CarStyleID"] != DBNull.Value)
			{
				pInstance.CarStyleID =   Convert.ToInt32(pReader["CarStyleID"]);
			}
			if (pReader["Price"] != DBNull.Value)
			{
				pInstance.Price =   Convert.ToInt32(pReader["Price"]);
			}
			if (pReader["TotalPrice"] != DBNull.Value)
			{
				pInstance.TotalPrice =   Convert.ToInt32(pReader["TotalPrice"]);
			}
			if (pReader["CompulsoryInsurance"] != DBNull.Value)
			{
				pInstance.CompulsoryInsurance =   Convert.ToInt32(pReader["CompulsoryInsurance"]);
			}
			if (pReader["VehicleUseTax"] != DBNull.Value)
			{
				pInstance.VehicleUseTax =   Convert.ToInt32(pReader["VehicleUseTax"]);
			}
			if (pReader["ThirdParty"] != DBNull.Value)
			{
				pInstance.ThirdParty =   Convert.ToInt32(pReader["ThirdParty"]);
			}
			if (pReader["VehicleDamage"] != DBNull.Value)
			{
				pInstance.VehicleDamage =   Convert.ToInt32(pReader["VehicleDamage"]);
			}
			if (pReader["WholeVehiclePilfer"] != DBNull.Value)
			{
				pInstance.WholeVehiclePilfer =   Convert.ToInt32(pReader["WholeVehiclePilfer"]);
			}
			if (pReader["BreakageGlass"] != DBNull.Value)
			{
				pInstance.BreakageGlass =   Convert.ToInt32(pReader["BreakageGlass"]);
			}
			if (pReader["SpontaneousCombustion"] != DBNull.Value)
			{
				pInstance.SpontaneousCombustion =   Convert.ToInt32(pReader["SpontaneousCombustion"]);
			}
			if (pReader["NonDeductible"] != DBNull.Value)
			{
				pInstance.NonDeductible =   Convert.ToInt32(pReader["NonDeductible"]);
			}
			if (pReader["NoLiability"] != DBNull.Value)
			{
				pInstance.NoLiability =   Convert.ToInt32(pReader["NoLiability"]);
			}
			if (pReader["PassengerLiability"] != DBNull.Value)
			{
				pInstance.PassengerLiability =   Convert.ToInt32(pReader["PassengerLiability"]);
			}
			if (pReader["BodyScratch"] != DBNull.Value)
			{
				pInstance.BodyScratch =   Convert.ToInt32(pReader["BodyScratch"]);
			}
			if (pReader["ShoufuRatio"] != DBNull.Value)
			{
				pInstance.ShoufuRatio =   Convert.ToInt32(pReader["ShoufuRatio"]);
			}
			if (pReader["RepaymentPeriod"] != DBNull.Value)
			{
				pInstance.RepaymentPeriod =   Convert.ToInt32(pReader["RepaymentPeriod"]);
			}
			if (pReader["WxId"] != DBNull.Value)
			{
				pInstance.WxId =  Convert.ToString(pReader["WxId"]);
			}
			if (pReader["WxOpenId"] != DBNull.Value)
			{
				pInstance.WxOpenId =  Convert.ToString(pReader["WxOpenId"]);
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
