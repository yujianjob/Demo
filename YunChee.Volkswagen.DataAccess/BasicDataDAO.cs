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
    /// 数据访问： 0804基础数据表 BasicData 
    /// 表BasicData的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class BasicDataDAO : BaseDAO<BasicUserInfo>, ICRUDable<BasicDataEntity>, IQueryable<BasicDataEntity>
    {
        #region 获取指定类型的键值对列表

        /// <summary>
        /// 获取指定类型的键值对列表
        /// </summary>
        /// <param name="typecode">类型的编码</param>
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
