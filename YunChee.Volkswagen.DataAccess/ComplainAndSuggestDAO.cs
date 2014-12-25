/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/15 16:31:12
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
using Yunchee.Volkswagen.Common.Const;

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// ���ݷ��ʣ� 0704Ͷ���뽨��� ComplainAndSuggest 
    /// ��ComplainAndSuggest�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ComplainAndSuggestDAO : BaseDAO<BasicUserInfo>, ICRUDable<ComplainAndSuggestEntity>, IQueryable<ComplainAndSuggestEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳͶ���뽨���б�
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">�����ı�</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetComplainAndSuggestList(PagedQueryEntity pageEntity, string SearchText, ComplainAndSuggestEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"(
     SELECT    a.* ,
                    b.WxNickName ,--΢���ǳ�
                    ReplyName = c.ChineseName ,--�ظ���
                    IsReplyName = d.Name ,--�Ƿ�ظ�
                    IsShowName = e.Name ,--�Ƿ���ʾ
                    cc.ParentID AS ClientParentID -- ������ParentID
          FROM      dbo.ComplainAndSuggest AS a
                    LEFT JOIN dbo.Customer AS b ON b.WxOpenId = a.WxOpenId
                                                   AND b.IsDelete = 0
                    LEFT JOIN dbo.Users AS c ON c.ClientID = a.ReplyID
                                                AND c.IsDelete = 0
                    LEFT JOIN dbo.BasicData AS d ON d.Value = a.IsReply
                                                    AND d.TypeCode = '{0}'
                                                    AND d.IsDelete = 0
                    LEFT JOIN dbo.BasicData AS e ON e.Value = a.IsShow
                                                    AND e.TypeCode = '{1}'
                                                    AND e.IsDelete = 0
                    LEFT JOIN dbo.Client AS cc ON cc.ID = a.ClientID
                                                  AND cc.IsDelete = 0
          WHERE     a.IsDelete = 0
        ) AS t", E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString());

            pageEntity.QueryFieldName = string.Format("*");

            StringBuilder strcondition = new StringBuilder();
            if (queryEntity.IsReply != "-1")//�ظ�
                strcondition.AppendFormat(" AND IsReply={0}", queryEntity.IsReply);
            if (queryEntity.IsShow != "-1")//��ʾ
                strcondition.AppendFormat(" AND IsShow={0}", queryEntity.IsShow);
            if (!string.IsNullOrEmpty(SearchText))//�ͻ�����
                strcondition.AppendFormat(" AND( SuggestContent LIKE '%{0}%' or ReplyContent LIKE '%{0}%' )", SearchText);


            if (this.CurrentUserInfo.ClientType == C_ClientType.DEALER)
                strcondition.AppendFormat(" AND ClientID={0}", this.CurrentUserInfo.ClientID);
            if (this.CurrentUserInfo.ClientType == C_ClientType.REGIONAL)
            {
                if (queryEntity.ClientID != -1)
                    strcondition.AppendFormat(" AND ClientID={0}", queryEntity.ClientID);
                else
                    strcondition.AppendFormat(" AND (ClientParentID={0} OR ClientID={0})", this.CurrentUserInfo.ClientID);
            }

            pageEntity.QueryCondition = strcondition.ToString();


            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion
    }
}
