/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/9 21:12:51
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
    /// 数据访问： 1101客服会话信息表 ServiceSessionInfo 
    /// 表ServiceSessionInfo的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ServiceSessionInfoDAO : BaseDAO<BasicUserInfo>, ICRUDable<ServiceSessionInfoEntity>, IQueryable<ServiceSessionInfoEntity>
    {
        #region 获取聊天记录(通过业务类型和订单号)

        /// <summary>
        /// 获取聊天记录(通过业务类型和订单号)
        /// </summary>
        /// <param name="BusinessType">业务类型</param>
        /// <param name="BusinessID">订单号</param>
        public DataSet GetChatRecord(string BusinessType, string BusinessID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT MessageType, SendTime=CONVERT(varchar(100),SendTime, 120),SessionFlag, ");
            sql.AppendFormat("        WxHeadImgUrl=(SELECT WxHeadImgUrl FROM dbo.Customer WHERE WxOpenId=a.FromOpenId AND IsDelete=0), ");
            sql.AppendFormat("        ChatContent =(CASE WHEN MessageType='1' THEN (SELECT Text FROM dbo.ServiceText WHERE SessionID=a.ID ) ");
            sql.AppendFormat("                           WHEN MessageType='2' THEN (SELECT LocalPicUrl FROM dbo.ServiceImage WHERE SessionID=a.ID) ");
            sql.AppendFormat("                           WHEN MessageType='3' THEN (SELECT LocalMediaUrl FROM dbo.ServiceVoice WHERE SessionID=a.ID) ");
            sql.AppendFormat("                           WHEN MessageType='4' THEN (SELECT LocalMediaUrl FROM dbo.ServiceVideo WHERE SessionID=a.ID) ");
            sql.AppendFormat("                           WHEN MessageType='5' THEN (SELECT (LocationX+','+LocationY+','+Scale+','+Label) as aa FROM dbo.ServiceLocation WHERE SessionID=a.ID) ");
            sql.AppendFormat("                           WHEN MessageType='6' THEN (SELECT Url FROM dbo.ServiceLink WHERE SessionID=a.ID)");
            sql.AppendFormat("                      ELSE MessageType END ) ");
            sql.AppendFormat(" FROM ServiceSessionInfo a ");
            sql.AppendFormat(" WHERE a.IsDelete=0 ");
            sql.AppendFormat(" AND a.BusinessID={0} ",BusinessID);
            sql.AppendFormat(" AND a.BusinessType='{0}' ", BusinessType);
            sql.AppendFormat(" ORDER BY a.SendTime ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 更改为再次聊天

        /// <summary>
        /// 更改为再次聊天
        /// </summary>
        /// <param name="ServiceOpenID">客服微信用户标识</param>
        /// <param name="CustomerOpenID">客户微信用户标识</param>
        /// <param name="BusinessType">业务类型目前有5类，1 = 预约保养   2 = 预约维修   3 = 预约试驾   4 = 在线询价   5 = 在线咨询</param>
        /// <param name="BusinessID">业务单号ID</param>
        public void UpdateChatStatus(string ServiceOpenID, string CustomerOpenID, string BusinessType, int BusinessID)
        {   
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ServiceCustomerMapping SET  ");
            sql.AppendFormat("  SessionStatus = 1 ,");
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE ServiceOpenId='{0}' ", ServiceOpenID);
            sql.AppendFormat(" AND CustomerOpenId = '{0}' ", CustomerOpenID);
            sql.AppendFormat(" AND BusinessType = '{0}' ", BusinessType);
            sql.AppendFormat(" AND BusinessID = {0} ", BusinessID);
            sql.AppendFormat(" AND IsDelete = 0 ");
            this.SQLHelper.ExecuteNonQuery(sql.ToString());      
        }
        #endregion

        #region 获取正与客服聊天的客户人数
        /// <summary>
        /// 获取客服聊天的人数
        /// </summary>
        /// <param name="ServiceOpenID">客服微信识别号</param>
        public DataSet GetServiceChatCount(string ServiceOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.ServiceCustomerMapping ");
            sql.AppendFormat(" WHERE ServiceOpenId='{0}' AND SessionStatus=1 AND IsDelete=0", ServiceOpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region 获取正与客户聊天的客服人数
        /// <summary>
        /// 获取正与客户聊天的客服人数
        /// </summary>
        /// <param name="CustomerOpenID">客户微信识别号</param>
        public DataSet GetCustomerChatCount(string CustomerOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.ServiceCustomerMapping ");
            sql.AppendFormat(" WHERE CustomerOpenId='{0}' AND SessionStatus=1 AND IsDelete=0", CustomerOpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

    }
}
