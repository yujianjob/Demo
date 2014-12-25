/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/12/19 23:05:56
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
using YuJian.WeiXin.Entity;
using Yunchee.Volkswagen.Utility.DataAccess.Query;
using Yunchee.Volkswagen.DataAccess.Base;

namespace YuJian.WeiXin.DataAccess
{
    
    /// <summary>
    /// ���ݷ��ʣ�  
    /// ��CustomerPrizeMapping�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CustomerPrizeMappingDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerPrizeMappingEntity>, IQueryable<CustomerPrizeMappingEntity>
    {
        public DataSet GetLottery()
        {
            return this.SQLHelper.ExecuteDataset(CommandType.Text, @"SELECT * FROM dbo.CustomerPrizeMapping 
WHERE Enable=1 OR EXISTS (SELECT 1 FROM dbo.DrawAll WHERE CustomerPrizeMapping.Openid=dbo.DrawAll.Openid)");
        }
    }
}
