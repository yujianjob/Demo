/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/24 15:48:54
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
    /// ���ݷ��ʣ� 1007΢�Ų˵��� WMenu 
    /// ��WMenu�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class WMenuDAO : BaseDAO<BasicUserInfo>, ICRUDable<WMenuEntity>, IQueryable<WMenuEntity>
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public WMenuDAO(BasicUserInfo pUserInfo)
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
        public void Create(WMenuEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(WMenuEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [WMenu](");
            strSql.Append("[ParentID],[ReplyTypeID],[Text],[MenuUrl],[Name],[Key],[Type],[Level],[SortIndex],[ApplicationID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@ParentID,@ReplyTypeID,@Text,@MenuUrl,@Name,@Key,@Type,@Level,@SortIndex,@ApplicationID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@ParentID",SqlDbType.Int),
					new SqlParameter("@ReplyTypeID",SqlDbType.NVarChar),
					new SqlParameter("@Text",SqlDbType.NVarChar),
					new SqlParameter("@MenuUrl",SqlDbType.NVarChar),
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@Key",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@Level",SqlDbType.NVarChar),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@ApplicationID",SqlDbType.Int),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.ParentID;
			parameters[1].Value = pEntity.ReplyTypeID;
			parameters[2].Value = pEntity.Text;
			parameters[3].Value = pEntity.MenuUrl;
			parameters[4].Value = pEntity.Name;
			parameters[5].Value = pEntity.Key;
			parameters[6].Value = pEntity.Type;
			parameters[7].Value = pEntity.Level;
			parameters[8].Value = pEntity.SortIndex;
			parameters[9].Value = pEntity.ApplicationID;
			parameters[10].Value = pEntity.CreateBy;
			parameters[11].Value = pEntity.CreateTime;
			parameters[12].Value = pEntity.LastUpdateBy;
			parameters[13].Value = pEntity.LastUpdateTime;
			parameters[14].Value = pEntity.IsDelete;

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
        public WMenuEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [WMenu] where ID='{0}' and IsDelete=0 ", id.ToString());
            //��ȡ����
            WMenuEntity m = null;
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
        public WMenuEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [WMenu] where isdelete=0");
            //��ȡ����
            List<WMenuEntity> list = new List<WMenuEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    WMenuEntity m;
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
        public void Update(WMenuEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(WMenuEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [WMenu] set ");
                        if (pIsUpdateNullField || pEntity.ParentID!=null)
                strSql.Append( "[ParentID]=@ParentID,");
            if (pIsUpdateNullField || pEntity.ReplyTypeID!=null)
                strSql.Append( "[ReplyTypeID]=@ReplyTypeID,");
            if (pIsUpdateNullField || pEntity.Text!=null)
                strSql.Append( "[Text]=@Text,");
            if (pIsUpdateNullField || pEntity.MenuUrl!=null)
                strSql.Append( "[MenuUrl]=@MenuUrl,");
            if (pIsUpdateNullField || pEntity.Name!=null)
                strSql.Append( "[Name]=@Name,");
            if (pIsUpdateNullField || pEntity.Key!=null)
                strSql.Append( "[Key]=@Key,");
            if (pIsUpdateNullField || pEntity.Type!=null)
                strSql.Append( "[Type]=@Type,");
            if (pIsUpdateNullField || pEntity.Level!=null)
                strSql.Append( "[Level]=@Level,");
            if (pIsUpdateNullField || pEntity.SortIndex!=null)
                strSql.Append( "[SortIndex]=@SortIndex,");
            if (pIsUpdateNullField || pEntity.ApplicationID!=null)
                strSql.Append( "[ApplicationID]=@ApplicationID,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@ParentID",SqlDbType.Int),
					new SqlParameter("@ReplyTypeID",SqlDbType.NVarChar),
					new SqlParameter("@Text",SqlDbType.NVarChar),
					new SqlParameter("@MenuUrl",SqlDbType.NVarChar),
					new SqlParameter("@Name",SqlDbType.NVarChar),
					new SqlParameter("@Key",SqlDbType.NVarChar),
					new SqlParameter("@Type",SqlDbType.NVarChar),
					new SqlParameter("@Level",SqlDbType.NVarChar),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@ApplicationID",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.ParentID;
			parameters[1].Value = pEntity.ReplyTypeID;
			parameters[2].Value = pEntity.Text;
			parameters[3].Value = pEntity.MenuUrl;
			parameters[4].Value = pEntity.Name;
			parameters[5].Value = pEntity.Key;
			parameters[6].Value = pEntity.Type;
			parameters[7].Value = pEntity.Level;
			parameters[8].Value = pEntity.SortIndex;
			parameters[9].Value = pEntity.ApplicationID;
			parameters[10].Value = pEntity.LastUpdateBy;
			parameters[11].Value = pEntity.LastUpdateTime;
			parameters[12].Value = pEntity.ID;

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
        public void Update(WMenuEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(WMenuEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(WMenuEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [WMenu] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(WMenuEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(WMenuEntity[] pEntities)
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
            sql.AppendLine("update [WMenu] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public WMenuEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [WMenu] where isdelete=0 ");
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
            List<WMenuEntity> list = new List<WMenuEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    WMenuEntity m;
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
        public PagedQueryResult<WMenuEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [WMenu] where isdelete=0 ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [WMenu] where isdelete=0 ");
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
            PagedQueryResult<WMenuEntity> result = new PagedQueryResult<WMenuEntity>();
            List<WMenuEntity> list = new List<WMenuEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    WMenuEntity m;
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
        public WMenuEntity[] QueryByEntity(WMenuEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<WMenuEntity> PagedQueryByEntity(WMenuEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(WMenuEntity pQueryEntity)
        { 
            //��ȡ�ǿ���������
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.ParentID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ParentID", Value = pQueryEntity.ParentID });
            if (pQueryEntity.ReplyTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ReplyTypeID", Value = pQueryEntity.ReplyTypeID });
            if (pQueryEntity.Text!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Text", Value = pQueryEntity.Text });
            if (pQueryEntity.MenuUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "MenuUrl", Value = pQueryEntity.MenuUrl });
            if (pQueryEntity.Name!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Name", Value = pQueryEntity.Name });
            if (pQueryEntity.Key!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Key", Value = pQueryEntity.Key });
            if (pQueryEntity.Type!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Type", Value = pQueryEntity.Type });
            if (pQueryEntity.Level!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Level", Value = pQueryEntity.Level });
            if (pQueryEntity.SortIndex!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SortIndex", Value = pQueryEntity.SortIndex });
            if (pQueryEntity.ApplicationID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ApplicationID", Value = pQueryEntity.ApplicationID });
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
        protected void Load(SqlDataReader pReader, out WMenuEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
            pInstance = new WMenuEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ID"] != DBNull.Value)
			{
				pInstance.ID =   Convert.ToInt32(pReader["ID"]);
			}
			if (pReader["ParentID"] != DBNull.Value)
			{
				pInstance.ParentID =   Convert.ToInt32(pReader["ParentID"]);
			}
			if (pReader["ReplyTypeID"] != DBNull.Value)
			{
				pInstance.ReplyTypeID =  Convert.ToString(pReader["ReplyTypeID"]);
			}
			if (pReader["Text"] != DBNull.Value)
			{
				pInstance.Text =  Convert.ToString(pReader["Text"]);
			}
			if (pReader["MenuUrl"] != DBNull.Value)
			{
				pInstance.MenuUrl =  Convert.ToString(pReader["MenuUrl"]);
			}
			if (pReader["Name"] != DBNull.Value)
			{
				pInstance.Name =  Convert.ToString(pReader["Name"]);
			}
			if (pReader["Key"] != DBNull.Value)
			{
				pInstance.Key =  Convert.ToString(pReader["Key"]);
			}
			if (pReader["Type"] != DBNull.Value)
			{
				pInstance.Type =  Convert.ToString(pReader["Type"]);
			}
			if (pReader["Level"] != DBNull.Value)
			{
				pInstance.Level =  Convert.ToString(pReader["Level"]);
			}
			if (pReader["SortIndex"] != DBNull.Value)
			{
				pInstance.SortIndex =   Convert.ToInt32(pReader["SortIndex"]);
			}
			if (pReader["ApplicationID"] != DBNull.Value)
			{
				pInstance.ApplicationID =   Convert.ToInt32(pReader["ApplicationID"]);
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
