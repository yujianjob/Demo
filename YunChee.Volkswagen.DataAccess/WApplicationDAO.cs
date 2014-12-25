/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/4 17:25:06
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
    /// 数据访问： 1001微信公众账号表 WApplication 
    /// 表WApplication的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class WApplicationDAO : BaseDAO<BasicUserInfo>, ICRUDable<WApplicationEntity>, IQueryable<WApplicationEntity>
    {
        #region 获取分页权限经销商列表

           /// <summary>
        /// 获取分页列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> WApplicationList(PagedQueryEntity pageEntity, WApplicationEntity wapplicationEntity, int clientID)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " (select  w.ID,c.Name TypeName, w.Name, w.Type ,w.WeixinID,w.WeixinNumber,w.DevelopUrl,";
            pageEntity.TableName += "  w.DevelopToken,w.AppID,w.AppSecret,w.LoginName,w.LoginPassword,w.OAuthUrl from dbo.WApplication w ";
            pageEntity.TableName += " LEFT JOIN  client c on w.ClientID = c.id ";
            pageEntity.TableName += " where w.isdelete = 0 and c.isdelete=0 and  c.id =" + clientID + ") t  ";


            pageEntity.QueryFieldName = " t.ID,t.TypeName, t.Name, t.Type ,t.WeixinID,t.WeixinNumber,t.DevelopUrl,  ";
            pageEntity.QueryFieldName += "t.DevelopToken,t.AppID,t.AppSecret,t.LoginName,t.LoginPassword, ";
            pageEntity.QueryFieldName += "  t.OAuthUrl";
            pageEntity.SortField = "t." + pageEntity.SortField;

         

            if (!string.IsNullOrEmpty(wapplicationEntity.Name))
            {
                pageEntity.QueryCondition += string.Format(" AND t.Name like '%{0}%' ", wapplicationEntity.Name);
            }

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;

        }


        #endregion

        #region 删除区域或经销商公众好
        /// <summary>
        /// 删除区域或者公众号
        /// </summary>
        /// <param name="quesIds"></param>
        public void DeleteWapplication(string quesIds)
        {
            if (!string.IsNullOrEmpty(quesIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.WApplication SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", quesIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }


        #endregion

        //[WeixinID]是否唯一
        public WApplicationEntity[] Query(WApplicationEntity pQueryEntity)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [WApplication] where [WeixinID]='" + pQueryEntity.WeixinID + "' and [ClientID]='" + pQueryEntity.ClientID + "' and ID!=" + pQueryEntity.ID + " and  isdelete=0  ");
            //执行SQL
            List<WApplicationEntity> list = new List<WApplicationEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    WApplicationEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            //返回结果
            return list.ToArray();
        }


        #region 获取微信号信息(通过微信ID)

        /// <summary>
        /// 获取微信号信息
        /// </summary>
        /// <param name="WinXinID">微信ID</param>
        public DataSet GetWinXinBasicInfo(string WinXinID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  ");
            sql.AppendFormat("WeixinName=a.Name,a.QrcodeUrl,DealerName=b.Name,DealerLongitude=b.Longitude,DealerLatitude=b.Latitude  ");
            sql.AppendFormat(" FROM dbo.WApplication a JOIN dbo.Client b ON a.ClientID=b.ID  ");
            sql.AppendFormat(" WHERE a.IsDelete=0 AND b.IsDelete=0 ");
            sql.AppendFormat(" AND a.WeixinID='{0}' ", WinXinID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion
    }
}
