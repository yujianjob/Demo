/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/23 18:09:08
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
    /// ���ݷ��ʣ� 0318����յ��� CarStyleAirConditioning 
    /// ��CarStyleAirConditioning�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CarStyleAirConditioningDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarStyleAirConditioningEntity>, IQueryable<CarStyleAirConditioningEntity>
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public CarStyleAirConditioningDAO(BasicUserInfo pUserInfo)
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
        public void Create(CarStyleAirConditioningEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(CarStyleAirConditioningEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [CarStyleAirConditioning](");
            strSql.Append("[CarStyleID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete],[Column1],[Column2],[Column3],[Column4],[Column5],[Column6],[Column7],[Column8],[Column9],[Column10])");
            strSql.Append(" values (");
            strSql.Append("@CarStyleID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete,@Column1,@Column2,@Column3,@Column4,@Column5,@Column6,@Column7,@Column8,@Column9,@Column10)");            
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
					new SqlParameter("@Column10",SqlDbType.NVarChar)
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
        public CarStyleAirConditioningEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleAirConditioning] where ID='{0}' and IsDelete=0 ", id.ToString());
            //��ȡ����
            CarStyleAirConditioningEntity m = null;
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
        public CarStyleAirConditioningEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleAirConditioning] where isdelete=0");
            //��ȡ����
            List<CarStyleAirConditioningEntity> list = new List<CarStyleAirConditioningEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleAirConditioningEntity m;
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
        public void Update(CarStyleAirConditioningEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(CarStyleAirConditioningEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [CarStyleAirConditioning] set ");
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
                strSql.Append( "[Column10]=@Column10");
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
			parameters[13].Value = pEntity.ID;

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
        public void Update(CarStyleAirConditioningEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(CarStyleAirConditioningEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(CarStyleAirConditioningEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [CarStyleAirConditioning] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(CarStyleAirConditioningEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(CarStyleAirConditioningEntity[] pEntities)
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
            sql.AppendLine("update [CarStyleAirConditioning] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public CarStyleAirConditioningEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleAirConditioning] where isdelete=0 ");
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
            List<CarStyleAirConditioningEntity> list = new List<CarStyleAirConditioningEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleAirConditioningEntity m;
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
        public PagedQueryResult<CarStyleAirConditioningEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [CarStyleAirConditioning] where isdelete=0 ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [CarStyleAirConditioning] where isdelete=0 ");
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
            PagedQueryResult<CarStyleAirConditioningEntity> result = new PagedQueryResult<CarStyleAirConditioningEntity>();
            List<CarStyleAirConditioningEntity> list = new List<CarStyleAirConditioningEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleAirConditioningEntity m;
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
        public CarStyleAirConditioningEntity[] QueryByEntity(CarStyleAirConditioningEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<CarStyleAirConditioningEntity> PagedQueryByEntity(CarStyleAirConditioningEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(CarStyleAirConditioningEntity pQueryEntity)
        { 
            //��ȡ�ǿ���������
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

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// װ��ʵ��
        /// </summary>
        /// <param name="pReader">��ǰֻ����</param>
        /// <param name="pInstance">ʵ��ʵ��</param>
        protected void Load(SqlDataReader pReader, out CarStyleAirConditioningEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
            pInstance = new CarStyleAirConditioningEntity();
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

        }
        #endregion
    }
}
