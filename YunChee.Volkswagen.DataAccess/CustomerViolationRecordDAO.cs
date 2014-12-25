/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/25 10:51:03
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
    /// 数据访问： 0107客户违章记录表 CustomerViolationRecord 
    /// 表CustomerViolationRecord的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CustomerViolationRecordDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerViolationRecordEntity>, IQueryable<CustomerViolationRecordEntity>
    {
        #region 删除违章查询记录(通过微信识别号)

        /// <summary>
        /// 删除违章查询记录
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void DeleteCustomerViolationRecord(string OpenID)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" DELETE FROM CustomerViolationRecord ");
                sql.AppendFormat("  WHERE  ID IN (  ");
                sql.AppendFormat("  SELECT a.ID FROM  CustomerViolationRecord a ");
                sql.AppendFormat("  WHERE   a.CustomerPlateNumberID in ");
                sql.AppendFormat("  ( SELECT  b.ID FROM    CustomerPlateNumber b ");
                sql.AppendFormat("  WHERE   b.CustomerID in ( SELECT d.ID FROM dbo.Customer d ");
                sql.AppendFormat("  WHERE d.WxOpenId = '{0}' AND d.IsDelete=0) AND b.IsDelete=0) )", OpenID);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }
        #endregion

        #region 获取车主车辆违章记录(通过微信用户标识)

        /// <summary>
        /// 获取车主车辆违章记录
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetCustomerViolationRecord(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT LicensePlateNumber=b.LicensePlateNumber, ");
            sql.AppendFormat(" a.Date,a.area,a.act,a.Code,a.Points,a.Money,a.Handled ");
            sql.AppendFormat(" FROM CustomerViolationRecord a JOIN CustomerPlateNumber b ON a.CustomerPlateNumberID=b.ID ");
            sql.AppendFormat(" WHERE b.CustomerID=(SELECT c.ID FROM dbo.Customer c WHERE c.WxOpenId='{0}' AND c.IsDelete=0) ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion
    }
}
