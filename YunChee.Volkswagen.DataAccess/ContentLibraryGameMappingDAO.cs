/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/13 13:42:54
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
    /// ���ݷ��ʣ� 0405���ݿ���Ϸ��ϵ�� ContentLibraryGameMapping 
    /// ��ContentLibraryGameMapping�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ContentLibraryGameMappingDAO : BaseDAO<BasicUserInfo>, ICRUDable<ContentLibraryGameMappingEntity>, IQueryable<ContentLibraryGameMappingEntity>
    {
        #region ��ȡ��Ϸ��ҳȨ��

        /// <summary>
        /// ��ȡ��Ϸ��ҳȨ��
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetGameLists(PagedQueryEntity pageEntity, int contentId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " ContentLibraryGameMapping ";
            pageEntity.QueryCondition = " and IsDelete = 0 ";
            pageEntity.QueryFieldName = " * ";
            pageEntity.QueryCondition += string.Format(" AND ContentLibraryID={0} ", contentId);

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }

        #endregion

        #region ɾ�����ݿ���Ϸ�б�

        /// <summary>
        /// ɾ�����ݿ���Ϸ�б�
        /// </summary>
        /// <param name="gameMappingId">���ݿ���ϷID</param>
        public void DeleteContentLibraryGameMapping(string gameMappingId)
        {
            if (!string.IsNullOrEmpty(gameMappingId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.ContentLibraryGameMapping SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", gameMappingId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region ��ȡָ�����ݿ����Ϸ�б�

        ///<summary>
        /// ��ȡָ�����ݿ����Ϸ�б�
        ///</summary>
        /// <param name="contentLibraryId">���ݿ�ID</param>
        /// <param name="gameIDs">��ϷID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
        public void UpdateContentLibraryGameMappingIsDelete(int contentLibraryId, string gameIDs, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ContentLibraryGameMapping SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (contentLibraryId != -1)
            {
                sql.AppendFormat(" AND ContentLibraryID = {0} ", contentLibraryId);
            }
            if (!string.IsNullOrEmpty(gameIDs))
            {
                sql.AppendFormat(" AND GameID IN ({0}) ", gameIDs);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region ��ȡָ�����ݿ����Ϸ�б�

        /// <summary>
        /// ��ȡָ����Ϸ�б�������ɾ������Ϸ��
        /// </summary>
        /// <param name="contentLibraryId">���ݿ�ID</param>
        /// <returns></returns>
        public DataSet GetContentLibraryGameMappingByGameId(int contentLibraryId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.GameID FROM dbo.ContentLibraryGameMapping a ");
            sql.AppendFormat(" WHERE a.ContentLibraryID = {0} ", contentLibraryId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ������ݿ���Ϸ�б�

        /// <summary>
        /// ������ݿ���Ϸ�б�
        /// </summary>
        /// <param name="contentLibraryId">���ݿ�ID</param>
        /// <param name="gameIDs"></param>
        public void AddContentLibraryGameMapping(int contentLibraryId, string gameIDs)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" INSERT INTO dbo.ContentLibraryGameMapping ");
            sql.AppendFormat(" (ContentLibraryID ,GameID ,ImageUrl ,GameName ,GameDescription,InitialTime ,IconUrl ,CreateBy ,CreateTime ,LastUpdateBy ,LastUpdateTime ,IsDelete) ");
            sql.AppendFormat(" SELECT  {0} ,a.ID ,a.GameUrl ,a.Name ,a.Description ,a.InitialTime ,a.IconUrl ,{1} ,GETDATE() ,{1} ,GETDATE() ,0 ", contentLibraryId, this.CurrentUserInfo.UserID);
            sql.AppendFormat(" FROM    dbo.Game a ");
            sql.AppendFormat(" WHERE   a.IsDelete = 0 AND a.ID IN (" + gameIDs + ") ");

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region ������ݿ���Ϸ�����б�

        /// <summary>
        /// ������ݿ���Ϸ�����б�
        /// </summary>
        /// <param name="mappingIds">���ݿ�ID</param>
        /// <param name="gameIDs">��ϷID</param>
        /// <param name="gameAwardsIds">����ID</param>
        public void AddContentLibraryGameAwards(string mappingIds, int gameIDs, string gameAwardsIds)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" INSERT INTO dbo.ContentLibraryGameAwards ");
            sql.AppendFormat(" (MappingID ,GameID ,Point ,Time ,Grade ,CreateBy ,CreateTime ,LastUpdateBy ,LastUpdateTime ,IsDelete) ");
            sql.AppendFormat(" SELECT  {0} ,{1} ,a.Point ,a.Time ,a.Grade ,{2} ,GETDATE() ,{2} ,GETDATE() ,0 ", mappingIds, gameIDs, this.CurrentUserInfo.UserID);
            sql.AppendFormat(" FROM    dbo.GameAwards a ");
            sql.AppendFormat(" WHERE   a.IsDelete = 0 AND a.ID IN (" + gameAwardsIds + ") ");

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        public void AddContentGameAwards(string mappingIds, int gameIDs)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" INSERT INTO dbo.ContentLibraryGameAwards ");
            sql.AppendFormat(" (MappingID ,GameID ,Point ,Time ,Grade ,CreateBy ,CreateTime ,LastUpdateBy ,LastUpdateTime ,IsDelete) ");
            sql.AppendFormat(" SELECT  {0} ,{1} ,a.Point ,a.Time ,a.Grade ,{2} ,GETDATE() ,{2} ,GETDATE() ,0 ", mappingIds, gameIDs, this.CurrentUserInfo.UserID);
            sql.AppendFormat(" FROM    dbo.GameAwards a ");
            sql.AppendFormat(" WHERE   a.IsDelete = 0 AND a.ID IN ( SELECT ID FROM dbo.GameAwards WHERE IsDelete=0 AND GameID IN({0}) ) ", gameIDs);

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region ����ָ�����ݿ����Ϸ�б�

        ///<summary>
        /// ����ָ�����ݿ����Ϸ�б�
        ///</summary>
        /// <param name="contentLibraryId">���ݿ�ID</param>
        /// <param name="gameIDs">��ϷID���� "1,2,3"</param>
        /// <param name="isDelete">ɾ����־  0=��  1=��</param>
        ///<returns>�ܵļ�¼��</returns>
        public void UpdateContentLibraryGameAwardsIsDelete(int mappingIds, string gameIDs, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ContentLibraryGameAwards SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (mappingIds != -1)
            {
                sql.AppendFormat(" AND MappingID = {0} ", mappingIds);
            }
            if (!string.IsNullOrEmpty(gameIDs))
            {
                sql.AppendFormat(" AND ID IN ({0}) ", gameIDs);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        public void UpdateContentGameAwardsIsDelete(int mappingIds, string gameIDs, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ContentLibraryGameAwards SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (mappingIds != -1)
            {
                sql.AppendFormat(" AND MappingID = {0} ", mappingIds);
            }
            if (!string.IsNullOrEmpty(gameIDs))
            {
                sql.AppendFormat(" AND ID IN ( SELECT ID FROM dbo.ContentLibraryGameAwards WHERE MappingID = {0} AND GameID IN ( {1} ) ) ", mappingIds, gameIDs);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }
        public void UpdateContentGameAwardsIsDeletes(int mappingIds, int isDelete)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ContentLibraryGameAwards SET IsDelete = {0}, ", isDelete);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE 1 = 1 ");

            if (mappingIds != -1)
            {
                sql.AppendFormat(" AND MappingID = {0} ", mappingIds);
            }

            this.SQLHelper.ExecuteNonQuery(sql.ToString()).ToInt();
        }

        #endregion

        #region   ������ϷID��ȡ��Ϸ����ID

        public DataSet GetContentLibraryGameAwardsByGameId(int gameId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ID FROM dbo.GameAwards ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND GameID = {0} ", gameId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
         
        #region   ������ϷID��ȡ��Ϸ����ID  ������ɾ����

        public DataSet GetContentLibraryGameAwardsEntityByGameId(int mappingID, int gameId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ID FROM dbo.ContentLibraryGameAwards ");
            sql.AppendFormat(" WHERE  ");//ANDIsDelete = 0
            sql.AppendFormat("  MappingID = {0} ", mappingID);
            sql.AppendFormat(" AND GameID = {0} ", gameId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region  

        //public DataSet GetContentLibraryGameMappingsByGameId(int contentLibraryId)
        //{
        //    var sql = new StringBuilder();



        //    sql.AppendFormat(" SELECT ID FROM dbo.ContentLibraryGameMapping ");
        //    sql.AppendFormat(" WHERE IsDelete = 0 ");
        //    sql.AppendFormat(" AND ContentLibraryID = {0} ", contentLibraryId);

        //    return this.SQLHelper.ExecuteDataset(sql.ToString());
        //}

        #endregion

        #region �������ݿ���Ϸ��Ӧ�Ĺ�ϵID  ������ɾ��

        /// <summary>
        /// �������ݿ���Ϸ��Ӧ�Ĺ�ϵID
        /// </summary>
        public string GetContentLibraryGameMappingsByGameIds(int contentLibraryId, int gameId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
              SELECT ID FROM dbo.ContentLibraryGameMapping 
                WHERE  ContentLibraryID = {0} AND GameID = {1}", contentLibraryId, gameId);
            //IsDelete = 0 AND
            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }
        public string GetContentameMappingsByGameIds(int contentLibraryId, int gameId)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
              SELECT ID FROM dbo.ContentLibraryGameMapping 
                WHERE IsDelete = 0 AND ContentLibraryID = {0} AND GameID = {1}", contentLibraryId, gameId);
            result = this.SQLHelper.ExecuteScalar(sb.ToString()).ToString();
            return result;
        }

        #endregion
    }
}
