/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/19 13:40:50
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
    /// ���ݷ��ʣ� 0406���ݿ���Ϸ�����  ContentLibraryGameAwards 
    /// ��ContentLibraryGameAwards�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ContentLibraryGameAwardsDAO : BaseDAO<BasicUserInfo>, ICRUDable<ContentLibraryGameAwardsEntity>, IQueryable<ContentLibraryGameAwardsEntity>
    {
        #region ������ϷID��ȡ��Ϸ�����б�

        /// <summary>
        /// ������ϷID��ȡ��Ϸ�����б�
        /// </summary>
        /// <param name="gameId">��ϷID</param>
        public DataSet GetContentGameAwardsListByMappingId(int mappingId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.* ");
            sql.AppendFormat(" FROM dbo.ContentLibraryGameAwards a ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.MappingID = {0} ", mappingId);
            sql.AppendFormat(" ORDER BY Grade ASC  ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ɾ����Ϸ�����б�

        /// <summary>
        /// ɾ����Ϸ�����б�
        /// </summary>
        /// <param name="gameAwardsIds">����ID����  "1,2,3"</param>
        public void DeleteContentGameAwards(string gameAwardsIds)
        {
            var sql = new StringBuilder();

            //���½����
            sql.AppendFormat(" UPDATE dbo.ContentLibraryGameAwards SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", gameAwardsIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region   ���ݹ�ϵ��ȡ��Ϸ����ID

        public DataSet GetContentLibraryGameAwardsEntityByGameId(int mappingId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.ContentLibraryGameAwards ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");
            sql.AppendFormat(" AND MappingID = {0} ", mappingId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
