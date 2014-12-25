/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/9 14:04:16
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
using Yunchee.Volkswagen.Common.Enum;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// ���ݷ��ʣ� 0601ͶƱ�� Vote 
    /// ��Vote�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class VoteDAO : BaseDAO<BasicUserInfo>, ICRUDable<VoteEntity>, IQueryable<VoteEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳͶƱ�б�
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">�����ı�</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetVoteList(PagedQueryEntity pageEntity, string searchText, VoteEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"
        ( SELECT    a.* ,
                    VoteType = b.Name ,
                    c.TID
          FROM      dbo.Vote AS a
                    LEFT JOIN dbo.BasicData AS b ON a.Type = b.Value
                                                    AND b.IsDelete = 0
                                                    AND b.TypeCode = '{0}'
                    LEFT JOIN (
					SELECT VoteID,COUNT(VoteID) AS TID FROM  dbo.VoteOption WHERE isdelete=0 GROUP BY VoteID
					) AS c ON c.VoteID = a.ID
          WHERE     a.IsDelete = 0
                    AND a.ClientID = {1}
        ) AS t", E_BasicData.VoteType.ToString(),this.CurrentUserInfo.ClientID);

            pageEntity.QueryFieldName = string.Format("*");

            StringBuilder strcondition = new StringBuilder();

            if (!string.IsNullOrEmpty(searchText))
                strcondition.AppendFormat(" AND Name like '%{0}%'", searchText);
            pageEntity.QueryCondition = strcondition.ToString();

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }

        #endregion


        #region ����EventID��ȡ��ϵ��Ϣ

        /// <summary>
        /// ����EventID��ȡ��ϵ��Ϣ
        /// </summary>
        public DataSet GetEventVoteMappingByEventId(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.EventVoteMapping WHERE EventID = {0} ", eventId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ����ContentLibraryID��ȡ��ϵ��Ϣ

        /// <summary>
        /// ����ContentLibraryID��ȡ��ϵ��Ϣ
        /// </summary>
        public DataSet GetContentVoteMappingByContentLibraryID(int contentLibraryId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.ContentLibraryVoteMapping WHERE IsDelete=0 AND ContentLibraryID = {0} ", contentLibraryId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ�г������ͶƱ��Ϣ(���ݻID)

        /// <summary>
        /// ��ȡ�г������ͶƱ��Ϣ
        /// </summary>
        public DataSet GetMarketEventVoteInfo(int EventID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT VoteID=ID,VoteName=Name,Description,Type FROM  Vote WHERE id in	(SELECT VoteID FROM EventVoteMapping WHERE EventID={0} AND IsDelete=0) AND Vote.IsDelete=0", EventID);
            sql.AppendFormat(" SELECT * FROM VoteOption WHERE VoteID IN (SELECT ID FROM  Vote WHERE id in	(SELECT VoteID FROM EventVoteMapping WHERE EventID={0} AND IsDelete=0) AND Vote.IsDelete=0) ", EventID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡͶƱ����(����ͶƱID�Լ�ͶƱѡ��ID)

        /// <summary>
        /// ��ȡͶƱ����
        /// </summary>
        public int GetVoteCount(int VoteID,int VoteOptionID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT COUNT(*) FROM VoteResult a WHERE a.VoteID={0}", VoteID);
            sql.AppendFormat(" AND a.VoteOptionID={0} AND IsDelete=0 ", VoteOptionID);
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion
    }
}
