/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/10 17:48:52
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
    /// 数据访问： 1109在线询价表 ServiceAskPrice 
    /// 表ServiceAskPrice的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ServiceAskPriceDAO : BaseDAO<BasicUserInfo>, ICRUDable<ServiceAskPriceEntity>, IQueryable<ServiceAskPriceEntity>
    {
        /// <summary>
        /// 预约维修ID
        /// </summary>
        /// <param name="roId"></param>
        /// <returns></returns>
        public ServiceAskPriceEntity GetRAskPriceOrderById(object roId)
        {
            var entity = new ServiceAskPriceEntity();
            //参数检查
            if (roId == null)
                return null;
            string id = roId.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
    	 SELECT  a.* ,
            b.Name AS CarTypeName ,--车型名称
            c.Name AS CarStyleName --车款名称
            FROM    dbo.ServiceAskPrice AS a
            LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                          AND b.IsDelete = 0
            LEFT JOIN dbo.CarStyle AS c ON a.CarStyleID = c.ID
                                           AND c.IsDelete = 0
               WHERE     a.IsDelete = 0  AND a.ID={0}", id.ToString());

            DataSet ds = this.SQLHelper.ExecuteDataset(sql.ToString());

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                entity = DataTableToObject.ConvertToObject<ServiceAskPriceEntity>(ds.Tables[0].Rows[0]);
            }


            //返回
            return entity;
        }
    }
}
