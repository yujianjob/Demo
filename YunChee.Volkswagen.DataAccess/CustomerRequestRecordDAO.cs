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
    /// ���ݷ��ʣ� 0103�ͻ������¼�� CustomerRequestRecord 
    /// ��CustomerRequestRecord�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CustomerRequestRecordDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerRequestRecordEntity>, IQueryable<CustomerRequestRecordEntity>
    {
        #region �ͻ������¼(ͨ��΢���û���ʶ)

        /// <summary>
        /// �ͻ������¼
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
      
        public DataSet GetByOpenId(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.CustomerRequestRecord a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' and IsDelete=0 ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region �޸Ŀͻ������¼(ͨ��΢���û���ʶ)

        /// <summary>
        /// �ͻ������¼
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>

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
