using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Yunchee.Volkswagen.Entity.Weixin;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.Log;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using Yunchee.Volkswagen.DataAccess;
using System.Data;
using Yunchee.Volkswagen.BLL.Weixin.Enum;
using System.Net;
using System.IO;
using System.Globalization;
using Yunchee.Volkswagen.Entity;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Yunchee.Volkswagen.Utility.DataAccess;
using YuJian.WeiXin.DataAccess;
using YuJian.WeiXin.Entity;
namespace Yunchee.Volkswagen.BLL.Weixin.Common
{
    /// <summary>
    /// 微信公共类
    /// </summary>
    public class CommonBLL
    {
        public static string Host = ConfigurationManager.AppSettings["Host"].ToString();
        #region 构造函数

        public CommonBLL() { }

        #endregion

        #region 验证token

        /// <summary>
        /// 验证token
        /// </summary>
        public void ValidToken(HttpContext httpContext, string token)
        {
            Loggers.Debug(new DebugLogInfo { Message = "开始执行token验证" });

            if (httpContext.Request["echoStr"] != null)
            {
                var echostr = httpContext.Request["echoStr"].ToString();

                Loggers.Debug(new DebugLogInfo { Message = "echoStr = " + echostr });

                if (CheckSignature(httpContext, token) && !string.IsNullOrEmpty(echostr))
                {
                    Loggers.Debug(new DebugLogInfo { Message = "结束执行token验证" });

                    //推送...不然微信平台无法验证token
                    httpContext.Response.Write(echostr);
                }
            }
            else
            {
                Loggers.Debug(new DebugLogInfo { Message = "echoStr is null" });
            }
        }

        /// <summary>
        /// 加密/校验流程：
        /// 1. 将token、timestamp、nonce三个参数进行字典序排序
        /// 2. 将三个参数字符串拼接成一个字符串进行sha1加密
        /// 3. 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信
        /// </summary>
        /// <returns></returns>
        private bool CheckSignature(HttpContext httpContext, string token)
        {
            var signature = httpContext.Request["signature"].ToString();
            var timestamp = httpContext.Request["timestamp"].ToString();
            var nonce = httpContext.Request["nonce"].ToString();

            Loggers.Debug(new DebugLogInfo { Message = "token = " + token });
            Loggers.Debug(new DebugLogInfo { Message = "加密前的 signature = " + signature });

            //字典排序
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);

            //sha1加密
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            Loggers.Debug(new DebugLogInfo { Message = "加密后的 signature = " + tmpStr });

            if (tmpStr == signature)
            {
                Loggers.Debug(new DebugLogInfo { Message = "token验证成功" });
                return true;
            }
            else
            {
                Loggers.Debug(new DebugLogInfo { Message = "token验证失败" });
                return false;
            }
        }

        #endregion

        #region 获取凭证接口

        /// <summary>
        /// 在使用通用接口前，你需要做以下两步工作:
        /// 1.拥有一个微信公众账号，并获取到appid和appsecret
        /// 2.通过获取凭证接口获取到access_token
        /// access_token是第三方访问微信公众平台api资源的票据。
        /// </summary>
        /// <param name="appID">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="weixinID">微信ID</param>
        /// <returns></returns>
        public AccessTokenEntity GetAccessToken(string appID, string appSecret, string weixinID)
        {
            if (HttpContext.Current != null)
            {
                CommonUtils.WriteLogWeixin("获取凭证接口： ", weixinID);
                CommonUtils.WriteLogWeixin("appID： " + appID, weixinID);
                CommonUtils.WriteLogWeixin("appSecret： " + appSecret, weixinID);
            }

            var appService = new WApplicationDAO(new BasicUserInfo() { ClientID = 1, UserID = 1 });
            var app = appService.QueryByEntity(new WApplicationEntity() { AppID = appID, AppSecret = appSecret, WeixinID = weixinID }, null);
            if ((string.IsNullOrEmpty(app[0].AccessToken) || app[0].ExpirationTime == null) || app[0].ExpirationTime < appService.GetSqlServerTime())
            {
                string uri = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appID + "&secret=" + appSecret;
                string method = "GET";
                string data = CommonUtils.GetRemoteData(uri, method, string.Empty);
                if (HttpContext.Current != null)
                    CommonUtils.WriteLogWeixin("调用获取凭证接口返回值： " + data, weixinID);

                var accessToken = data.DeserializeJSONTo<AccessTokenEntity>();
                app[0].AccessToken = accessToken.access_token;
                app[0].ExpirationTime = DateTime.Now.AddHours(1);
                appService.Update(app[0]);
                return accessToken;
            }
            else
            {
                AccessTokenEntity entity = new AccessTokenEntity();
                entity.access_token = app[0].AccessToken;
                entity.expires_in = "";
                entity.errcode = "";
                entity.errmsg = "";
                return entity;
            }
        }

