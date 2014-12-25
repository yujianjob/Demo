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
    /// 数据访问： 1402兑换码用户关系表 RedeemCodeCustomer 
    /// 表RedeemCodeCustomer的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class RedeemCodeCustomerDAO : BaseDAO<BasicUserInfo>, ICRUDable<RedeemCodeCustomerEntity>, IQueryable<RedeemCodeCustomerEntity>
    {
        #region 判断兑换用户关系表中是否已经存在数据(根据上级微信用户标识以及下级微信用户标识)

        /// <summary>
        /// 注意：该处会出现，在一个活动上，多个上家分享活动给一个下家，
        /// 这样按照常理是需要将每个上家与该下家的对应关系插入到数据库中。但是，我们这里的处理是，只插入最后一个分享给他上家信息。
        /// 这里引入活动ID，是为了以后做活动统计。
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

        #region 更改上家

        /// <summary>
        /// 更改上家,更改为最后一个给出分享地址的上家微信识别号
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
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

        #region 判断下家是否存在上家(根据下级微信用户标识)

        /// <summary>
        /// 判断下家是否存在上家，只根据下级微信用户标识的意义，因为会出现这样一种情况，当上次失效的活动，该用户是有上家的，
        /// 可是这次注册时，这次活动是没有上家的，在做判断是否该用户有上家？
        /// </summary>
        public DataSet GetCodeCustomerByDownID(string DownOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM RedeemCodeCustomer a ");
            sql.AppendFormat(" WHERE a.DownWxOpenId='{0}' ", DownOpenID);
            sql.AppendFormat(" AND a.IsDelete=0  ");
            //筛选出可以兑换兑换码的且没过期的活动
            sql.AppendFormat(" AND a.EventID in (SELECT b.ID FROM dbo.Event b WHERE (b.ContentScenario='1' or b.ContentScenario='2')  AND b.IsDelete=0 AND b.EndTime>'{0}')", DateTime.Now.AddDays(-1));
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 判断上家获取下家的数量(根据上级微信用户标识)

        /// <summary>
        /// 判断上家获取下家的数量
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

        #region 更改下家的注册状态(通过下家微信识别号场景一)

        /// <summary>
        /// 更改下家的注册状态,如果出现一次兑换码活动中，有多个活动，而一个上家同时分享不同的活动给同一下家，
        /// 而下家都点击了查看，这时候上家与下家在客户关系表中会通过不同的活动而建立多条关系，当下家注册时，我们
        /// 的逻辑时，只更改可以兑换兑换码活动且没过期，且结束时间最大的活动的ID
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateRegisterStatu(string DownOpenID,string UpOpenID)
        {           
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RedeemCodeCustomer SET  ");
            sql.AppendFormat("  RegisterFlag = '1', ");
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE DownWxOpenId='{0}' ", DownOpenID);
            sql.AppendFormat(" AND IsDelete=0 ");
            //筛选出可以兑换兑换码的且没过期的活动
            sql.AppendFormat(" AND EventID = (SELECT TOP 1 b.ID FROM dbo.Event b WHERE b.ContentScenario='1' ");
            sql.AppendFormat(" AND id IN (SELECT a.EventID FROM dbo.RedeemCodeCustomer a WHERE a.DownWxOpenId='{0}' AND a.WxOpenId='{1}') ",DownOpenID,UpOpenID);
            sql.AppendFormat(" AND b.IsDelete=0 AND b.EndTime>'{0}' ORDER BY b.EndTime DESC)", DateTime.Now.AddDays(-1) );
            this.SQLHelper.ExecuteNonQuery(sql.ToString());          
        }

        #endregion

        #region 更改下家的注册状态(通过下家微信识别号场景二)

        /// <summary>
        /// 更改下家的注册状态,如果出现一次兑换码活动中，有多个活动，而一个上家同时分享不同的活动给同一下家，
        /// 而下家都点击了查看，这时候上家与下家在客户关系表中会通过不同的活动而建立多条关系，当下家注册时，我们
        /// 的逻辑时，只更改可以兑换兑换码活动且没过期，且结束时间最大的活动的ID
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateRegisterStatuC2(string DownOpenID, string UpOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RedeemCodeCustomer SET  ");
            sql.AppendFormat("  RegisterFlag = '1', ");
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE DownWxOpenId='{0}' ", DownOpenID);
            sql.AppendFormat(" AND IsDelete=0 ");
            //筛选出可以兑换兑换码的且没过期的活动
            sql.AppendFormat(" AND EventID = (SELECT TOP 1 b.ID FROM dbo.Event b WHERE b.ContentScenario='2' ");
            sql.AppendFormat(" AND id IN (SELECT a.EventID FROM dbo.RedeemCodeCustomer a WHERE a.DownWxOpenId='{0}' AND a.WxOpenId='{1}') ", DownOpenID, UpOpenID);
            sql.AppendFormat(" AND b.IsDelete=0 AND b.EndTime>'{0}' ORDER BY b.EndTime DESC)", DateTime.Now.AddDays(-1));
            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region 获取邀请注册人员信息

        /// <summary>
        /// 获取邀请注册人员信息
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
