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
    /// ���ݷ��ʣ� 0107�ͻ�Υ�¼�¼�� CustomerViolationRecord 
    /// ��CustomerViolationRecord�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CustomerViolationRecordDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerViolationRecordEntity>, IQueryable<CustomerViolationRecordEntity>
    {
        #region ɾ��Υ�²�ѯ��¼(ͨ��΢��ʶ���)

        /// <summary>
        /// ɾ��Υ�²�ѯ��¼
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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

        #region ��ȡ��������Υ�¼�¼(ͨ��΢���û���ʶ)

        /// <summary>
        /// ��ȡ��������Υ�¼�¼
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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
