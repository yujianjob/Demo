/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/9 20:13:46
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
    /// ���ݷ��ʣ� 0202������� EventApply 
    /// ��EventApply�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class EventApplyDAO : BaseDAO<BasicUserInfo>, ICRUDable<EventApplyEntity>, IQueryable<EventApplyEntity>
    {
        #region ���ݻID��ȡ������Ϣ

        /// <summary>
        /// ���ݻID��ȡ������Ϣ
        /// </summary>
        /// <param name="eventId">�ID</param>
        public DataSet GetEventApplyByEventId(int eventId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT OptionValue FROM dbo.EventApply WHERE EventID = {0} AND IsDelete = 0", eventId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ�г������������Ϣ

        /// <summary>
        /// ��ȡ�г������������Ϣ
        /// </summary>
        /// <param name="EventID">�ID</param>
        /// <returns></returns>
        public DataSet GetMarketEventApplyInfo(int EventID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT  OptionValue=a.value , ");
            sql.AppendFormat(" ISNULL(b.Isshow,0) AS IsShow ");//������������ֵΪnull�����Ϊ0
            sql.AppendFormat(" FROM    dbo.BasicData AS a ");
            sql.AppendFormat("  LEFT JOIN ( SELECT  OptionValue ,Isshow = 1 ");//Ĭ�ϻ�������������ľ��屨��ѡ��ֵ��ΪĬ����ǰ����Ҫչʾ��Isshow = 1��
            sql.AppendFormat(" FROM    EventApply ");
            sql.AppendFormat("  WHERE   EventID = {0} ",EventID);
            sql.AppendFormat(" AND IsDelete = 0 ) AS b ON a.Value = b.OptionValue ");
            sql.AppendFormat(" WHERE   a.TypeCode = 'EventApplyOption' AND a.IsDelete = 0 ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ�����ID(�ID������ѡ��)

        /// <summary>
        /// ��ȡ�����
        /// </summary>
        /// <param name="EventID">�ID</param>
        /// <param name="OptionValue">����ѡ��ֵ</param>
        /// <returns></returns>
        public int GetIdByEventIdOption(int EventID, string OptionValue)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ID FROM EventApply a ");
            sql.AppendFormat(" WHERE a.EventID={0} ", EventID);
            sql.AppendFormat(" AND a.OptionValue='{0}' ", OptionValue);
            sql.AppendFormat(" AND a.IsDelete=0 ");
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion
    }
}
