using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using YuJian.WeiXin.BLL;
using YuJian.WeiXin.Entity;
using Yunchee.Volkswagen.BLL.Weixin.Common;
using Yunchee.Volkswagen.Common.Const;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using Yunchee.Volkswagen.Utility.Log;

namespace Yujian.WebService.WeiXin
{
    public partial class AuthFeedBack : System.Web.UI.Page
    {
        public BasicUserInfo loginInfo = new BasicUserInfo();
        public string weixinId = string.Empty;
        public string goUrl = string.Empty;
        public string goUrlTemp = ConfigurationManager.AppSettings["DomainShort"];
        public string authCode = string.Empty;
        public string strAppId = string.Empty;
        public string strAppSecret = string.Empty;
        public string strState = string.Empty;
        // OAuth认证地址
        public string strRedirectUrl = ConfigurationManager.AppSettings["Domain"] + "Weixin/AuthFeedBack.aspx";
        // 未关注时跳转关注页面
        public string strNoFollowUrl = ConfigurationManager.AppSettings["Domain"] + "PhonePage/html/no_attention.html";
        //// 如果是粉丝，跳转到注册页面
        //public string strRegisterUrl = ConfigurationManager.AppSettings["Domain"] + "PhonePage/html/choose_role.html";
        //// 潜客无权访问车主页面
        //public string strNoAccessUrl = ConfigurationManager.AppSettings["Domain"] + "PhonePage/html/no_access.html";
        //// 员工访问页面
        //public string strEmployeeUrl = ConfigurationManager.AppSettings["Domain"] + "PhonePage/html/customer_index.html";

        protected void Page_Load(object sender, EventArgs e)
        {
            Loggers.Debug(new DebugLogInfo()
            {
                Message = string.Format("进入认证界面: {0}", goUrl)
            });

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["state"]))
                {
                    #region 第二次请求该界面
                    string state = Request["state"];
                    try
                    {
                        Loggers.Debug(new DebugLogInfo()
                        {
                            Message = string.Format("strState2: {0}", state)
                        });

                        byte[] buff1 = Convert.FromBase64String(state);
                        state = Encoding.UTF8.GetString(buff1);
                        state = HttpUtility.UrlDecode(state, Encoding.UTF8);
                        string[] array = state.Split(',');
                        weixinId = array[0];
                        goUrl = array[1];
                        goUrl = goUrlTemp + goUrl;
                        authCode = array[2];

                        Loggers.Debug(new DebugLogInfo()
                        {
                            Message = string.Format("goUrl: {0}", goUrl)
                        });
                    }
                    catch (Exception ex)
                    {
                        Response.Write("state错误:" + ex.ToString());
                    }
                    #endregion
                }
                else
                {
                    #region 第一次请求该界面，解析参数

                    if (!string.IsNullOrEmpty(Request["weixinId"]))
                    {
                        weixinId = Request["weixinId"];
                    }
                    else
                    {
                        Response.Write("没有获取weixinId");
                    }

                    if (!string.IsNullOrEmpty(Request["goUrl"]))
                    {
                        goUrl = Request["goUrl"];
                        Response.Write("goUrl:" + goUrl);
                    }
                    else
                    {
                        Response.Write("没有获取goUrl");
                        Response.End();
                    }

                    #endregion
                }

                Response.Write("goUrl:" + goUrl);

