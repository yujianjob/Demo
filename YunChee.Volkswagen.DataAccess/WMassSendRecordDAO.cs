/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/23 15:00:25
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
    /// ���ݷ��ʣ� 1009΢��Ⱥ����¼�� WMassSendRecord 
    /// ��WMassSendRecord�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class WMassSendRecordDAO : BaseDAO<BasicUserInfo>, ICRUDable<WMassSendRecordEntity>, IQueryable<WMassSendRecordEntity>
    {
        #region ��ȡ��ҳȨ���б�

        /// <summary>
        /// ��ȡ��ҳȨ���б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetMassSendRecordList(PagedQueryEntity entity, int applicationId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.WMassSendRecord a";
            entity.TableName += " LEFT JOIN dbo.BasicData b ON a.ReplyTypeID = b.Value AND b.IsDelete=0  AND b.TypeCode='ReplyType' ";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , ReplyTypeIDs = b.Name ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND a.ApplicationID = {0} ", applicationId);
            //entity.QueryCondition += " order by a.CreateTime desc ";
            entity.SortField = "a." + entity.SortField;
            if (entity.SortField.Equals("a.ReplyTypeIDs"))
                entity.SortField = "b.Name";
            //if (!string.IsNullOrEmpty(applicationId))
            //{
            //    entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%' OR EnglishName LIKE '%{0}%') ", searchText);
            //}

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region  ��ȡͼ���ز�����

        /// <summary>
        /// ��ȡͼ���ز�����
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetSendRecordList(PagedQueryEntity entity, string sendId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.WNews";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND id in({0}) ", sendId);
            entity.SortField = "SortIndex";
            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion


        #region �����Ա𣬿ͻ����ͻ�ȡID����

        /// <summary>
        /// �����Ա𣬿ͻ�����ID��ȡ����
        /// </summary>
        /// <param name="Sex">�Ա�</param>
        /// <param name="type">�ͻ�����</param>
        /// <returns></returns>
        public DataSet GetCustomeListByCustomeId(string wxSex, string type)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT WxOpenId FROM dbo.Customer ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            if (type != "-1")
            {
                sql.AppendFormat(" AND Type = {0} ", type);
            }

            if (wxSex != "-1")
            {
                sql.AppendFormat(" AND WxSex = {0} ", wxSex);
            }

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
        
        #region ��ȡÿ���ѷ��͵Ĵ���

        /// <summary>
        /// ��ȡÿ���ѷ��͵Ĵ���
        /// </summary>
        public string GetSendRecordLists(int sendId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
          SELECT ( SELECT    ConfigValue
          FROM      dbo.Configs
          WHERE     ConfigKey = 'MonthMassSendCount'
                    AND IsDelete = 0
        ) - ( SELECT    COUNT(*)
              FROM      dbo.WMassSendRecord
              WHERE     DATEDIFF(MONTH, CreateTime, GETDATE()) = 0
                        AND ApplicationID = {0}
                        AND IsDelete = 0
            )", sendId);

            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion


        #region  ��ȡͼ���ز�ID

        /// <summary>
        /// ��ȡͼ���ز�ID
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetMassSendRecordListId(PagedQueryEntity entity, int sendId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = string.Format(@"
                   dbo.WMassSendRecord a ,
                        dbo.WNewsMapping b ,
                        dbo.WNews c ");
            entity.QueryFieldName = string.Format("c.*");
            StringBuilder strcondition = new StringBuilder();
            strcondition.AppendFormat(" AND b.ObjectID = a.ID AND b.NewsID = c.ID ");
            strcondition.AppendFormat(" AND a.IsDelete = 0 AND b.IsDelete = 0 AND c.IsDelete = 0 ");
            strcondition.AppendFormat(" AND a.ReplyTypeID=2 AND b.TypeID=5 ");
            strcondition.AppendFormat(" AND a.ID={0}", sendId);
            entity.QueryCondition = strcondition.ToString();
            entity.SortField = "c." + entity.SortField;
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
    }
}
