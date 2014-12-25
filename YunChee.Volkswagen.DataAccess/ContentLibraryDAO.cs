/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/1 0:13:02
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
    /// ���ݷ��ʣ� 0401���ݿ� ContentLibrary 
    /// ��ContentLibrary�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ContentLibraryDAO : BaseDAO<BasicUserInfo>, ICRUDable<ContentLibraryEntity>, IQueryable<ContentLibraryEntity>
    {
        #region ��ȡ��ҳȨ���б�

        /// <summary>
        /// ��ȡ��ҳȨ���б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetContentLibraryList(PagedQueryEntity entity, ContentLibraryEntity contentLibraryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ContentLibrary a";
            entity.TableName += " LEFT JOIN dbo.BasicData b ON a.ClientType = b.Value AND b.IsDelete=0  AND b.TypeCode='ClientType' ";
            entity.TableName += " LEFT JOIN dbo.BasicData c ON a.Enabled = c.Value AND c.IsDelete=0  AND c.TypeCode='YesOrNo' ";
            entity.TableName += " LEFT JOIN dbo.BasicData e ON a.ContentShowScope = e.Value AND e.IsDelete=0  AND e.TypeCode='ContentShowScope' ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , ClientTypeName = b.Name ";
            entity.QueryFieldName += " , EnabledName = c.Name ";
            entity.QueryFieldName += " , TemplateName = e.Name ";
            entity.QueryFieldName += " , RegionSecondName = (CASE a.ClientType  ";
            entity.QueryFieldName += " WHEN '1' THEN  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.RegionSecondTypeID) END  ";
            entity.QueryFieldName += " WHEN '2' THEN ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerFirstTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerSecondTypeID) END +  ";
            entity.QueryFieldName += " CASE ISNULL((SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID),'') WHEN ''  ";
            entity.QueryFieldName += " THEN '' ELSE ";
            entity.QueryFieldName += " + '-' + (SELECT t1.Name FROM dbo.ContentType t1 WHERE t1.IsDelete = 0 AND t1.ID = a.DealerLastTypeID) END  ";
            entity.QueryFieldName += " END) ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";

            entity.SortField = "a." + entity.SortField;

            if (contentLibraryEntity.ClientType != "-1")
            {
                if (contentLibraryEntity.ClientType != "0")
                {
                    entity.QueryCondition += string.Format(" AND a.ClientType = {0} ", contentLibraryEntity.ClientType);
                }
            }
            if (contentLibraryEntity.RegionFirstTypeID != 0)
            {
                if (contentLibraryEntity.RegionFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionFirstTypeID = {0} ", contentLibraryEntity.RegionFirstTypeID);
                }
            }
            if (contentLibraryEntity.RegionSecondTypeID != 0)
            {
                if (contentLibraryEntity.RegionSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.RegionSecondTypeID = {0} ", contentLibraryEntity.RegionSecondTypeID);
                }
            }
            if (contentLibraryEntity.DealerFirstTypeID != 0)
            {
                if (contentLibraryEntity.DealerFirstTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerFirstTypeID = {0} ", contentLibraryEntity.DealerFirstTypeID);
                }
            }
            if (contentLibraryEntity.DealerSecondTypeID != 0)
            {
                if (contentLibraryEntity.DealerSecondTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerSecondTypeID = {0} ", contentLibraryEntity.DealerSecondTypeID);
                }
            }
            if (contentLibraryEntity.DealerLastTypeID != 0)
            {
                if (contentLibraryEntity.DealerLastTypeID != -1)
                {
                    entity.QueryCondition += string.Format(" AND a.DealerLastTypeID = {0} ", contentLibraryEntity.DealerLastTypeID);
                }
            }
            if (contentLibraryEntity.Enabled != "-1")
            {
                entity.QueryCondition += string.Format(" AND a.Enabled = {0} ", contentLibraryEntity.Enabled);
            }
            if (contentLibraryEntity.ContentApplicationScope != "-1")
            {
                entity.QueryCondition += string.Format(" AND (a.ContentApplicationScope LIKE '%{0}%') ", contentLibraryEntity.ContentApplicationScope);
            }

            //entity.QueryCondition += string.Format("ORDER BY CreateTime ASC ");
            if (!string.IsNullOrEmpty(contentLibraryEntity.Title))
            {
                entity.QueryCondition += string.Format(" AND (a.Title LIKE '%{0}%') ", contentLibraryEntity.Title);
            }

            entity.SortField = "a.SortIndex";
            //entity.QueryCondition += string.Format(" AND a.CreateBy = {0} ", this.CurrentUserInfo.UserID);

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region ɾ�����ݿ�

        /// <summary>
        /// ɾ�����ݿ�
        /// </summary>
        /// <param name="contentLibraryId">���ݿ�ID</param>
        public void DeleteContentLibrary(string contentLibraryId)
        {
            if (!string.IsNullOrEmpty(contentLibraryId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.ContentLibrary SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", contentLibraryId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region ��ȡ���ݱ���������

        /// <summary>
        /// ��ȡ���ݱ���������
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetContentLibraryList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.ContentLibrary ";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Title LIKE '%{0}%') ", searchText);
            }
            entity.QueryCondition += string.Format(" AND ContentApplicationScope LIKE '%2%' ");
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region �������²����IDֵ

        ///<summary>
        /// ����Ϊĳ���Ự���������е�ָ�������ɵ����±�ʶֵ
        ///</summary>
        ///<returns>����</returns>
        public int GetIdentCurrent(string tableName)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT IDENT_CURRENT('{0}') ", tableName);

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region ��ȡָ�������

        /// <summary>
        /// ��ȡָ�������
        /// </summary>
        /// <param name="contentLibraryId">���ݿ�ID</param>
        /// <returns></returns>
        public DataSet GetContentLibraryApplyByContentLibraryId(int contentLibraryId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.OptionValue FROM dbo.ContentLibraryApply a ");
            sql.AppendFormat(" WHERE a.ContentLibraryID = {0} ", contentLibraryId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ���»�������Ƿ�ɾ��

        ///<summary>
        /// ���»�������Ƿ�ɾ��
        ///</summary>
        ///<param name="contentLibraryId">���ݿ�ID</param>
        /// <param name="eventApplyIds">�����ID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
        public void UpdateContentLibraryApplyIsDelete(int contentLibraryId, string eventApplyIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ContentLibraryApply SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (contentLibraryId != -1)
            {
                sql.AppendFormat(" AND ContentLibraryID = {0} ", contentLibraryId);
            }
            if (!string.IsNullOrEmpty(eventApplyIds))
            {
                sql.AppendFormat(" AND OptionValue IN ({0}) ", eventApplyIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region   ��ȡIsDelete=1����

        /// <summary>
        /// ��ȡIsDelete=1����
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        public ContentLibraryEntity GetByIDs(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [ContentLibrary] where ID='{0}' ", id.ToString());
            //��ȡ����
            ContentLibraryEntity m = null;
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

        #endregion

        #region  ����IsDelete=1�Ĳ���

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Updates(ContentLibraryEntity pEntity, IDbTransaction pTran, bool pIsUpdateNullField)
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
            //pEntity.IsDelete = 1;

            //��֯������SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [ContentLibrary] set ");
            if (pIsUpdateNullField || pEntity.ClientType != null)
                strSql.Append("[ClientType]=@ClientType,");
            if (pIsUpdateNullField || pEntity.RegionFirstTypeID != null)
                strSql.Append("[RegionFirstTypeID]=@RegionFirstTypeID,");
            if (pIsUpdateNullField || pEntity.RegionSecondTypeID != null)
                strSql.Append("[RegionSecondTypeID]=@RegionSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerFirstTypeID != null)
                strSql.Append("[DealerFirstTypeID]=@DealerFirstTypeID,");
            if (pIsUpdateNullField || pEntity.DealerSecondTypeID != null)
                strSql.Append("[DealerSecondTypeID]=@DealerSecondTypeID,");
            if (pIsUpdateNullField || pEntity.DealerLastTypeID != null)
                strSql.Append("[DealerLastTypeID]=@DealerLastTypeID,");
            if (pIsUpdateNullField || pEntity.ContentApplicationScope != null)
                strSql.Append("[ContentApplicationScope]=@ContentApplicationScope,");
            if (pIsUpdateNullField || pEntity.ContentShowScope != null)
                strSql.Append("[ContentShowScope]=@ContentShowScope,");
            if (pIsUpdateNullField || pEntity.Title != null)
                strSql.Append("[Title]=@Title,");
            if (pIsUpdateNullField || pEntity.Description != null)
                strSql.Append("[Description]=@Description,");
            if (pIsUpdateNullField || pEntity.PictureUrl != null)
                strSql.Append("[PictureUrl]=@PictureUrl,");
            if (pIsUpdateNullField || pEntity.Content != null)
                strSql.Append("[Content]=@Content,");
            if (pIsUpdateNullField || pEntity.SortIndex != null)
                strSql.Append("[SortIndex]=@SortIndex,");
            if (pIsUpdateNullField || pEntity.Enabled != null)
                strSql.Append("[Enabled]=@Enabled,");
            if (pIsUpdateNullField || pEntity.OriginalUrl != null)
                strSql.Append("[OriginalUrl]=@OriginalUrl,");
            if (pIsUpdateNullField || pEntity.Remark != null)
                strSql.Append("[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ContentScenario != null)
                strSql.Append("[ContentScenario]=@ContentScenario,");
            if (pIsUpdateNullField || pEntity.VideoUrl != null)
                strSql.Append("[VideoUrl]=@VideoUrl,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy != null)
                strSql.Append("[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime != null)
                strSql.Append("[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.IsDelete != null)
                strSql.Append("[IsDelete]=@IsDelete,");
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
					new SqlParameter("@ContentApplicationScope",SqlDbType.NVarChar),
					new SqlParameter("@ContentShowScope",SqlDbType.NVarChar),
					new SqlParameter("@Title",SqlDbType.NVarChar),
					new SqlParameter("@Description",SqlDbType.NVarChar),
					new SqlParameter("@PictureUrl",SqlDbType.NVarChar),
					new SqlParameter("@Content",SqlDbType.NVarChar),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@Enabled",SqlDbType.NVarChar),
					new SqlParameter("@OriginalUrl",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ContentScenario",SqlDbType.NVarChar),
					new SqlParameter("@VideoUrl",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
                    new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@ID",SqlDbType.Int)
            };
            parameters[0].Value = pEntity.ClientType;
            parameters[1].Value = pEntity.RegionFirstTypeID;
            parameters[2].Value = pEntity.RegionSecondTypeID;
            parameters[3].Value = pEntity.DealerFirstTypeID;
            parameters[4].Value = pEntity.DealerSecondTypeID;
            parameters[5].Value = pEntity.DealerLastTypeID;
            parameters[6].Value = pEntity.ContentApplicationScope;
            parameters[7].Value = pEntity.ContentShowScope;
            parameters[8].Value = pEntity.Title;
            parameters[9].Value = pEntity.Description;
            parameters[10].Value = pEntity.PictureUrl;
            parameters[11].Value = pEntity.Content;
            parameters[12].Value = pEntity.SortIndex;
            parameters[13].Value = pEntity.Enabled;
            parameters[14].Value = pEntity.OriginalUrl;
            parameters[15].Value = pEntity.Remark;
            parameters[16].Value = pEntity.ContentScenario;
            parameters[17].Value = pEntity.VideoUrl;
            parameters[18].Value = pEntity.LastUpdateBy;
            parameters[19].Value = pEntity.LastUpdateTime;
            parameters[20].Value = pEntity.IsDelete;
            parameters[21].Value = pEntity.ID;

            //ִ�����
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion

        #region ���IsDelete=1�Ĳ���

        /// <summary>
        /// ����һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        public void Creates(ContentLibraryEntity pEntity)
        {
            this.Creates(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Creates(ContentLibraryEntity pEntity, IDbTransaction pTran)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");

            //��ʼ���̶��ֶ�
            pEntity.CreateTime = DateTime.Now;
            pEntity.CreateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.LastUpdateTime = pEntity.CreateTime;
            pEntity.LastUpdateBy = Convert.ToInt32(CurrentUserInfo.UserID);
            pEntity.IsDelete = 1;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [ContentLibrary](");
            strSql.Append("[ClientType],[RegionFirstTypeID],[RegionSecondTypeID],[DealerFirstTypeID],[DealerSecondTypeID],[DealerLastTypeID],[ContentApplicationScope],[ContentShowScope],[Title],[Description],[PictureUrl],[Content],[SortIndex],[Enabled],[OriginalUrl],[Remark],[ContentScenario],[VideoUrl],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete])");
            strSql.Append(" values (");
            strSql.Append("@ClientType,@RegionFirstTypeID,@RegionSecondTypeID,@DealerFirstTypeID,@DealerSecondTypeID,@DealerLastTypeID,@ContentApplicationScope,@ContentShowScope,@Title,@Description,@PictureUrl,@Content,@SortIndex,@Enabled,@OriginalUrl,@Remark,@ContentScenario,@VideoUrl,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete)");
            strSql.AppendFormat("{0}select SCOPE_IDENTITY();", Environment.NewLine);


            SqlParameter[] parameters = 
            {
					new SqlParameter("@ClientType",SqlDbType.NVarChar),
					new SqlParameter("@RegionFirstTypeID",SqlDbType.Int),
					new SqlParameter("@RegionSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerFirstTypeID",SqlDbType.Int),
					new SqlParameter("@DealerSecondTypeID",SqlDbType.Int),
					new SqlParameter("@DealerLastTypeID",SqlDbType.Int),
					new SqlParameter("@ContentApplicationScope",SqlDbType.NVarChar),
					new SqlParameter("@ContentShowScope",SqlDbType.NVarChar),
					new SqlParameter("@Title",SqlDbType.NVarChar),
					new SqlParameter("@Description",SqlDbType.NVarChar),
					new SqlParameter("@PictureUrl",SqlDbType.NVarChar),
					new SqlParameter("@Content",SqlDbType.NVarChar),
					new SqlParameter("@SortIndex",SqlDbType.Int),
					new SqlParameter("@Enabled",SqlDbType.NVarChar),
					new SqlParameter("@OriginalUrl",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ContentScenario",SqlDbType.NVarChar),
					new SqlParameter("@VideoUrl",SqlDbType.NVarChar),
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
            parameters[6].Value = pEntity.ContentApplicationScope;
            parameters[7].Value = pEntity.ContentShowScope;
            parameters[8].Value = pEntity.Title;
            parameters[9].Value = pEntity.Description;
            parameters[10].Value = pEntity.PictureUrl;
            parameters[11].Value = pEntity.Content;
            parameters[12].Value = pEntity.SortIndex;
            parameters[13].Value = pEntity.Enabled;
            parameters[14].Value = pEntity.OriginalUrl;
            parameters[15].Value = pEntity.Remark;
            parameters[16].Value = pEntity.ContentScenario;
            parameters[17].Value = pEntity.VideoUrl;
            parameters[18].Value = pEntity.CreateBy;
            parameters[19].Value = pEntity.CreateTime;
            parameters[20].Value = pEntity.LastUpdateBy;
            parameters[21].Value = pEntity.LastUpdateTime;
            parameters[22].Value = pEntity.IsDelete;

            //ִ�в��������д
            object result;
            if (pTran != null)
                result = this.SQLHelper.ExecuteScalar((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            pEntity.ID = Convert.ToInt32(result);
        }

        #endregion
    }
}
