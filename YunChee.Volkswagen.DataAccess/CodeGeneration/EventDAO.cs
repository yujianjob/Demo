/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/7 15:38:20
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
    /// ���ݷ��ʣ� 0201��� Event 
    /// ��Event�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class EventDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventEntity>, IQueryable<EventEntity>
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public EventDAO(BasicUserInfo pUserInfo)
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
        public void Create(EventEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(EventEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [Event](");
            strSql.Append("[ClientType],[RegionFirstTypeID],[RegionSecondTypeID],[DealerFirstTypeID],[DealerSecondTypeID],[DealerLastTypeID],[Template],[Title],[Description],[Content],[PictureUrl],[StartTime],[EndTime],[SortIndex],[IsShow],[IsTop],[VideoUrl],[OriginalUrl],[Remark],[ClientID],[ContentScenario],[ContentLibraryID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@ClientType,@RegionFirstTypeID,@RegionSecondTypeID,@DealerFirstTypeID,@DealerSecondTypeID,@DealerLastTypeID,@Template,@Title,@Description,@Content,@PictureUrl,@StartTime,@EndTime,@SortIndex,@IsShow,@IsTop,@VideoUrl,@OriginalUrl,@Remark,@ClientID,@ContentScenario,@ContentLibraryID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");            
			strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@ClientType",SqlDbType.NVarChar),
					new SqlParameter("@RegionFirstTypeID",SqlDbType.Int),
					new SqlParameter("@RegionSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerFirstTypeID",SqlDbType.Int),
					new SqlParameter("@DealerSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerLastTypeID",SqlDbType.Int),
					new SqlParameter("@Template",SqlDbType.NVarChar),
					new SqlParameter("@Title",SqlDbType.NVarChar),
					new SqlParameter("@Description",SqlDbType.NVarChar),
					new SqlParameter("@Content",SqlDbType.NVarChar),
					new SqlParameter("@PictureUrl",SqlDbType.NVarChar),
					new SqlParameter("@StartTime",SqlDbType.DateTime),
					new SqlParameter("@EndTime",SqlDbType.DateTime),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@IsShow",SqlDbType.NVarChar),
					new SqlParameter("@IsTop",SqlDbType.NVarChar),
					new SqlParameter("@VideoUrl",SqlDbType.NVarChar),
					new SqlParameter("@OriginalUrl",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@ContentScenario",SqlDbType.NVarChar),
					new SqlParameter("@ContentLibraryID",SqlDbType.Int),
					new SqlParameter("@CreateBy",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.ClientType;
			parameters[1].Value = pEntity.RegionFirstTypeID;
			parameters[2].Value = pEntity.RegionSecondTypeID;
			parameters[3].Value = pEntity.DealerFirstTypeID;
			parameters[4].Value = pEntity.DealerSecondTypeID;
			parameters[5].Value = pEntity.DealerLastTypeID;
			parameters[6].Value = pEntity.Template;
			parameters[7].Value = pEntity.Title;
			parameters[8].Value = pEntity.Description;
			parameters[9].Value = pEntity.Content;
			parameters[10].Value = pEntity.PictureUrl;
			parameters[11].Value = pEntity.StartTime;
			parameters[12].Value = pEntity.EndTime;
			parameters[13].Value = pEntity.SortIndex;
			parameters[14].Value = pEntity.IsShow;
			parameters[15].Value = pEntity.IsTop;
			parameters[16].Value = pEntity.VideoUrl;
			parameters[17].Value = pEntity.OriginalUrl;
			parameters[18].Value = pEntity.Remark;
			parameters[19].Value = pEntity.ClientID;
			parameters[20].Value = pEntity.ContentScenario;
			parameters[21].Value = pEntity.ContentLibraryID;
			parameters[22].Value = pEntity.CreateBy;
			parameters[23].Value = pEntity.CreateTime;
			parameters[24].Value = pEntity.LastUpdateBy;
			parameters[25].Value = pEntity.LastUpdateTime;
			parameters[26].Value = pEntity.IsDelete;

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
        public EventEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Event] where ID='{0}' and IsDelete=0 ", id.ToString());
            //��ȡ����
            EventEntity m = null;
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
        public EventEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Event] where isdelete=0");
            //��ȡ����
            List<EventEntity> list = new List<EventEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    EventEntity m;
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
        public void Update(EventEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(EventEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
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
            strSql.Append("update [Event] set ");
                        if (pIsUpdateNullField || pEntity.ClientType!=null)
                strSql.Append( "[ClientType]=@ClientType,");
            if (pIsUpdateNullField || pEntity.RegionFirstTypeID!=null)
                strSql.Append( "[RegionFirstTypeID]=@RegionFirstTypeID,");
            if (pIsUpdateNullField || pEntity.RegionSecondTypeID!=null)
                strSql.Append( "[RegionSecondTypeID]=@RegionSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerFirstTypeID!=null)
                strSql.Append( "[DealerFirstTypeID]=@DealerFirstTypeID,");
            if (pIsUpdateNullField || pEntity.DealerSecondTypeID!=null)
                strSql.Append( "[DealerSecondTypeID]=@DealerSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerLastTypeID!=null)
                strSql.Append( "[DealerLastTypeID]=@DealerLastTypeID,");
            if (pIsUpdateNullField || pEntity.Template!=null)
                strSql.Append( "[Template]=@Template,");
            if (pIsUpdateNullField || pEntity.Title!=null)
                strSql.Append( "[Title]=@Title,");
            if (pIsUpdateNullField || pEntity.Description!=null)
                strSql.Append( "[Description]=@Description,");
            if (pIsUpdateNullField || pEntity.Content!=null)
                strSql.Append( "[Content]=@Content,");
            if (pIsUpdateNullField || pEntity.PictureUrl!=null)
                strSql.Append( "[PictureUrl]=@PictureUrl,");
            if (pIsUpdateNullField || pEntity.StartTime!=null)
                strSql.Append( "[StartTime]=@StartTime,");
            if (pIsUpdateNullField || pEntity.EndTime!=null)
                strSql.Append( "[EndTime]=@EndTime,");
            if (pIsUpdateNullField || pEntity.SortIndex!=null)
                strSql.Append( "[SortIndex]=@SortIndex,");
            if (pIsUpdateNullField || pEntity.IsShow!=null)
                strSql.Append( "[IsShow]=@IsShow,");
            if (pIsUpdateNullField || pEntity.IsTop!=null)
                strSql.Append( "[IsTop]=@IsTop,");
            if (pIsUpdateNullField || pEntity.VideoUrl!=null)
                strSql.Append( "[VideoUrl]=@VideoUrl,");
            if (pIsUpdateNullField || pEntity.OriginalUrl!=null)
                strSql.Append( "[OriginalUrl]=@OriginalUrl,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ClientID!=null)
                strSql.Append( "[ClientID]=@ClientID,");
            if (pIsUpdateNullField || pEntity.ContentScenario!=null)
                strSql.Append( "[ContentScenario]=@ContentScenario,");
            if (pIsUpdateNullField || pEntity.ContentLibraryID!=null)
                strSql.Append( "[ContentLibraryID]=@ContentLibraryID,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@ClientType",SqlDbType.NVarChar),
					new SqlParameter("@RegionFirstTypeID",SqlDbType.Int),
					new SqlParameter("@RegionSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerFirstTypeID",SqlDbType.Int),
					new SqlParameter("@DealerSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerLastTypeID",SqlDbType.Int),
					new SqlParameter("@Template",SqlDbType.NVarChar),
					new SqlParameter("@Title",SqlDbType.NVarChar),
					new SqlParameter("@Description",SqlDbType.NVarChar),
					new SqlParameter("@Content",SqlDbType.NVarChar),
					new SqlParameter("@PictureUrl",SqlDbType.NVarChar),
					new SqlParameter("@StartTime",SqlDbType.DateTime),
					new SqlParameter("@EndTime",SqlDbType.DateTime),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@IsShow",SqlDbType.NVarChar),
					new SqlParameter("@IsTop",SqlDbType.NVarChar),
					new SqlParameter("@VideoUrl",SqlDbType.NVarChar),
					new SqlParameter("@OriginalUrl",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.Int),
					new SqlParameter("@ContentScenario",SqlDbType.NVarChar),
					new SqlParameter("@ContentLibraryID",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
            };
			parameters[0].Value = pEntity.ClientType;
			parameters[1].Value = pEntity.RegionFirstTypeID;
			parameters[2].Value = pEntity.RegionSecondTypeID;
			parameters[3].Value = pEntity.DealerFirstTypeID;
			parameters[4].Value = pEntity.DealerSecondTypeID;
			parameters[5].Value = pEntity.DealerLastTypeID;
			parameters[6].Value = pEntity.Template;
			parameters[7].Value = pEntity.Title;
			parameters[8].Value = pEntity.Description;
			parameters[9].Value = pEntity.Content;
			parameters[10].Value = pEntity.PictureUrl;
			parameters[11].Value = pEntity.StartTime;
			parameters[12].Value = pEntity.EndTime;
			parameters[13].Value = pEntity.SortIndex;
			parameters[14].Value = pEntity.IsShow;
			parameters[15].Value = pEntity.IsTop;
			parameters[16].Value = pEntity.VideoUrl;
			parameters[17].Value = pEntity.OriginalUrl;
			parameters[18].Value = pEntity.Remark;
			parameters[19].Value = pEntity.ClientID;
			parameters[20].Value = pEntity.ContentScenario;
			parameters[21].Value = pEntity.ContentLibraryID;
			parameters[22].Value = pEntity.LastUpdateBy;
			parameters[23].Value = pEntity.LastUpdateTime;
			parameters[24].Value = pEntity.ID;

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
        public void Update(EventEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(EventEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(EventEntity pEntity, IDbTransaction pTran)
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
            sql.AppendLine("update [Event] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where ID=@ID;");
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
        public void Delete(EventEntity[] pEntities, IDbTransaction pTran)
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
        public void Delete(EventEntity[] pEntities)
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
            sql.AppendLine("update [Event] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy="+CurrentUserInfo.UserID+",IsDelete=1 where ID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public EventEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [Event] where isdelete=0 ");
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
            List<EventEntity> list = new List<EventEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    EventEntity m;
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
        public PagedQueryResult<EventEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
            pagedSql.AppendFormat(") as ___rn,* from [Event] where isdelete=0 ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [Event] where isdelete=0 ");
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
            PagedQueryResult<EventEntity> result = new PagedQueryResult<EventEntity>();
            List<EventEntity> list = new List<EventEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    EventEntity m;
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
        public EventEntity[] QueryByEntity(EventEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<EventEntity> PagedQueryByEntity(EventEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(EventEntity pQueryEntity)
        { 
            //��ȡ�ǿ���������
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ID", Value = pQueryEntity.ID });
            if (pQueryEntity.ClientType!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientType", Value = pQueryEntity.ClientType });
            if (pQueryEntity.RegionFirstTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "RegionFirstTypeID", Value = pQueryEntity.RegionFirstTypeID });
            if (pQueryEntity.RegionSecondTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "RegionSecondTypeID", Value = pQueryEntity.RegionSecondTypeID });
            if (pQueryEntity.DealerFirstTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DealerFirstTypeID", Value = pQueryEntity.DealerFirstTypeID });
            if (pQueryEntity.DealerSecondTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DealerSecondTypeID", Value = pQueryEntity.DealerSecondTypeID });
            if (pQueryEntity.DealerLastTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DealerLastTypeID", Value = pQueryEntity.DealerLastTypeID });
            if (pQueryEntity.Template!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Template", Value = pQueryEntity.Template });
            if (pQueryEntity.Title!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Title", Value = pQueryEntity.Title });
            if (pQueryEntity.Description!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Description", Value = pQueryEntity.Description });
            if (pQueryEntity.Content!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Content", Value = pQueryEntity.Content });
            if (pQueryEntity.PictureUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PictureUrl", Value = pQueryEntity.PictureUrl });
            if (pQueryEntity.StartTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "StartTime", Value = pQueryEntity.StartTime });
            if (pQueryEntity.EndTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EndTime", Value = pQueryEntity.EndTime });
            if (pQueryEntity.SortIndex!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SortIndex", Value = pQueryEntity.SortIndex });
            if (pQueryEntity.IsShow!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsShow", Value = pQueryEntity.IsShow });
            if (pQueryEntity.IsTop!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsTop", Value = pQueryEntity.IsTop });
            if (pQueryEntity.VideoUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VideoUrl", Value = pQueryEntity.VideoUrl });
            if (pQueryEntity.OriginalUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "OriginalUrl", Value = pQueryEntity.OriginalUrl });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
            if (pQueryEntity.ClientID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientID", Value = pQueryEntity.ClientID });
            if (pQueryEntity.ContentScenario!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ContentScenario", Value = pQueryEntity.ContentScenario });
            if (pQueryEntity.ContentLibraryID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ContentLibraryID", Value = pQueryEntity.ContentLibraryID });
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
        protected void Load(SqlDataReader pReader, out EventEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
            pInstance = new EventEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ID"] != DBNull.Value)
			{
				pInstance.ID =   Convert.ToInt32(pReader["ID"]);
			}
			if (pReader["ClientType"] != DBNull.Value)
			{
				pInstance.ClientType =  Convert.ToString(pReader["ClientType"]);
			}
			if (pReader["RegionFirstTypeID"] != DBNull.Value)
			{
				pInstance.RegionFirstTypeID =   Convert.ToInt32(pReader["RegionFirstTypeID"]);
			}
			if (pReader["RegionSecondTypeID"] != DBNull.Value)
			{
				pInstance.RegionSecondTypeID =   Convert.ToInt32(pReader["RegionSecondTypeID"]);
			}
			if (pReader["DealerFirstTypeID"] != DBNull.Value)
			{
				pInstance.DealerFirstTypeID =   Convert.ToInt32(pReader["DealerFirstTypeID"]);
			}
			if (pReader["DealerSecondTypeID"] != DBNull.Value)
			{
				pInstance.DealerSecondTypeID =   Convert.ToInt32(pReader["DealerSecondTypeID"]);
			}
			if (pReader["DealerLastTypeID"] != DBNull.Value)
			{
				pInstance.DealerLastTypeID =   Convert.ToInt32(pReader["DealerLastTypeID"]);
			}
			if (pReader["Template"] != DBNull.Value)
			{
				pInstance.Template =  Convert.ToString(pReader["Template"]);
			}
			if (pReader["Title"] != DBNull.Value)
			{
				pInstance.Title =  Convert.ToString(pReader["Title"]);
			}
			if (pReader["Description"] != DBNull.Value)
			{
				pInstance.Description =  Convert.ToString(pReader["Description"]);
			}
			if (pReader["Content"] != DBNull.Value)
			{
				pInstance.Content =  Convert.ToString(pReader["Content"]);
			}
			if (pReader["PictureUrl"] != DBNull.Value)
			{
				pInstance.PictureUrl =  Convert.ToString(pReader["PictureUrl"]);
			}
			if (pReader["StartTime"] != DBNull.Value)
			{
				pInstance.StartTime =  Convert.ToDateTime(pReader["StartTime"]);
			}
			if (pReader["EndTime"] != DBNull.Value)
			{
				pInstance.EndTime =  Convert.ToDateTime(pReader["EndTime"]);
			}
			if (pReader["SortIndex"] != DBNull.Value)
			{
				pInstance.SortIndex =   Convert.ToInt32(pReader["SortIndex"]);
			}
			if (pReader["IsShow"] != DBNull.Value)
			{
				pInstance.IsShow =  Convert.ToString(pReader["IsShow"]);
			}
			if (pReader["IsTop"] != DBNull.Value)
			{
				pInstance.IsTop =  Convert.ToString(pReader["IsTop"]);
			}
			if (pReader["VideoUrl"] != DBNull.Value)
			{
				pInstance.VideoUrl =  Convert.ToString(pReader["VideoUrl"]);
			}
			if (pReader["OriginalUrl"] != DBNull.Value)
			{
				pInstance.OriginalUrl =  Convert.ToString(pReader["OriginalUrl"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
			}
			if (pReader["ClientID"] != DBNull.Value)
			{
				pInstance.ClientID =   Convert.ToInt32(pReader["ClientID"]);
			}
			if (pReader["ContentScenario"] != DBNull.Value)
			{
				pInstance.ContentScenario =  Convert.ToString(pReader["ContentScenario"]);
			}
			if (pReader["ContentLibraryID"] != DBNull.Value)
			{
				pInstance.ContentLibraryID =   Convert.ToInt32(pReader["ContentLibraryID"]);
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
