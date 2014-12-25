using System;
using System.Collections.Generic;
using System.Configuration;
using System.Transactions;
using System.Web;
using Yunchee.Volkswagen.BLL.Weixin.Common;
using Yunchee.Volkswagen.BLL.Weixin.Const;
using Yunchee.Volkswagen.BLL.Weixin.Enum;
using Yunchee.Volkswagen.Common.Const;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Entity.Weixin;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess.Query;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using Yunchee.Volkswagen.Utility.Log;

namespace Yunchee.Volkswagen.BLL.Weixin.Base
{
    /// <summary>
    /// 微信基类
    /// </summary>
    public class BaseBLL
    {
        #region 成员变量
        protected CommonBLL commonService = null;
        protected HttpContext httpContext = null;
        protected RequestParams requestParams = null;
        protected WeiXinBLL weixinService = null;

        #endregion

        #region 构造函数

        public BaseBLL(HttpContext httpContext, RequestParams requestParams)
        {
            BasicUserInfo loginInfo = new BasicUserInfo() { ClientID = 1, UserID = 1 };
            commonService = new CommonBLL();
            this.httpContext = httpContext;
            this.requestParams = requestParams;
            weixinService = new WeiXinBLL(httpContext, requestParams);
        }

        #endregion

        #region 响应微信平台推送的消息

        /// <summary>
        /// 响应微信平台推送的消息
        /// </summary>
        public void ResponseMessage()
        {
            //weixinService.UpdateRequestRecord();//更新最后请求时间
            //if (weixinService.CheckServiceCustomer() && requestParams.MsgType != MsgType.EVENT)//如存在客服关联，屏蔽普通事件,优先处理客服消息
            //{
            //    weixinService.ServiceAndCustomerCommuniciation();
            //}
            //else
            //{
            //根据不同的消息类型，进行不同的处理操作
            switch (requestParams.MsgType)
            {
                //case MsgType.TEXT:    //文本消息
                //    CommonUtils.WriteLogWeixin("消息类型：---------------text文本消息！", requestParams.WeixinId);
                //    Text();
                //    break;
                //case MsgType.IMAGE:   //图片消息
                //    CommonUtils.WriteLogWeixin("消息类型：---------------image图片消息！", requestParams.WeixinId);
                //    Image();
                //    break;
                //case MsgType.VOICE:   //语音消息
                //    CommonUtils.WriteLogWeixin("消息类型：---------------voice语音消息！", requestParams.WeixinId);
                //    Voice();
                //    break;
                //case MsgType.LOCATION:    //地理位置
                //    CommonUtils.WriteLogWeixin("消息类型：---------------location地理位置！", requestParams.WeixinId);
                //    Location();
                //    break;
                case MsgType.EVENT:   //事件
                    CommonUtils.WriteLogWeixin("消息类型：---------------event事件！", requestParams.WeixinId);
                    Event();
                    break;
            }
            //}

        }

        public virtual void Voice()
        {
            string mediaID = requestParams.XmlNode.SelectSingleNode("//MediaId").InnerText;  //媒体
            string msgId = requestParams.XmlNode.SelectSingleNode("//MsgId").InnerText;   //消息id，64位整型
            CommonUtils.WriteLogWeixin("媒体ID：---------------" + mediaID, requestParams.WeixinId);
            CommonUtils.WriteLogWeixin("消息id，64位整型MsgId：---------------" + msgId, requestParams.WeixinId);

            //weixinService.SaveNormalMessage();
        }

        #endregion

        #region 处理事件

        protected void Event()
        {
            string eventStr = requestParams.XmlNode.SelectSingleNode("//Event").InnerText.Trim().ToLower();
            var eventKey = string.Empty;

            CommonUtils.WriteLogWeixin("event" + eventStr, requestParams.WeixinId);
            switch (eventStr)
            {
                //关注
                case EventType.SUBSCRIBE:
                    CommonUtils.WriteLogWeixin("用户关注！", requestParams.WeixinId);
                    UserSubscribeEvent();
                    break;
                //取消关注
                case EventType.UNSUBSCRIBE:
                    CommonUtils.WriteLogWeixin("用户取消关注！", requestParams.WeixinId);
                    UserUnSubscribeEvent();
                    break;
                //扫描带参数二维码事件
                case EventType.SCAN:
                    CommonUtils.WriteLogWeixin("扫描带参数二维码事件，用户已关注时的事件推送", requestParams.WeixinId);

                    eventKey = requestParams.XmlNode.SelectSingleNode("//EventKey").InnerText;
                    CommonUtils.WriteLogWeixin("EventKey：" + eventKey, requestParams.WeixinId);
                    //ScanEvent(eventKey);

                    break;
                //上报地理位置事件
                case EventType.LOCATION:
                    CommonUtils.WriteLogWeixin("上报地理位置事件", requestParams.WeixinId);
                    //LocationEvent();
                    break;
                //点击菜单拉取消息时的事件推送
                case EventType.CLICK:
                    CommonUtils.WriteLogWeixin("点击菜单拉取消息时的事件推送", requestParams.WeixinId);

                    eventKey = requestParams.XmlNode.SelectSingleNode("//EventKey").InnerText;
                    CommonUtils.WriteLogWeixin("EventKey：" + eventKey, requestParams.WeixinId);
                    //ClickEvent(eventKey);

                    break;
                //点击菜单跳转链接时的事件推送
                case EventType.VIEW:
                    CommonUtils.WriteLogWeixin("点击菜单跳转链接时的事件推送", requestParams.WeixinId);

                    eventKey = requestParams.XmlNode.SelectSingleNode("//EventKey").InnerText;
                    CommonUtils.WriteLogWeixin("EventKey：" + eventKey, requestParams.WeixinId);
                    //ViewEvent(eventKey);
                    break;
                //事件推送群发结果
                case EventType.MASSSENDJOBFINISH:
                    CommonUtils.WriteLogWeixin("事件推送群发结果事件推送", requestParams.WeixinId);
                    MASSFINISHEvent();
                    break;
            }
        }

        /// <summary>
        /// 群发结果事件
        /// </summary>
        /// <param name="requestParams"></param>
        private void MASSFINISHEvent()
        {
            //weixinService.WriteMASSLog();
        }


        #endregion

        #region 用户关注微信号

        public virtual void UserSubscribeEvent()
        {
            try
            {
                weixinService.SaveOrUpdateCustomer(1);
                //weixinService.AutoReply(AutoReplyType.Subscrib, null);

                //扫描进来的关注
                //var xmlNode = requestParams.XmlNode.SelectSingleNode("//EventKey");
                //if (xmlNode != null && !string.IsNullOrEmpty(xmlNode.InnerText))
                //{
                //    //weixinService.SaveOrUpdateChannelEvent();
                //}
            }
            catch (Exception e)
            {
                CommonUtils.WriteLogWeixin("用户关注微信号！" + e.Message, requestParams.WeixinId);
            }
        }

        #endregion

        #region 用户取消关注微信号

        public virtual void UserUnSubscribeEvent()
        {
            try
            {
                weixinService.SaveOrUpdateCustomer(0);

                //weixinService.ClearServiceKeyWord();
            }
            catch (Exception e)
            {
                CommonUtils.WriteLogWeixin("用户取消关注微信号！" + e.Message, requestParams.WeixinId);
            }
        }


        #endregion


    }
}
