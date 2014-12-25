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
    /// ���ݷ��ʣ� 0312�����г������� CarStyleDrivingAuxiliary 
    /// ��CarStyleDrivingAuxiliary�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CarStyleDrivingAuxiliaryDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarStyleDrivingAuxiliaryEntity>, IQueryable<CarStyleDrivingAuxiliaryEntity>
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public CarStyleDrivingAuxiliaryDAO(BasicUserInfo pUserInfo)
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
        public void Create(CarStyleDrivingAuxiliaryEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(CarStyleDrivingAuxiliaryEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [CarStyleDrivingAuxiliary](");
            strSql.Append("[CarStyleID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete],[Column1],[Column2],[Column3],[Column4],[Column5],[Column6],[Column7],[Column8],[Column9],[Column10],[Column11],[Column12],[Column13],[Column14],[Column15],[Column16],[Column17],[Column18],[Column19],[Column20],[Column21],[Column22],[Column23],[Column24],[Column25])");
            strSql.Append(" values (");
            strSql.Append("@CarStyleID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete,@Column1,@Column2,@Column3,@Column4,@Column5,@Column6,@Column7,@Column8,@Column9,@Column10,@Column11,@Column12,@Column13,@Column14,@Column15,@Column16,@Column17,@Column18,@Column19,@Column20,@Column21,@Column22,@Column23,@Column24,@Column25)");            
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
					new SqlParameter("@Column20",SqlDbType.NVarChar),
					new SqlParameter("@Column21",SqlDbType.NVarChar),
					new SqlParameter("@Column22",SqlDbType.NVarChar),
					new SqlParameter("@Column23",SqlDbType.NVarChar),
					new SqlParameter("@Column24",SqlDbType.NVarChar),
					new SqlParameter("@Column25",SqlDbType.NVarChar)
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
			parameters[26].Value = pEntity.Column21;
			parameters[27].Value = pEntity.Column22;
			parameters[28].Value = pEntity.Column23;
			parameters[29].Value = pEntity.Column24;
			parameters[30].Value = pEntity.Column25;

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
        public CarStyleDrivingAuxiliaryEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleDrivingAuxiliary] where ID='{0}' and IsDelete=0 ", id.ToString());
            //��ȡ����
            CarStyleDrivingAuxiliaryEntity m = null;
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
        public CarStyleDrivingAuxiliaryEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleDrivingAuxiliary] where isdelete=0");
            //��ȡ����
            List<CarStyleDrivingAuxiliaryEntity> list = new List<CarStyleDrivingAuxiliaryEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleDrivingAuxiliaryEntity m;
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
        public void Update(CarStyleDrivingAuxiliaryEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(CarStyleDrivingAuxiliaryEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [CarStyleDrivingAuxiliary] set ");
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
                strSql.Append( "[Column20]=@Column20,");
            if (pIsUpdateNullField || pEntity.Column21!=null)
                strSql.Append( "[Column21]=@Column21,");
            if (pIsUpdateNullField || pEntity.Column22!=null)
                strSql.Append( "[Column22]=@Column22,");
            if (pIsUpdateNullField || pEntity.Column23!=null)
                strSql.Append( "[Column23]=@Column23,");
            if (pIsUpdateNullField || pEntity.Column24!=null)
                strSql.Append( "[Column24]=@Column24,");
            if (pIsUpdateNullField || pEntity.Column25!=null)
                strSql.Append( "[Column25]=@Column25");
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
					new SqlParameter("@Column21",SqlDbType.NVarChar),
					new SqlParameter("@Column22",SqlDbType.NVarChar),
					new SqlParameter("@Column23",SqlDbType.NVarChar),
					new SqlParameter("@Column24",SqlDbType.NVarChar),
					new SqlParameter("@Column25",SqlDbType.NVarChar),
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
			parameters[23].Value = pEntity.Column21;
			parameters[24].Value = pEntity.Column22;
			parameters[25].Value = pEntity.Column23;
			parameters[26].Value = pEntity.Column24;
			parameters[27].Value = pEntity.Column25;
			parameters[28].Value = pEntity.ID;

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
        public void Update(CarStyleDrivingAuxiliaryEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(CarStyleDrivingAuxiliaryEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(CarStyleDrivingAuxiliaryEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [CarStyleDrivingAuxiliary] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(CarStyleDrivingAuxiliaryEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(CarStyleDrivingAuxiliaryEntity[] pEntities)
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
            sql.AppendLine("update [CarStyleDrivingAuxiliary] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public CarStyleDrivingAuxiliaryEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [CarStyleDrivingAuxiliary] where isdelete=0 ");
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
            List<CarStyleDrivingAuxiliaryEntity> list = new List<CarStyleDrivingAuxiliaryEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleDrivingAuxiliaryEntity m;
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
        public PagedQueryResult<CarStyleDrivingAuxiliaryEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [CarStyleDrivingAuxiliary] where isdelete=0 ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [CarStyleDrivingAuxiliary] where isdelete=0 ");
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
            PagedQueryResult<CarStyleDrivingAuxiliaryEntity> result = new PagedQueryResult<CarStyleDrivingAuxiliaryEntity>();
            List<CarStyleDrivingAuxiliaryEntity> list = new List<CarStyleDrivingAuxiliaryEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    CarStyleDrivingAuxiliaryEntity m;
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
        public CarStyleDrivingAuxiliaryEntity[] QueryByEntity(CarStyleDrivingAuxiliaryEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<CarStyleDrivingAuxiliaryEntity> PagedQueryByEntity(CarStyleDrivingAuxiliaryEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(CarStyleDrivingAuxiliaryEntity pQueryEntity)
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
            if (pQueryEntity.Column21!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column21", Value = pQueryEntity.Column21 });
            if (pQueryEntity.Column22!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column22", Value = pQueryEntity.Column22 });
            if (pQueryEntity.Column23!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column23", Value = pQueryEntity.Column23 });
            if (pQueryEntity.Column24!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column24", Value = pQueryEntity.Column24 });
            if (pQueryEntity.Column25!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Column25", Value = pQueryEntity.Column25 });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// װ��ʵ��
        /// </summary>
        /// <param name="pReader">��ǰֻ����</param>
        /// <param name="pInstance">ʵ��ʵ��</param>
        protected void Load(SqlDataReader pReader, out CarStyleDrivingAuxiliaryEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
            pInstance = new CarStyleDrivingAuxiliaryEntity();
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
			if (pReader["Column21"] != DBNull.Value)
			{
				pInstance.Column21 =  Convert.ToString(pReader["Column21"]);
			}
			if (pReader["Column22"] != DBNull.Value)
			{
				pInstance.Column22 =  Convert.ToString(pReader["Column22"]);
			}
			if (pReader["Column23"] != DBNull.Value)
			{
				pInstance.Column23 =  Convert.ToString(pReader["Column23"]);
			}
			if (pReader["Column24"] != DBNull.Value)
			{
				pInstance.Column24 =  Convert.ToString(pReader["Column24"]);
			}
			if (pReader["Column25"] != DBNull.Value)
			{
				pInstance.Column25 =  Convert.ToString(pReader["Column25"]);
			}

        }
        #endregion
    }
}