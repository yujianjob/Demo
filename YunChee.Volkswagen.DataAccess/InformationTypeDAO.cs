/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/10 14:03:22
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
    /// 数据访问： 1501资讯类型 InformationType 
    /// 表InformationType的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class InformationTypeDAO : BaseDAO<BasicUserInfo>, ICRUDable<InformationTypeEntity>, IQueryable<InformationTypeEntity>
    {
        #region 获取资讯列表

        /// <summary>
        /// 获取资讯列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetInformationTypeListById(string parentId, string id)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.InformationType WHERE IsDelete=0 ");

            if (!string.IsNullOrEmpty(parentId))
            {
                sql.AppendFormat(" AND ParentID = {0} ", parentId);
            }
            if (!string.IsNullOrEmpty(id))
            {
                sql.AppendFormat(" OR ID = {0} ", id);
            }
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取指定的资讯

        /// <summary>
        /// 获取指定的资讯
        /// </summary>
        /// <returns></returns>
        public DataSet GetInformationById(string id)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.InformationType ");
            sql.AppendFormat(" WHERE IsDelete = 0 ");

            if (!string.IsNullOrEmpty(id))
            {
                sql.AppendFormat(" AND ID = {0} ", id);
            }

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
