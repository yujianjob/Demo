/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/30 14:06:32
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
    /// 数据访问： 0103客户请求记录表 CustomerRequestRecord 
    /// 表CustomerRequestRecord的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CustomerRequestRecordDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerRequestRecordEntity>, IQueryable<CustomerRequestRecordEntity>
    {
        #region 客户请求记录(通过微信用户标识)

        /// <summary>
        /// 客户请求记录
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
      
        public DataSet GetByOpenId(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.CustomerRequestRecord a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' and IsDelete=0 ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region 修改客户请求记录(通过微信用户标识)

        /// <summary>
        /// 客户请求记录
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>

        public DataSet UpdateByOpenId(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.CustomerRequestRecord  SET LastRequestDate=GETDATE() ");
            sql.AppendFormat(" WHERE WxOpenId='{0}' AND IsDelete=0 ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion
    }
}
