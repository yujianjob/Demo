/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/5/8 18:02:12
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
    /// ���ݷ��ʣ� 0804�������ݱ� BasicData 
    /// ��BasicData�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class BasicDataDAO : BaseDAO<BasicUserInfo>, ICRUDable<BasicDataEntity>, IQueryable<BasicDataEntity>
    {
        #region ��ȡָ�����͵ļ�ֵ���б�

        /// <summary>
        /// ��ȡָ�����͵ļ�ֵ���б�
        /// </summary>
        /// <param name="typecode">���͵ı���</param>
        /// <returns></returns>
        public DataSet GetTypeCodeList(string typeCode)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT value,name ");
            sql.AppendFormat(" FROM dbo.BasicData ");
            sql.AppendFormat(" WHERE IsDelete=0 ");
            sql.AppendFormat(" AND TypeCode = '{0}' ", typeCode);
            sql.AppendFormat(" ORDER BY SortIndex ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

    }

}
