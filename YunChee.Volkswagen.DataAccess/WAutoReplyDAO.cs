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
    /// 数据访问： 1005消息自动回复 WAutoReply 
    /// 表WAutoReply的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WAutoReplyDAO : BaseDAO<BasicUserInfo>, ICRUDable<WAutoReplyEntity>, IQueryable<WAutoReplyEntity>
    {

        /// <summary>
        /// 删除区域或者公众号
        /// </summary>
        /// <param name="quesIds"></param>
        public void UpdateWAutoReplay(Int32? applicationid, string text)
        {

            var sql = new StringBuilder();

            sql.AppendFormat(" UPDATE dbo.WAutoReply SET text='{0}', ", text);
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ApplicationID IN ({0}) ", applicationid);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());

        }




        /// <summary>
        /// 获取选中图文列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetWAutoReply(PagedQueryEntity entity, int clientID, string loginType, Int32? objectID)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);
            //区域登陆
            if (loginType == C_ClientType.REGIONAL)
            {
                entity.TableName = "dbo.WNews a  ";
                entity.TableName += " INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                entity.TableName += "INNER JOIN dbo.WNewsMapping c ON c.NewsID=a.ID";
                entity.QueryCondition = " and a.IsDelete = 0 AND c.IsDelete=0 and c.TypeID='"+C_NewsType.MessageAutoReply+"' and c.ObjectID=" + objectID + " and (b.ParentID = " + clientID + " OR b.ID=" + clientID + "  )";
                entity.SortField = "c.SortIndex";
                entity.QueryFieldName = "c.id NewsMappingId,c.SortIndex MappingSortIndex, a.*";
            }
            //经销商登陆
            if (loginType == C_ClientType.DEALER)
            {
                entity.TableName = "  dbo.WNews a ";
                entity.TableName += " INNER JOIN dbo.Client b ON a.ClientID = b.ID AND b.IsDelete = 0 ";
                entity.TableName += " INNER JOIN dbo.WNewsMapping c ON c.NewsID=a.ID ";
                entity.QueryCondition = "and a.IsDelete = 0 AND c.IsDelete=0 and  c.TypeID='" + C_NewsType.MessageAutoReply+ "' and  c.ObjectID=" + objectID + " and ( b.ID = (SELECT c.ParentID FROM dbo.Client c WHERE c.IsDelete = 0 AND c.ID = " + clientID + ") ";
                entity.QueryCondition += "OR b.ID=" + clientID + " )";
                entity.SortField = "c.SortIndex";
                entity.QueryFieldName = "c.id NewsMappingId,c.SortIndex MappingSortIndex,a.*";
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);
            return result;
        }

       
    }
}
