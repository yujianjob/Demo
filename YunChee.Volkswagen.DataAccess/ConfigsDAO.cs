/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/23 11:26:20
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
    /// ���ݷ��ʣ� 0908ϵͳ���ñ� Configs 
    /// ��Configs�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ConfigsDAO : BaseDAO<BasicUserInfo>, ICRUDable<ConfigsEntity>, IQueryable<ConfigsEntity>
    {
        #region ͨ��Key��ȡvalue

        /// <summary>
        /// ͨ��Key��ȡvalue
        /// </summary>
        /// <returns></returns>
        public DataSet GetValueByKey(string configKey)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ConfigValue FROM dbo.Configs  ");
            sql.AppendFormat(" WHERE ConfigKey='{0}' AND IsDelete=0  ", configKey);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
