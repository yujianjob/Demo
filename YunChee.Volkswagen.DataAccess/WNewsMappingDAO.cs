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
using Yunchee.Volkswagen.Common.Const;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// ���ݷ��ʣ� 1003ͼ���زĹ�ϵ�� WNewsMapping 
    /// ��WNewsMapping�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class WNewsMappingDAO : BaseDAO<BasicUserInfo>, ICRUDable<WNewsMappingEntity>, IQueryable<WNewsMappingEntity>
    {
        /// <summary>
        /// ��ȡδѡ��ͼ���б�
        /// </summary>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetUnderWSubscriptionReply(PagedQueryEntity entity, string selectType, int clientID, string loginType, string newsMappingType, Int32? objectID)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            //�����½
            if (loginType == C_ClientType.REGIONAL)
            {
                //ѡ������
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews news";
                    entity.QueryCondition = "  and news.IsDelete = 0 ";
                    entity.QueryCondition += "  AND news.ClientID =" + clientID + "";
                    entity.QueryCondition += " AND NOT EXISTS (SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND news.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID="+objectID+" and TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "news.SortIndex";
                    entity.QueryFieldName = " news.* ";
                }
                //ѡ������
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " And a.IsDelete = 0 AND b.ParentID = " + clientID + "  AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = " a.* ";
                }
            }
            //�����̵�½
            else
            {
                //ѡ������
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " and a.IsDelete = 0 AND b.ID = (SELECT c.ParentID FROM dbo.Client c WHERE c.IsDelete = 0 AND c.ID = " + clientID + ") ";
                    entity.QueryCondition += " AND NOT EXISTS( SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE  ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";
                }

                //ѡ������
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a ";
                    entity.QueryCondition = " and a.IsDelete=0 and a.ClientID = " + clientID + " ";
                    entity.QueryCondition += " AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (Select ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";
                }

            }


            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);
            return result;
        }

        #region ��ȡָ����ɫ���û��б�������ɾ�����û���

        /// <summary>
        /// ��ȡ_ͼ���زĹ�ϵ�� ��������ɾ�����û���
        /// </summary>
        /// <param name="roleId">��ɫID</param>
        /// <returns></returns>
        public DataSet GetWnewMappinByObjectId(int type,int ObjectId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT w.NewsID FROM dbo.WNewsMapping w ");
            sql.AppendFormat(" WHERE w.ObjectID = {0} and typeid={1}", ObjectId,type);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ��Ϣ�б����Ϣ����
        /// <summary>
        /// ��ȡ��Ϣ�б����Ϣ����
        /// </summary>
        /// <param name="objectid">����id</param>
        /// <param name="type">����</param>
        /// <param name="isdelete">�Ƿ�ɾ��</param>
        /// <returns></returns>
        public int newMapping(int objectid, string type, int isdelete)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" select COUNT(*) FROM WNewsMapping where");
            sql.AppendFormat(" typeid={0} and objectid={1} and isdelete={2}", type, objectid, isdelete);
            DataSet ds = SQLHelper.ExecuteDataset(sql.ToString());
            DataTable da = ds.Tables[0];
            if (da != null && da.Rows[0][0].ToString() != "")
                return Convert.ToInt32(da.Rows[0][0]);
            else
                return 0;
        }

        #endregion

        public int maxnewMappingIndex(int objectid, string type, int isdelete)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" select MAX(SortIndex) FROM WNewsMapping where");
            sql.AppendFormat(" typeid={0} and objectid={1} and isdelete={2}", type, objectid, isdelete);
            DataSet ds = SQLHelper.ExecuteDataset(sql.ToString());
            DataTable da = ds.Tables[0];
            if (da != null && da.Rows[0][0].ToString()!="" )
                return Convert.ToInt32(da.Rows[0][0]);
            else
                return 0;
        }

      

        #region ����ͼ���زı�

     

        ///<summary>
        /// ����ͼ���زı�
        ///</summary>
        ///<param name="roleId">��ɫID</param>
        /// <param name="usersIds">�û�ID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
        public void UpdateWnewMappingIsDelete(int roleId, string usersId,int sortIndex, int isDelete)
        {             
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.WNewsMapping SET IsDelete = {0}, SortIndex={1},", isDelete, sortIndex);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (roleId != -1)
            {
                sql.AppendFormat(" AND ObjectID = {0} ", roleId);
            }
            if (!string.IsNullOrEmpty(usersId))
            {
                sql.AppendFormat(" AND NewsID IN ({0}) ", usersId);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region ���½�ɫ�û����Ƿ�ɾ��

        ///<summary>
        /// ���½�ɫ�û����Ƿ�ɾ��
        ///</summary>
        ///<param name="roleId">��ɫID</param>
        /// <param name="usersIds">�û�ID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
        public void UpdateNewMappingIsDelete(int? objectID, string updateIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.WNewsMapping SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (objectID != -1)
            {
                sql.AppendFormat(" AND ObjectID = {0} ", objectID);
            }
            if (!string.IsNullOrEmpty(updateIds))
            {
                sql.AppendFormat(" AND ID IN ({0}) ", updateIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region ����΢�Ų˵�ͼ���ز��Ƿ�ɾ��

        ///<summary>
        /// ����΢�Ų˵�ͼ���ز��Ƿ�ɾ��
        ///</summary>
        ///<param name="wMenuId">΢�Ų˵�ID</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
        public void UpdateNewMappingIsDeletes(int? objectID, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.WNewsMapping SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (objectID != -1)
            {
                sql.AppendFormat(" AND ID = {0} ", objectID);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region

        public PagedQueryObjectResult<DataSet> GetUnderWSubscriptionReplyLists(PagedQueryEntity entity, string selectType, int clientID, string loginType, string newsMappingType, Int32? objectID, string wNewsId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            //�����½
            if (loginType == C_ClientType.REGIONAL)
            {
                //ѡ������
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews news";
                    entity.QueryCondition = "  and news.IsDelete = 0 ";
                    entity.QueryCondition += "  AND news.ClientID =" + clientID + "";
                    entity.QueryCondition += " AND NOT EXISTS (SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND news.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "news.SortIndex";
                    entity.QueryFieldName = " news.* ";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND news.ID NOT IN({0}) ", wNewsId);
                    }
                }
                //ѡ������
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " And a.IsDelete = 0 AND b.ParentID = " + clientID + "  AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = " a.* ";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND a.ID NOT IN({0}) ", wNewsId);
                    }
                }
            }
            //�����̵�½
            else
            {
                //ѡ������
                if (selectType == C_ClientType.REGIONAL)
                {
                    entity.TableName = " dbo.WNews a INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                    entity.QueryCondition = " and a.IsDelete = 0 AND b.ID = (SELECT c.ParentID FROM dbo.Client c WHERE c.IsDelete = 0 AND c.ID = " + clientID + ") ";
                    entity.QueryCondition += " AND NOT EXISTS( SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE  ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (SELECT ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND a.ID NOT IN({0}) ", wNewsId);
                    }
                }

                //ѡ������
                if (selectType == C_ClientType.DEALER)
                {
                    entity.TableName = " dbo.WNews a ";
                    entity.QueryCondition = " and a.IsDelete=0 and a.ClientID = " + clientID + " ";
                    entity.QueryCondition += " AND NOT EXISTS( ";
                    entity.QueryCondition += " SELECT 1 FROM dbo.WNewsMapping NewsMapping WHERE ";
                    entity.QueryCondition += " NewsMapping.IsDelete=0 AND a.ID IN (Select ";
                    entity.QueryCondition += " NewsID FROM dbo.WNewsMapping WHERE  IsDelete=0 and  ObjectID=" + objectID + " and  TypeID='" + newsMappingType + "' ) ) ";
                    entity.SortField = "a.SortIndex";
                    entity.QueryFieldName = "a.*";

                    if (!string.IsNullOrEmpty(wNewsId))
                    {
                        entity.QueryCondition += string.Format(" AND a.ID NOT IN({0}) ", wNewsId);
                    }
                }

            }


            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);
            return result;
        }

        #endregion

    }
}