        #endregion

        #region 获取用户信息接口

        /// <summary>
        /// 第三方通过openid获取用户信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="openID">普通用户的标识，对当前公众号唯一</param>
        /// <returns></returns>
        public Yunchee.Volkswagen.Entity.Weixin.UserInfoEntity GetUserInfo(string accessToken, string openID)
        {
            string uri = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + accessToken + "&openid=" + openID;
            string method = "GET";
            string data = CommonUtils.GetRemoteData(uri, method, string.Empty);

            var userInfo = data.DeserializeJSONTo<Yunchee.Volkswagen.Entity.Weixin.UserInfoEntity>();
            return userInfo;
        }

        #endregion

        #region 获取用户信息接口

        /// <summary>
        /// 第三方通过openid获取用户信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="openID">普通用户的标识，对当前公众号唯一</param>
        /// <returns></returns>
        public Yunchee.Volkswagen.Entity.Weixin.UserInfoEntity GetAuthUserInfo(string accessToken, string openID)
        {
            string uri = "https://api.weixin.qq.com/sns/userinfo?access_token=" + accessToken + "&openid=" + openID + "&lang=zh_CN";
            string method = "GET";
            string data = CommonUtils.GetRemoteData(uri, method, string.Empty);

            var userInfo = data.DeserializeJSONTo<Yunchee.Volkswagen.Entity.Weixin.UserInfoEntity>();
            return userInfo;
        }

        #endregion

        #region 回复图文素材

        /// <summary>
        /// 回复图文素材
        /// </summary>
        /// <param name="weixinID">开发者微信号</param>
        /// <param name="openID">接收方帐号（收到的OpenID）</param>
        /// <param name="newsList">图文素材实体类集合</param>
        public void ResponseNewsMessage(string weixinID, string openID, List<Yunchee.Volkswagen.BLL.Weixin.Base.BaseBLL.WNewsEntity> newsList, HttpContext httpContext)
        {
            if (newsList != null && newsList.Count > 0)
            {
                var response = "<xml>";
                response += "<ToUserName><![CDATA[" + openID + "]]></ToUserName>";
                response += "<FromUserName><![CDATA[" + weixinID + "]]></FromUserName>";
                response += "<CreateTime>" + CommonUtils.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";
                response += "<MsgType><![CDATA[news]]></MsgType>";
                response += "<ArticleCount>" + newsList.Count + "</ArticleCount>";
                response += "<Articles>";

                foreach (var item in newsList)
                {
                    response += "<item>";
                    response += "<Title><![CDATA[" + item.Title + "]]></Title> ";
                    response += "<Description><![CDATA[" + item.Description + "]]></Description>";
                    response += "<PicUrl><![CDATA[" + item.PictureUrl + "]]></PicUrl>";
                    response += "<Url><![CDATA[" + item.OriginalUrl + "]]></Url>";
                    response += "</item>";
                }

                response += "</Articles>";
                response += "<FuncFlag>1</FuncFlag>";
                response += "</xml>";

                CommonUtils.WriteLogWeixin("公众平台返回给用户的图文素材:  " + response, weixinID);
                CommonUtils.WriteLogWeixin("回复图文素材结束-------------------------------------------\n", weixinID);

                httpContext.Response.Write(response);
            }
        }

        #endregion

        
        
    }
}
