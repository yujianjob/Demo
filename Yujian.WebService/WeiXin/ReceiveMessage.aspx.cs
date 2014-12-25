using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using YuJian.WeiXin.BLL;

using YuJian.WeiXin.Entity;
using Yunchee.Volkswagen.BLL.Weixin.Base;
using Yunchee.Volkswagen.BLL.Weixin.Common;
using Yunchee.Volkswagen.BLL.Weixin.Factory;
using Yunchee.Volkswagen.BLL.Weixin.Interface;
using Yunchee.Volkswagen.Common.Const;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Entity.Weixin;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.Log;

namespace Yujian.WebService.WeiXin
{
    public partial class ReceiveMessage : System.Web.UI.Page
    {
        public const string TOKEN = "TerrakeSH";

        #region PageLoad

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region HTTP请求入口

        public override void ProcessRequest(HttpContext param_context)
        {
            try
            {
                var httpContext = param_context;

                if (!string.IsNullOrEmpty(httpContext.Request["echoStr"]))
                {
                    //用于进行微信平台token验证
                    new CommonBLL().ValidToken(httpContext, TOKEN);
                }

                if (httpContext.Request.HttpMethod.ToLower() == "post")
                {
                    //把HTTP请求转换为字符串
                    string postStr = CommonUtils.ConvertHttpContextToString(httpContext);

                    if (!string.IsNullOrEmpty(postStr))
                    {
                        //设置请求参数
                        var requestParams = SetRequestParams(postStr);

                        //响应微信平台推送消息
                        ResponseMessage(httpContext, requestParams);
                    }

                }
            }
            catch (Exception ex)
            {
                Loggers.Exception(new ExceptionLogInfo(ex));
            }
        }

        #endregion

        #region 设置请求参数

        //设置请求参数
        private RequestParams SetRequestParams(string postStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(postStr);

            XmlNodeList list = doc.GetElementsByTagName("xml");
            XmlNode xn = list[0];
            string openID = xn.SelectSingleNode("//FromUserName").InnerText;    //发送方帐号（一个OpenID）
            string weixinID = xn.SelectSingleNode("//ToUserName").InnerText;    //开发者微信号
            string msgType = xn.SelectSingleNode("//MsgType").InnerText.ToLower();  //消息类型(text, image, location, link, event)

            CommonUtils.WriteLogWeixin("---------------开始接收微信平台推送消息---------------", weixinID);
            CommonUtils.WriteLogWeixin("微信平台推送的消息:  " + postStr, weixinID);
            CommonUtils.WriteLogWeixin("FromUserName(发送方帐号):  " + openID, weixinID);
            CommonUtils.WriteLogWeixin("ToUserName(开发者微信号):  " + weixinID, weixinID);
            CommonUtils.WriteLogWeixin("MsgType(消息类型):  " + msgType, weixinID);

            var requestParams = new RequestParams()
            {
                OpenId = openID,
                WeixinId = weixinID,
                MsgType = msgType,
                XmlNode = xn,
            };

            return requestParams;
        }

        #endregion

        #region 响应微信平台推送消息

        //响应微信平台推送消息
        private void ResponseMessage(HttpContext httpContext, RequestParams requestParams)
        {
            BaseBLL weixin = null;
            IFactory factory = null;

            #region 通过微信类型生成对应的业务处理类

            var application = new WApplicationBLL(new BasicUserInfo());
            var appEntitys = application.QueryByEntity(new WApplicationEntity() { WeixinID = requestParams.WeixinId }, null);

            if (appEntitys != null && appEntitys.Length > 0)
            {
                var entity = appEntitys.FirstOrDefault();
                CommonUtils.WriteLogWeixin("通过微信类型生成对应的业务处理类", requestParams.WeixinId);
                CommonUtils.WriteLogWeixin("WeiXinTypeId(微信类型):  " + entity.Type, requestParams.WeixinId);

                switch (entity.Type)
                {
                    case C_WeixinType.SUBSCRIPTION:
                        factory = new SubscriptionFactory();
                        weixin = factory.CreateWeiXin(httpContext, requestParams);
                        CommonUtils.WriteLogWeixin("订阅号", requestParams.WeixinId);
                        break;
                    case C_WeixinType.SERVICE:
                        factory = new ServiceFactory();
                        weixin = factory.CreateWeiXin(httpContext, requestParams);
                        CommonUtils.WriteLogWeixin("服务号", requestParams.WeixinId);
                        break;
                    case C_WeixinType.CERTIFICATION:
                        factory = new CertificationFactory();
                        weixin = factory.CreateWeiXin(httpContext, requestParams);
                        CommonUtils.WriteLogWeixin("认证服务号", requestParams.WeixinId);
                        break;
                    default:
                        factory = new SubscriptionFactory();
                        weixin = factory.CreateWeiXin(httpContext, requestParams);
                        CommonUtils.WriteLogWeixin("默认订阅号", requestParams.WeixinId);
                        break;
                }

                //Dictionary<string, string> dic = new Dictionary<string, string>();
                //dic.Add("first", "您好，您已预约租车成功。");
                //dic.Add("productType", "服务");
                //dic.Add("name", "小轿车车辆普通维护");
                //dic.Add("time", DateTime.Now.ToShortDateString());
                //dic.Add("result", "已预约");
                //dic.Add("remark", "如有疑问，请咨询13912345678。");
                //CommonBLL bll = new CommonBLL();
                //var accessToken = bll.GetAccessToken(appEntitys[0].AppID, appEntitys[0].AppSecret, appEntitys[0].WeixinID);
                //var result = bll.PushTemplateMessage(accessToken.access_token, requestParams.OpenId
                //    , "NoceyN6bKF-HnM6TmrkTfI3qcqTt1ncKe18sxhpQ_bI",dic);
            }

            #endregion
            
            weixin.ResponseMessage();
        }

        #endregion
    }
}