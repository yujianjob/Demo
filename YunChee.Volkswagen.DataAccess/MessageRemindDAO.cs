/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/9/17 18:34:53
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
    /// 数据访问： 0910消息提醒表 MessageRemind 
    /// 表MessageRemind的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class MessageRemindDAO : BaseDAO<BasicUserInfo>, ICRUDable<MessageRemindEntity>, IQueryable<MessageRemindEntity>
    {
        #region 获取一条系统提示消息

        /// <summary>
        /// 根据企业ID 获取消息
        /// </summary>
        /// <param name="clientId">企业ID</param>
        /// <returns></returns>
        public DataSet GetMessageRemind(int clientId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP 1 ");
            sql.AppendFormat(" MIN(CreateTime) ,COUNT(id) ,MessageType ");
            sql.AppendFormat(" FROM   dbo.MessageRemind  ");
            sql.AppendFormat(" 	WHERE  Status = 0");
            sql.AppendFormat("  AND IsDelete = 0 AND ClientID={0} ", clientId);
            sql.AppendFormat(" GROUP BY MessageType ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 更新消息

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="typeId">消息类型</param>
        public void UpdateMessageRemind(string typeId)
        {
            if (!string.IsNullOrEmpty(typeId))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.MessageRemind SET Status = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE MessageType={0} ", typeId);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion
    }
}
