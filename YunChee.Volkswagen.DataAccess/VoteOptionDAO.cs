/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/8 14:14:40
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
    /// ���ݷ��ʣ� 0602ͶƱѡ��� VoteOption 
    /// ��VoteOption�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class VoteOptionDAO : BaseDAO<BasicUserInfo>, ICRUDable<VoteOptionEntity>, IQueryable<VoteOptionEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳͶƱѡ���б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetVoteOptionList(PagedQueryEntity pageEntity, int voteId)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = "dbo.VoteOption a";
            pageEntity.TableName += " LEFT JOIN dbo.Vote b ON a.VoteID=b.ID AND b.IsDelete=0 ";
            pageEntity.QueryFieldName = "a.*";
            pageEntity.QueryCondition = " AND a.IsDelete = 0 ";
            pageEntity.QueryCondition += string.Format(" AND a.VoteID = {0} ", voteId);
            pageEntity.SortField = "a." + pageEntity.SortField;
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;
        }

        #endregion
    }
}
