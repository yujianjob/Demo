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
    /// 数据访问： 0704投诉与建议表 ComplainAndSuggest 
    /// 表ComplainAndSuggest的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ComplainAndSuggestDAO : BaseDAO<BasicUserInfo>, ICRUDable<ComplainAndSuggestEntity>, IQueryable<ComplainAndSuggestEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页投诉与建议列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">搜索文本</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetComplainAndSuggestList(PagedQueryEntity pageEntity, string SearchText, ComplainAndSuggestEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"(
     SELECT    a.* ,
                    b.WxNickName ,--微信昵称
                    ReplyName = c.ChineseName ,--回复人
                    IsReplyName = d.Name ,--是否回复
                    IsShowName = e.Name ,--是否显示
                    cc.ParentID AS ClientParentID -- 经销商ParentID
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
            if (queryEntity.IsReply != "-1")//回复
                strcondition.AppendFormat(" AND IsReply={0}", queryEntity.IsReply);
            if (queryEntity.IsShow != "-1")//显示
                strcondition.AppendFormat(" AND IsShow={0}", queryEntity.IsShow);
            if (!string.IsNullOrEmpty(SearchText))//客户姓名
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
