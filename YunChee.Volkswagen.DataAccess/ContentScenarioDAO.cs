/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/17 11:39:44
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
    /// ���ݷ��ʣ� 0810����Ӧ�ó����� ContentScenario 
    /// ��ContentScenario�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ContentScenarioDAO : BaseDAO<BasicUserInfo>, ICRUDable<ContentScenarioEntity>, IQueryable<ContentScenarioEntity>
    {
        #region ͨ��Key��ȡvalue

        /// <summary>
        /// ͨ��Key��ȡvalue
        /// </summary>
        /// <returns></returns>
        public DataSet GetContentSeriel()
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM ContentScenario  ");
            sql.AppendFormat(" WHERE IsDelete=0  ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion 
    }
}
