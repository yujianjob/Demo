/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/30 15:11:04
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
    /// ���ݷ��ʣ� 1402�һ����û���ϵ�� RedeemCodeCustomer 
    /// ��RedeemCodeCustomer�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class RedeemCodeCustomerDAO : BaseDAO<BasicUserInfo>, ICRUDable<RedeemCodeCustomerEntity>, IQueryable<RedeemCodeCustomerEntity>
    {
        #region �ж϶һ��û���ϵ�����Ƿ��Ѿ���������(�����ϼ�΢���û���ʶ�Լ��¼�΢���û���ʶ)

        /// <summary>
        /// ע�⣺�ô�����֣���һ����ϣ�����ϼҷ�����һ���¼ң�
        /// �������ճ�������Ҫ��ÿ���ϼ�����¼ҵĶ�Ӧ��ϵ���뵽���ݿ��С����ǣ���������Ĵ����ǣ�ֻ�������һ����������ϼ���Ϣ��
        /// ��������ID����Ϊ���Ժ����ͳ�ơ�
        /// </summary>
        public int GetCodeCustomerCount(string DownOpenID,int EventID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT COUNT(*) FROM RedeemCodeCustomer a ");
            sql.AppendFormat(" WHERE a.IsDelete=0  ");
            sql.AppendFormat(" AND a.DownWxOpenId='{0}' ", DownOpenID);
            sql.AppendFormat(" AND a.EventID={0} ", EventID);
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region �����ϼ�

        /// <summary>
        /// �����ϼ�,����Ϊ���һ�����������ַ���ϼ�΢��ʶ���
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateUpOpenID(string DownOpenID,string OpenID,int EventID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RedeemCodeCustomer SET  ");
            sql.AppendFormat("  WxOpenId = '{0}', ", DownOpenID);
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE DownWxOpenId='{0}' ", OpenID);
            sql.AppendFormat(" AND IsDelete=0 ");
            sql.AppendFormat(" AND EventID = {0} ", EventID);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region �ж��¼��Ƿ�����ϼ�(�����¼�΢���û���ʶ)

        /// <summary>
        /// �ж��¼��Ƿ�����ϼң�ֻ�����¼�΢���û���ʶ�����壬��Ϊ���������һ����������ϴ�ʧЧ�Ļ�����û������ϼҵģ�
        /// �������ע��ʱ����λ��û���ϼҵģ������ж��Ƿ���û����ϼң�
        /// </summary>
        public DataSet GetCodeCustomerByDownID(string DownOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM RedeemCodeCustomer a ");
            sql.AppendFormat(" WHERE a.DownWxOpenId='{0}' ", DownOpenID);
            sql.AppendFormat(" AND a.IsDelete=0  ");
            //ɸѡ�����Զһ��һ������û���ڵĻ
            sql.AppendFormat(" AND a.EventID in (SELECT b.ID FROM dbo.Event b WHERE (b.ContentScenario='1' or b.ContentScenario='2')  AND b.IsDelete=0 AND b.EndTime>'{0}')", DateTime.Now.AddDays(-1));
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region �ж��ϼһ�ȡ�¼ҵ�����(�����ϼ�΢���û���ʶ)

        /// <summary>
        /// �ж��ϼһ�ȡ�¼ҵ�����
        /// </summary>
        public int GetCountByOpenID(string OpenID,int EventID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT COUNT(*) FROM RedeemCodeCustomer a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' ", OpenID);
            sql.AppendFormat(" AND a.IsDelete=0 AND a.RegisterFlag='1' ");
            sql.AppendFormat(" AND a.EventID ={0} ", EventID);
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region �����¼ҵ�ע��״̬(ͨ���¼�΢��ʶ��ų���һ)

        /// <summary>
        /// �����¼ҵ�ע��״̬,�������һ�ζһ����У��ж�������һ���ϼ�ͬʱ����ͬ�Ļ��ͬһ�¼ң�
        /// ���¼Ҷ�����˲鿴����ʱ���ϼ����¼��ڿͻ���ϵ���л�ͨ����ͬ�Ļ������������ϵ�����¼�ע��ʱ������
        /// ���߼�ʱ��ֻ���Ŀ��Զһ��һ�����û���ڣ��ҽ���ʱ�����Ļ��ID
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateRegisterStatu(string DownOpenID,string UpOpenID)
        {           
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RedeemCodeCustomer SET  ");
            sql.AppendFormat("  RegisterFlag = '1', ");
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE DownWxOpenId='{0}' ", DownOpenID);
            sql.AppendFormat(" AND IsDelete=0 ");
            //ɸѡ�����Զһ��һ������û���ڵĻ
            sql.AppendFormat(" AND EventID = (SELECT TOP 1 b.ID FROM dbo.Event b WHERE b.ContentScenario='1' ");
            sql.AppendFormat(" AND id IN (SELECT a.EventID FROM dbo.RedeemCodeCustomer a WHERE a.DownWxOpenId='{0}' AND a.WxOpenId='{1}') ",DownOpenID,UpOpenID);
            sql.AppendFormat(" AND b.IsDelete=0 AND b.EndTime>'{0}' ORDER BY b.EndTime DESC)", DateTime.Now.AddDays(-1) );
            this.SQLHelper.ExecuteNonQuery(sql.ToString());          
        }

        #endregion

        #region �����¼ҵ�ע��״̬(ͨ���¼�΢��ʶ��ų�����)

        /// <summary>
        /// �����¼ҵ�ע��״̬,�������һ�ζһ����У��ж�������һ���ϼ�ͬʱ����ͬ�Ļ��ͬһ�¼ң�
        /// ���¼Ҷ�����˲鿴����ʱ���ϼ����¼��ڿͻ���ϵ���л�ͨ����ͬ�Ļ������������ϵ�����¼�ע��ʱ������
        /// ���߼�ʱ��ֻ���Ŀ��Զһ��һ�����û���ڣ��ҽ���ʱ�����Ļ��ID
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateRegisterStatuC2(string DownOpenID, string UpOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RedeemCodeCustomer SET  ");
            sql.AppendFormat("  RegisterFlag = '1', ");
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE DownWxOpenId='{0}' ", DownOpenID);
            sql.AppendFormat(" AND IsDelete=0 ");
            //ɸѡ�����Զһ��һ������û���ڵĻ
            sql.AppendFormat(" AND EventID = (SELECT TOP 1 b.ID FROM dbo.Event b WHERE b.ContentScenario='2' ");
            sql.AppendFormat(" AND id IN (SELECT a.EventID FROM dbo.RedeemCodeCustomer a WHERE a.DownWxOpenId='{0}' AND a.WxOpenId='{1}') ", DownOpenID, UpOpenID);
            sql.AppendFormat(" AND b.IsDelete=0 AND b.EndTime>'{0}' ORDER BY b.EndTime DESC)", DateTime.Now.AddDays(-1));
            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region ��ȡ����ע����Ա��Ϣ

        /// <summary>
        /// ��ȡ����ע����Ա��Ϣ
        /// </summary>
        public DataSet GetInviteRegisterList(string OpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat("  SELECT MemberCount=COUNT(EventID),EventID,EventName=(SELECT Title FROM dbo.Event WHERE id=EventID) FROM dbo.RedeemCodeCustomer GROUP BY EventID,WxOpenId HAVING WxOpenId='{0}' ", OpenID);
            sql.AppendFormat(" SELECT WxNickName=WxNickName,CreatTime=CONVERT(varchar(100),c.LastUpdateTime, 120),WxHeadImgUrl,EventID  FROM ");
            sql.AppendFormat(" (SELECT WxNickName,WxHeadImgUrl,WxOpenId FROM dbo.Customer a WHERE WxOpenId IN  ");
            sql.AppendFormat(" (SELECT DownWxOpenId FROM dbo.RedeemCodeCustomer  WHERE WxOpenId='{0}' AND IsDelete=0 AND RegisterFlag='1') ",OpenID);
            sql.AppendFormat(" AND a.IsDelete=0) b,dbo.RedeemCodeCustomer c WHERE c.WxOpenId='{0}' AND c.DownWxOpenId=b.WxOpenId ",OpenID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

    }
}
