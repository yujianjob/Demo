using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;
using YuJian.WeiXin.BLL;
using YuJian.WeiXin.Entity;
using Yunchee.Volkswagen.BLL.Weixin.Base;
using Yunchee.Volkswagen.BLL.Weixin.Const;
using Yunchee.Volkswagen.BLL.Weixin.Enum;
using Yunchee.Volkswagen.Common.Const;
using Yunchee.Volkswagen.Common.Enum;
using Yunchee.Volkswagen.DataAccess;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Entity.Weixin;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess.Query;

namespace Yunchee.Volkswagen.BLL.Weixin.Common
{
    /// <summary>
    /// 微信相关操作
    /// </summary>
    public class WeiXinBLL
    {
        #region 变量
        protected BasicUserInfo loginInfo = null;
        protected CommonBLL commonService = null;
        protected HttpContext httpContext = null;
        protected RequestParams requestParams = null;
        protected CustomerBLL customerService = null;
        protected WApplicationBLL applicationService = null;
       
        protected BasicDataBLL basicDataBLL = null;
        
        #endregion
        #region 构造函数

        public WeiXinBLL(HttpContext httpContext, RequestParams requestParams)
        {
            this.httpContext = httpContext;
            this.requestParams = requestParams;
            loginInfo = new BasicUserInfo() { ClientID = 1, UserID = 1 };
            commonService = new CommonBLL();
            customerService = new CustomerBLL(loginInfo);
            applicationService = new WApplicationBLL(loginInfo);
            basicDataBLL = new BasicDataBLL(loginInfo);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 关注与取消事件
        /// </summary>
        /// <param name="user_info">客户信息</param>
        /// <param name="requestParams">请求信息</param>
        /// <param name="event_type">1关注0取消</param>
        public void SubscribeEvent(Yunchee.Volkswagen.Entity.Weixin.UserInfoEntity user_info, RequestParams requestParams, int event_type)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
            {
                CustomerEntity entity;
                var customer_list = customerService.QueryByEntity(
                    new CustomerEntity() { WxId = requestParams.WeixinId, WxOpenId = user_info.openid }, null);//客户列表
                var wapplication_list = applicationService.QueryByEntity(
                    new WApplicationEntity() { WeixinID = requestParams.WeixinId, IsDelete = 0 }, null);//公众账号表

                if (event_type == 1)//关注
                {
                    #region 操作客户
                    string img_url = string.Empty;
                    if (!string.IsNullOrEmpty(user_info.headimgurl))
                        img_url = CommonUtils.UpLoadHeadImg(user_info.headimgurl, requestParams.WeixinId);

                    if (customer_list != null && customer_list.Length > 0)
                    {
                        entity = customer_list.FirstOrDefault();
                        CommonUtils.DeleteHeadImg(entity.WxHeadImgUrl);
                        entity.WxNickName = user_info.nickname;
                        entity.WxSex = user_info.sex;
                        entity.WxCity = user_info.city;
                        entity.WxCountry = user_info.country;
                        entity.WxProvince =  user_info.province;
                        entity.WxLanguage = user_info.language;
                        entity.WxHeadImgUrl = img_url;
                        entity.WxSubscribeTime = requestParams.XmlNode.SelectSingleNode("//CreateTime").InnerText.Trim();
                        entity.SubscribeStatus = "1";
                        entity.LastUpdateTime = DateTime.Now;
                        customerService.Update(entity);
                    }
                    else
                    {
                        entity = new CustomerEntity();
                        entity.WxId = requestParams.WeixinId;
                        entity.WxOpenId = user_info.openid;
                        entity.WxNickName = user_info.nickname;
                        entity.WxSex = user_info.sex;
                        entity.WxCity = user_info.city;
                        entity.WxCountry = user_info.country;
                        entity.WxProvince =  user_info.province;
                        entity.WxLanguage = user_info.language;
                        entity.WxHeadImgUrl = img_url;
                        entity.WxSubscribeTime = requestParams.XmlNode.SelectSingleNode("//CreateTime").InnerText.Trim();

                        entity.SubscribeStatus = "1";
                        entity.LastUpdateTime = DateTime.Now;
                        customerService.Create(entity);
                    }
                    #endregion
                }
                else//取消
                {
                    #region 操作客户
                    if (customer_list != null && customer_list.Length > 0)
                    {
                        CommonUtils.WriteLogWeixin("用户取消关注微信号,更新状态", requestParams.WeixinId);
                        entity = customer_list.FirstOrDefault();
                        entity.SubscribeStatus = "2";
                        entity.UnSubscribeDate = DateTime.Now;
                        entity.LastUpdateTime = DateTime.Now;
                        customerService.Update(entity);
                    }
                    #endregion
                }
                scope.Complete();
            }

        }

        /// <summary>
        /// 保存或更新客户信息
        /// </summary>
        /// <param name="type">1关注0取消</param>
        public void SaveOrUpdateCustomer(int type)
        {
            var appEntity = applicationService.QueryByEntity(
                new WApplicationEntity() { WeixinID = requestParams.WeixinId }, null);
            if (appEntity != null && appEntity.Length > 0)
            {
                AccessTokenEntity ACCESS_TOKEN = commonService.GetAccessToken(appEntity[0].AppID, appEntity[0].AppSecret, appEntity[0].WeixinID);
                SubscribeEvent(commonService.GetUserInfo(ACCESS_TOKEN.access_token, requestParams.OpenId), requestParams, type);
            }
        }
        #endregion

    }
}