                loginInfo = new BasicUserInfo() { ClientID = 1, UserID = 1 };
                //获取微信基本信息
                GetKeyByApp();
                try
                {
                    //微信认证 第一步：用户同意授权，获取code
                    string code = Request["code"];
                    if (string.IsNullOrEmpty(code))
                    {
                        authCode = Request["autoCode"];
                        strState = weixinId + "," + goUrl + "," + authCode;
                        strState = HttpUtility.UrlEncode(strState, Encoding.UTF8);
                        byte[] buff = Encoding.UTF8.GetBytes(strState);
                        strState = Convert.ToBase64String(buff);

                        Loggers.Debug(new DebugLogInfo()
                        {
                            Message = string.Format("压缩64位码: {0}", strState)
                        });

                        //第一步获取code
                        GetOAuthCode(strState);
                    }
                    else
                    {
                        //第二部获取token
                        GetAccessToken(code);
                    }
                }
                catch (Exception ex)
                {
                    Loggers.Debug(new DebugLogInfo()
                    {
                        Message = string.Format("OAuthWX初始化: {0}", ex.ToString())
                    });
                }
            }
        }

        #region 获取微信信息

        public void GetKeyByApp()
        {
            WApplicationBLL server = new WApplicationBLL(loginInfo);
            var applicationList = server.QueryByEntity(new WApplicationEntity { WeixinID = weixinId }, null);

            if (applicationList != null && applicationList.Length > 0)
            {
                var info = applicationList.FirstOrDefault();

                strAppId = info.AppID;
                strAppSecret = info.AppSecret;
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("APPID： {0}，AppSecret：{1}", strAppId, strAppSecret)
                });
            }
            else
            {
                Response.Write("不存在对应的微信标识");
            }
        }

        #endregion

        #region 第一步 获取code
        /// <summary>
        /// 获取code
        /// </summary>
        private void GetOAuthCode(string stateTmp)
        {
            try
            {
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("stateTmp: {0}", stateTmp)
                });

                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + strAppId + "&redirect_uri=" + HttpUtility.UrlEncode(strRedirectUrl) + "&response_type=code&scope=snsapi_userinfo&state=" + stateTmp + "#wechat_redirect";

                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("GetOAuthCode 出错: {0}", ex.ToString())
                });
            }
        }

        #endregion

        #region 第二步 获取Access Token
        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="code"></param>
        private void GetAccessToken(string code)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token";
                //https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
                WebClient myWebClient = new WebClient();
                // 注意这种拼字符串的ContentType
                myWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                // 转化成二进制数组
                var postData = "appid=" + strAppId + "&secret=" + strAppSecret + "&code=" + code + "&grant_type=authorization_code";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // 上传数据，并获取返回的二进制数据.
                byte[] responseArray = myWebClient.UploadData(url, "POST", byteArray);
                var data = System.Text.Encoding.UTF8.GetString(responseArray);
                var tokenInfo = data.DeserializeJSONTo<AccessTokenReturn>();

                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("GetAccessToken: {0}", data)
                });

                if (tokenInfo != null)
                {
                    if (string.IsNullOrEmpty(tokenInfo.openid))
                    {
                        Response.Redirect(strNoFollowUrl);
                    }
                    else
                    {
                        GetUserIdByOpenId(tokenInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("GetAccessToken错误: {0}", ex.ToString())
                });
            }
        }

        public class AccessTokenReturn
        {
            public string access_token { get; set; }    //网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
            public string expires_in { get; set; }      //access_token接口调用凭证超时时间，单位（秒）
            public string refresh_token { get; set; }   //用户刷新access_token
            public string openid { get; set; }          //用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
            public string scope { get; set; }           //用户授权的作用域，使用逗号（,）分隔
        }

        #endregion

        #region 第三部 获取用户标识
        public void GetUserIdByOpenId(AccessTokenReturn tokenInfo)
        {
            try
            {
                string openId=tokenInfo.openid;
                CustomerBLL server = new CustomerBLL(loginInfo);
                var customerList = server.QueryByEntity(new CustomerEntity
                {
                    WxOpenId = openId,
                    SubscribeStatus = C_SubscribeStatus.Attention
                }, null);

                //if (customerList != null && customerList.Length > 0)
                //{
                Response.Write("openId:" + openId);
                //Loggers.Debug(new DebugLogInfo()
                //{
                //    Message = string.Format("customerList.FirstOrDefault().Type: {0}", customerList.FirstOrDefault().Type)
                //});

                var customer = customerList.FirstOrDefault();

                //if (customer.Type.Equals(C_CustomerType.FANS) || customer.Type.Equals(C_CustomerType.POTENTIAL))
                //{
                //    var redirectUrl = goUrl.Substring(goUrlTemp.Length);
                //    Loggers.Debug(new DebugLogInfo()
                //    {
                //        Message = string.Format("redirectUrl: {0}", redirectUrl)
                //    });

                //    switch (redirectUrl)
                //    {
                //        case "PhonePage/html/after_sales_privilege.html":
                //        case "PhonePage/html/after_upkeep.html":
                //        case "PhonePage/html/after_maintain.html":
                //        case "PhonePage/html/after_emergency_rescue.html":
                //        case "PhonePage/html/break_rule_search.html":
                //            goUrl = strNoAccessUrl
                //                + "?openId=" + openId
                //                + "&weixinId=" + weixinId
                //                + "&rid=" + new Random().Next(1000, 10000);
                //            break;
                //        default:
                //            goUrl = "http://" + HttpUtility.UrlDecode(goUrl)
                //                + "?openId=" + openId
                //                + "&weixinId=" + weixinId
                //                + "&rid=" + new Random().Next(1000, 10000);
                //            break;
                //    }
                //}
                //else if (customer.Type.Equals(C_CustomerType.OWNER))
                //{
                //    Loggers.Debug(new DebugLogInfo()
                //    {
                //        Message = string.Format("车主")
                //    });

                //    goUrl = "http://" + HttpUtility.UrlDecode(goUrl)
                //        + "?openId=" + openId
                //        + "&weixinId=" + weixinId
                //        + "&rid=" + new Random().Next(1000, 10000);
                //}
                //else if (customer.Type.Equals(C_CustomerType.EMPLOYEE))
                //{
                //    Loggers.Debug(new DebugLogInfo()
                //    {
                //        Message = string.Format("员工")
                //    });

                //    goUrl = strEmployeeUrl
                //        + "?openId=" + openId
                //        + "&weixinId=" + weixinId
                //        + "&rid=" + new Random().Next(1000, 10000);
                //}
                var ip = CommonUtils.GetHostAddress();
                var visit = new VisitIPEntity();
                visit.Openid = openId;
                visit.IPAddress = ip;
                new VisitIPBLL(new BasicUserInfo()).Create(visit);

                var user_info = new CommonBLL().GetAuthUserInfo(tokenInfo.access_token, openId);
                CustomerEntity entity;
                var customer_list = new CustomerBLL(new BasicUserInfo()).QueryByEntity(
                    new CustomerEntity() { WxId = weixinId, WxOpenId = user_info.openid }, null);//客户列表
                var wapplication_list = new WApplicationBLL(new BasicUserInfo()).QueryByEntity(
                    new WApplicationEntity() { WeixinID = weixinId, IsDelete = 0 }, null);//公众账号表
                #region 操作客户
                string img_url = string.Empty;
                if (!string.IsNullOrEmpty(user_info.headimgurl))
                    img_url = CommonUtils.UpLoadHeadImg(user_info.headimgurl, weixinId);

                if (customer_list != null && customer_list.Length > 0)
                {
                    entity = customer_list.FirstOrDefault();
                    CommonUtils.DeleteHeadImg(entity.WxHeadImgUrl);
                    entity.WxNickName = user_info.nickname;
                    entity.WxSex = user_info.sex;
                    entity.WxCity = user_info.city;
                    entity.WxCountry = user_info.country;
                    entity.WxProvince = user_info.province;
                    entity.WxLanguage = user_info.language;
                    entity.WxHeadImgUrl = img_url;
                    entity.SubscribeStatus = "1";
                    entity.LastUpdateTime = DateTime.Now;
                    new CustomerBLL(new BasicUserInfo()).Update(entity);
                }
                else
                {
                    entity = new CustomerEntity();
                    entity.WxId = weixinId;
                    entity.WxOpenId = user_info.openid;
                    entity.WxNickName = user_info.nickname;
                    entity.WxSex = user_info.sex;
                    entity.WxCity = user_info.city;
                    entity.WxCountry = user_info.country;
                    entity.WxProvince = user_info.province;
                    entity.WxLanguage = user_info.language;
                    entity.WxHeadImgUrl = img_url;
                    entity.SubscribeStatus = "1";
                    entity.LastUpdateTime = DateTime.Now;
                    new CustomerBLL(new BasicUserInfo()).Create(entity);
                }
                #endregion

                //var firstLogin = new QualificationBLL(new BasicUserInfo()).QueryByEntity(new QualificationEntity() { Type = 1, WxOpenID = openId }, null);
                //if (firstLogin.Length == 0)
                //{
                //    var Qentity = new QualificationEntity();
                //    Qentity.WxID = "gh_0846d6c7c2b8";
                //    Qentity.WxOpenID = openId;
                //    Qentity.Type = 1;
                //    Qentity.EnableFlag = 1;
                //    new QualificationBLL(new BasicUserInfo()).Create(Qentity);
                //}

                goUrl = "http://tianrui.efic.com.cn/index.html" 
                        + "?openId=" + openId
                        + "&weixinId=" + weixinId
                        + "&rid=" + new Random().Next(1000, 10000);
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("goUrl: {0}", goUrl)
                });

                Response.Redirect(goUrl);
                //}
                //else
                //{
                //    //用户不存在或者取消关注，请处理
                //    Response.Redirect(strNoFollowUrl);
                //}
            }
            catch (Exception ex)
            {
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("GetUserIdByOpenId获取用户标识: {0}", ex.ToString())
                });

                Response.Write("GetUserIdByOpenId获取用户标识:" + ex.ToString());
            }
        }
        #endregion
    }
}