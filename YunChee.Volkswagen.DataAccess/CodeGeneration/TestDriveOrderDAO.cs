/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/2 16:32:44
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
    /// ���ݷ��ʣ� 0701ԤԼ�Լݱ� TestDriveOrder 
    /// ��TestDriveOrder�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class TestDriveOrderDAO : BaseDAO<BasicUserInfo>, ICRUDable<TestDriveOrderEntity>, IQueryable<TestDriveOrderEntity>
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public TestDriveOrderDAO(BasicUserInfo pUserInfo)
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
        public void Create(TestDriveOrderEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(TestDriveOrderEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [TestDriveOrder](");
            strSql.Append("[CarTypeID],[CustomerName],[Gender],[Phone],[PlanBuyTime],[CustomerRemark],[SubmitTime],[TargetTime],[IsHandle],[IsSuccess],[Remark],[ClientID],[WxId],[WxOpenId],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@CarTypeID,@CustomerName,@Gender,@Phone,@PlanBuyTime,@CustomerRemark,@SubmitTime,@TargetTime,@IsHandle,@IsSuccess,@Remark,@ClientID,@WxId,@WxOpenId,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");
            strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@CarTypeID",SqlDbType.Int),
					new SqlParameter("@CustomerName",SqlDbType.NVarChar),
					new SqlParameter("@Gender",SqlDbType.NVarChar),
					new SqlParameter("@Phone",SqlDbType.NVarChar),
					new SqlParameter("@PlanBuyTime",SqlDbType.NVarChar),
					new SqlParameter("@CustomerRemark",SqlDbType.NVarChar),
					new SqlParameter("@SubmitTime",SqlDbType.DateTime),
					new SqlParameter("@TargetTime",SqlDbType.DateTime),
					new SqlParameter("@IsHandle",SqlDbType.NVarChar),
					new SqlParameter("@IsSuccess",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
            parameters[0].Value = pEntity.CarTypeID;
            parameters[1].Value = pEntity.CustomerName;
            parameters[2].Value = pEntity.Gender;
            parameters[3].Value = pEntity.Phone;
            parameters[4].Value = pEntity.PlanBuyTime;
            parameters[5].Value = pEntity.CustomerRemark;
            parameters[6].Value = pEntity.SubmitTime;
            parameters[7].Value = pEntity.TargetTime;
            parameters[8].Value = pEntity.IsHandle;
            parameters[9].Value = pEntity.IsSuccess;
            parameters[10].Value = pEntity.Remark;
            parameters[11].Value = pEntity.ClientID;
            parameters[12].Value = pEntity.WxId;
            parameters[13].Value = pEntity.WxOpenId;
            parameters[14].Value = pEntity.CreateBy;
            parameters[15].Value = pEntity.CreateTime;
            parameters[16].Value = pEntity.LastUpdateBy;
            parameters[17].Value = pEntity.LastUpdateTime;
            parameters[18].Value = pEntity.IsDelete;

            //ִ�в��������д
            object result;
            if (pTran != null)
                result = this.SQLHelper.ExecuteScalar((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            pEntity.ID = Convert.ToInt32(result);
        }

        /// <summary>
        /// ���ݱ�ʶ����ȡʵ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        public TestDriveOrderEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [TestDriveOrder] where ID='{0}' and IsDelete=0 ", id.ToString());
            //��ȡ����
            TestDriveOrderEntity m = null;
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
        public TestDriveOrderEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [TestDriveOrder] where isdelete=0");
            //��ȡ����
            List<TestDriveOrderEntity> list = new List<TestDriveOrderEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    TestDriveOrderEntity m;
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
        public void Update(TestDriveOrderEntity pEntity, IDbTransaction pTran)
        {
            Update(pEntity, pTran, true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(TestDriveOrderEntity pEntity, IDbTransaction pTran, bool pIsUpdateNullField)
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
            strSql.Append("update [TestDriveOrder] set ");
            if (pIsUpdateNullField || pEntity.CarTypeID != null)
                strSql.Append("[CarTypeID]=@CarTypeID,");
            if (pIsUpdateNullField || pEntity.CustomerName != null)
                strSql.Append("[CustomerName]=@CustomerName,");
            if (pIsUpdateNullField || pEntity.Gender != null)
                strSql.Append("[Gender]=@Gender,");
            if (pIsUpdateNullField || pEntity.Phone != null)
                strSql.Append("[Phone]=@Phone,");
            if (pIsUpdateNullField || pEntity.PlanBuyTime != null)
                strSql.Append("[PlanBuyTime]=@PlanBuyTime,");
            if (pIsUpdateNullField || pEntity.CustomerRemark != null)
                strSql.Append("[CustomerRemark]=@CustomerRemark,");
            if (pIsUpdateNullField || pEntity.SubmitTime != null)
                strSql.Append("[SubmitTime]=@SubmitTime,");
            if (pIsUpdateNullField || pEntity.TargetTime != null)
                strSql.Append("[TargetTime]=@TargetTime,");
            if (pIsUpdateNullField || pEntity.IsHandle != null)
                strSql.Append("[IsHandle]=@IsHandle,");
            if (pIsUpdateNullField || pEntity.IsSuccess != null)
                strSql.Append("[IsSuccess]=@IsSuccess,");
            if (pIsUpdateNullField || pEntity.Remark != null)
                strSql.Append("[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ClientID != null)
                strSql.Append("[ClientID]=@ClientID,");
            if (pIsUpdateNullField || pEntity.WxId != null)
                strSql.Append("[WxId]=@WxId,");
            if (pIsUpdateNullField || pEntity.WxOpenId != null)
                strSql.Append("[WxOpenId]=@WxOpenId,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy != null)
                strSql.Append("[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime != null)
                strSql.Append("[LastUpdateTime]=@LastUpdateTime");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@CarTypeID",SqlDbType.Int),
					new SqlParameter("@CustomerName",SqlDbType.NVarChar),
					new SqlParameter("@Gender",SqlDbType.NVarChar),
					new SqlParameter("@Phone",SqlDbType.NVarChar),
					new SqlParameter("@PlanBuyTime",SqlDbType.NVarChar),
					new SqlParameter("@CustomerRemark",SqlDbType.NVarChar),
					new SqlParameter("@SubmitTime",SqlDbType.DateTime),
					new SqlParameter("@TargetTime",SqlDbType.DateTime),
					new SqlParameter("@IsHandle",SqlDbType.NVarChar),
					new SqlParameter("@IsSuccess",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@WxId",SqlDbType.NVarChar),
					new SqlParameter("@WxOpenId",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
            parameters[0].Value = pEntity.CarTypeID;
            parameters[1].Value = pEntity.CustomerName;
            parameters[2].Value = pEntity.Gender;
            parameters[3].Value = pEntity.Phone;
            parameters[4].Value = pEntity.PlanBuyTime;
            parameters[5].Value = pEntity.CustomerRemark;
            parameters[6].Value = pEntity.SubmitTime;
            parameters[7].Value = pEntity.TargetTime;
            parameters[8].Value = pEntity.IsHandle;
            parameters[9].Value = pEntity.IsSuccess;
            parameters[10].Value = pEntity.Remark;
            parameters[11].Value = pEntity.ClientID;
            parameters[12].Value = pEntity.WxId;
            parameters[13].Value = pEntity.WxOpenId;
            parameters[14].Value = pEntity.LastUpdateBy;
            parameters[15].Value = pEntity.LastUpdateTime;
            parameters[16].Value = pEntity.ID;

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
        public void Update(TestDriveOrderEntity pEntity)
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(TestDriveOrderEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(TestDriveOrderEntity pEntity, IDbTransaction pTran)
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
                return;
            //��֯������SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [TestDriveOrder] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@LastUpdateTime",SqlDbType=SqlDbType.DateTime,Value=DateTime.Now},
                new SqlParameter{ParameterName="@LastUpdateBy",SqlDbType=SqlDbType.Int,Value=Convert.ToInt32(CurrentUserInfo.UserID)},
                new SqlParameter{ParameterName="@ID",SqlDbType=SqlDbType.VarChar,Value=pID}
            };
            //ִ�����
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, sql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters);
            return;
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(TestDriveOrderEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(TestDriveOrderEntity[] pEntities)
        {
            Delete(pEntities, null);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        public void Delete(object[] pIDs)
        {
            Delete(pIDs, null);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(object[] pIDs, IDbTransaction pTran)
        {
            if (pIDs == null || pIDs.Length == 0)
                return;
            //��֯������SQL
            StringBuilder primaryKeys = new StringBuilder();
            foreach (object item in pIDs)
            {
                primaryKeys.AppendFormat("{0},", item.ToString());
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [TestDriveOrder] set LastUpdateTime='" + DateTime.Now.ToString() + "',LastUpdateBy=" + CurrentUserInfo.UserID + ",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
            //ִ�����
            int result = 0;
            if (pTran == null)
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), null);
            else
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, sql.ToString());
        }
        #endregion

        #region IQueryable ��Ա
        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <param name="pWhereConditions">ɸѡ����</param>
        /// <param name="pOrderBys">����</param>
        /// <returns></returns>
        public TestDriveOrderEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [TestDriveOrder] where isdelete=0 ");
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
            List<TestDriveOrderEntity> list = new List<TestDriveOrderEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    TestDriveOrderEntity m;
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
        public PagedQueryResult<TestDriveOrderEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                    if (item != null)
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
            pagedSql.AppendFormat(") as ___rn,* from [TestDriveOrder] where isdelete=0 ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [TestDriveOrder] where isdelete=0 ");
            //��������
            if (pWhereConditions != null)
            {
                foreach (var item in pWhereConditions)
                {
                    if (item != null)
                    {
                        pagedSql.AppendFormat(" and {0}", item.GetExpression());
                        totalCountSql.AppendFormat(" and {0}", item.GetExpression());
                    }
                }
            }
            pagedSql.AppendFormat(") as A ");
            //ȡָ��ҳ������
            pagedSql.AppendFormat(" where ___rn >{0} and ___rn <={1}", pPageSize * (pCurrentPageIndex - 1), pPageSize * (pCurrentPageIndex));
            //ִ����䲢���ؽ��
            PagedQueryResult<TestDriveOrderEntity> result = new PagedQueryResult<TestDriveOrderEntity>();
            List<TestDriveOrderEntity> list = new List<TestDriveOrderEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    TestDriveOrderEntity m;
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
        public TestDriveOrderEntity[] QueryByEntity(TestDriveOrderEntity pQueryEntity, OrderBy[] pOrderBys)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity(pQueryEntity);
            return Query(queryWhereCondition, pOrderBys);
        }

        /// <summary>
        /// ��ҳ����ʵ��������ѯʵ��
        /// </summary>
        /// <param name="pQueryEntity">��ʵ����ʽ����Ĳ���</param>
        /// <param name="pOrderBys">�������</param>
        /// <returns>����������ʵ�弯</returns>
        public PagedQueryResult<TestDriveOrderEntity> PagedQueryByEntity(TestDriveOrderEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity(pQueryEntity);
            return PagedQuery(queryWhereCondition, pOrderBys, pPageSize, pCurrentPageIndex);
        }

        #endregion

        #region ���߷���
        /// <summary>
        /// ����ʵ���Null�������ɲ�ѯ������
        /// </summary>
        /// <returns></returns>
        protected IWhereCondition[] GetWhereConditionByEntity(TestDriveOrderEntity pQueryEntity)
        {
            //��ȡ�ǿ���������
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.CarTypeID != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CarTypeID", Value = pQueryEntity.CarTypeID });
            if (pQueryEntity.CustomerName != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerName", Value = pQueryEntity.CustomerName });
            if (pQueryEntity.Gender != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Gender", Value = pQueryEntity.Gender });
            if (pQueryEntity.Phone != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Phone", Value = pQueryEntity.Phone });
            if (pQueryEntity.PlanBuyTime != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PlanBuyTime", Value = pQueryEntity.PlanBuyTime });
            if (pQueryEntity.CustomerRemark != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerRemark", Value = pQueryEntity.CustomerRemark });
            if (pQueryEntity.SubmitTime != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SubmitTime", Value = pQueryEntity.SubmitTime });
            if (pQueryEntity.TargetTime != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "TargetTime", Value = pQueryEntity.TargetTime });
            if (pQueryEntity.IsHandle != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsHandle", Value = pQueryEntity.IsHandle });
            if (pQueryEntity.IsSuccess != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsSuccess", Value = pQueryEntity.IsSuccess });
            if (pQueryEntity.Remark != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
            if (pQueryEntity.ClientID != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientID", Value = pQueryEntity.ClientID });
            if (pQueryEntity.WxId != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxId", Value = pQueryEntity.WxId });
            if (pQueryEntity.WxOpenId != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "WxOpenId", Value = pQueryEntity.WxOpenId });
            if (pQueryEntity.CreateBy != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
            if (pQueryEntity.CreateTime != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.LastUpdateBy != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateBy", Value = pQueryEntity.LastUpdateBy });
            if (pQueryEntity.LastUpdateTime != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateTime", Value = pQueryEntity.LastUpdateTime });
            if (pQueryEntity.IsDelete != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// װ��ʵ��
        /// </summary>
        /// <param name="pReader">��ǰֻ����</param>
        /// <param name="pInstance">ʵ��ʵ��</param>
        protected void Load(SqlDataReader pReader, out TestDriveOrderEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
            pInstance = new TestDriveOrderEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

            if (pReader["ID"] != DBNull.Value)
            {
                pInstance.ID = Convert.ToInt32(pReader["ID"]);
            }
            if (pReader["CarTypeID"] != DBNull.Value)
            {
                pInstance.CarTypeID = Convert.ToInt32(pReader["CarTypeID"]);
            }
            if (pReader["CustomerName"] != DBNull.Value)
            {
                pInstance.CustomerName = Convert.ToString(pReader["CustomerName"]);
            }
            if (pReader["Gender"] != DBNull.Value)
            {
                pInstance.Gender = Convert.ToString(pReader["Gender"]);
            }
            if (pReader["Phone"] != DBNull.Value)
            {
                pInstance.Phone = Convert.ToString(pReader["Phone"]);
            }
            if (pReader["PlanBuyTime"] != DBNull.Value)
            {
                pInstance.PlanBuyTime = Convert.ToString(pReader["PlanBuyTime"]);
            }
            if (pReader["CustomerRemark"] != DBNull.Value)
            {
                pInstance.CustomerRemark = Convert.ToString(pReader["CustomerRemark"]);
            }
            if (pReader["SubmitTime"] != DBNull.Value)
            {
                pInstance.SubmitTime = Convert.ToDateTime(pReader["SubmitTime"]);
            }
            if (pReader["TargetTime"] != DBNull.Value)
            {
                pInstance.TargetTime = Convert.ToDateTime(pReader["TargetTime"]);
            }
            if (pReader["IsHandle"] != DBNull.Value)
            {
                pInstance.IsHandle = Convert.ToString(pReader["IsHandle"]);
            }
            if (pReader["IsSuccess"] != DBNull.Value)
            {
                pInstance.IsSuccess = Convert.ToString(pReader["IsSuccess"]);
            }
            if (pReader["Remark"] != DBNull.Value)
            {
                pInstance.Remark = Convert.ToString(pReader["Remark"]);
            }
            if (pReader["ClientID"] != DBNull.Value)
            {
                pInstance.ClientID = Convert.ToInt32(pReader["ClientID"]);
            }
            if (pReader["WxId"] != DBNull.Value)
            {
                pInstance.WxId = Convert.ToString(pReader["WxId"]);
            }
            if (pReader["WxOpenId"] != DBNull.Value)
            {
                pInstance.WxOpenId = Convert.ToString(pReader["WxOpenId"]);
            }
            if (pReader["CreateBy"] != DBNull.Value)
            {
                pInstance.CreateBy = Convert.ToInt32(pReader["CreateBy"]);
            }
            if (pReader["CreateTime"] != DBNull.Value)
            {
                pInstance.CreateTime = Convert.ToDateTime(pReader["CreateTime"]);
            }
            if (pReader["LastUpdateBy"] != DBNull.Value)
            {
                pInstance.LastUpdateBy = Convert.ToInt32(pReader["LastUpdateBy"]);
            }
            if (pReader["LastUpdateTime"] != DBNull.Value)
            {
                pInstance.LastUpdateTime = Convert.ToDateTime(pReader["LastUpdateTime"]);
            }
            if (pReader["IsDelete"] != DBNull.Value)
            {
                pInstance.IsDelete = Convert.ToInt32(pReader["IsDelete"]);
            }

        }
        #endregion


    }
}
