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
    /// ���ݷ��ʣ� 1101�ͷ��Ự��Ϣ�� ServiceSessionInfo 
    /// ��ServiceSessionInfo�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ServiceSessionInfoDAO : BaseDAO<BasicUserInfo>, ICRUDable<ServiceSessionInfoEntity>, IQueryable<ServiceSessionInfoEntity>
    {
        #region ��ȡ�����¼(ͨ��ҵ�����ͺͶ�����)

        /// <summary>
        /// ��ȡ�����¼(ͨ��ҵ�����ͺͶ�����)
        /// </summary>
        /// <param name="BusinessType">ҵ������</param>
        /// <param name="BusinessID">������</param>
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

        #region ����Ϊ�ٴ�����

        /// <summary>
        /// ����Ϊ�ٴ�����
        /// </summary>
        /// <param name="ServiceOpenID">�ͷ�΢���û���ʶ</param>
        /// <param name="CustomerOpenID">�ͻ�΢���û���ʶ</param>
        /// <param name="BusinessType">ҵ������Ŀǰ��5�࣬1 = ԤԼ����   2 = ԤԼά��   3 = ԤԼ�Լ�   4 = ����ѯ��   5 = ������ѯ</param>
        /// <param name="BusinessID">ҵ�񵥺�ID</param>
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

        #region ��ȡ����ͷ�����Ŀͻ�����
        /// <summary>
        /// ��ȡ�ͷ����������
        /// </summary>
        /// <param name="ServiceOpenID">�ͷ�΢��ʶ���</param>
        public DataSet GetServiceChatCount(string ServiceOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.ServiceCustomerMapping ");
            sql.AppendFormat(" WHERE ServiceOpenId='{0}' AND SessionStatus=1 AND IsDelete=0", ServiceOpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region ��ȡ����ͻ�����Ŀͷ�����
        /// <summary>
        /// ��ȡ����ͻ�����Ŀͷ�����
        /// </summary>
        /// <param name="CustomerOpenID">�ͻ�΢��ʶ���</param>
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
