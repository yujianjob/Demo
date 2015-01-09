using System;
using System.Collections.Generic;
using System.Configuration;
using System.Transactions;
using System.Web;
using YuJian.WeiXin.BLL;
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
                case MsgType.TEXT:    //文本消息
                    CommonUtils.WriteLogWeixin("消息类型：---------------text文本消息！", requestParams.WeixinId);
                    Text();
                    break;
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

        #region 处理文本消息

        public virtual void Text()
        {
            var content = requestParams.XmlNode.SelectSingleNode("//Content").InnerText.Trim();   //文本消息内容
            CommonUtils.WriteLogWeixin("文本消息内容：" + content, requestParams.WeixinId);

            var list = new List<WNewsEntity>();
            switch (content.ToUpper())
            {
                case "A":
                    list.Add(new WNewsEntity()
                    {
                        Title = "A. 希望肌肤更加亮泽丰盈 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnW5ju7ibvkjMW6924ziaiccgdGuskKEh6bIiceDDtDy8gNvUGEsdpu1tqnKiafICqRoZAdmaYYxbrZwcUg/0",
                        Description = "A. 希望肌肤更加亮泽丰盈 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202175619&idx=1&sn=2db3043ccf3ce3b22d7449c43509ac16#rd"
                    }
                        );
                    break;
                case "B":
                    list.Add(new WNewsEntity()
                    {
                        Title = "B. 希望肌肤更加水润迷人 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnW5ju7ibvkjMW6924ziaiccgdGEzqJ60liceETxIXrSdVOhnLGQEhFAc4AJUNx9st4X8asW8Hq7VS781g/0",
                        Description = "B. 希望肌肤更加水润迷人 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202175633&idx=1&sn=a66d5c56807090746465b6550d709f06#rd"
                    }
                        );
                    break;
                case "C":
                    list.Add(new WNewsEntity()
                    {
                        Title = "C. 希望肌肤停驻年轻状态 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnW5ju7ibvkjMW6924ziaiccgdG5rNjTxGO88iczMC9icsvtTrb8naP55J9R2FjNN3Tjh2WykHAL8GgJR6g/0",
                        Description = "C. 希望肌肤停驻年轻状态 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202175654&idx=1&sn=165a5e7db028ab2a1e477097aa711602#rd"
                    }
                        );
                    break;
                case "D":
                    list.Add(new WNewsEntity()
                    {
                        Title = "D. 以上美肌心愿皆想实现 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnW5ju7ibvkjMW6924ziaiccgdG2iaIyzeLz0mmticVqAmnYnTIicIUaEnOzqjVzb90rukgJibV5kNv0ugVvw/0",
                        Description = "D. 以上美肌心愿皆想实现 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202175672&idx=1&sn=80e24431a3c9517d78525d5134d6bbf4#rd"
                    }
                        );
                    break;
                case "水瓶":
                case "水瓶座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：旅行】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUpeYohTMPCUiaXOF0mI4oZO5UsiacALOGekzqF3CIPfFFJ0OjcvJmpVmvA/0",
                        Description = "【我的星年签：旅行】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290366&idx=1&sn=334396e8a33270d1713593bb13a5ac62#rd"
                    }
                        );
                    break;
                case "双鱼":
                case "双鱼座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：承诺】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUpp8YfTU4d9fSKVEwiceK3cwWMVcM6EHDV0ia7VNicZwB7WI0fMJ0CHOW8g/0",
                        Description = "【我的星年签：承诺】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202289910&idx=1&sn=d3269faee5bcec890b1e6d43e96e3cb8#rd"
                    }
                        );
                    break;
                case "白羊":
                case "白羊座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：甜蜜】",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUp9WtkicdDr1Rae5uZcicXnIy5EamwC79TY5woEeELxmswDzalXady5rTA/0",
                        Description = "【我的星年签：甜蜜】",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290423&idx=1&sn=387fa52cb2fcaf12b2518b3ae917a4ec#rd"
                    }
                        );
                    break;
                case "金牛":
                case "金牛座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：真爱】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUpwVWAgiawzQoMaX182jEYvEFIZcHBC1fCFfonA9IwPhJoLE6IHU7Y9Lw/0",
                        Description = "【我的星年签：真爱】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290467&idx=1&sn=55c89b6963d0d60a1007d931575f6f8e#rd"
                    }
                        );
                    break;
                case "双子":
                case "双子座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：成长】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUpbMgJic3ic8dXcXu02pakAcicNW4ibIYicEaicaJdZGeVG7fbZsGr5iaHUmOgw/0",
                        Description = "【我的星年签：成长】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290510&idx=1&sn=c00fcd9a20a6b396204830bfa6a930f4#rd"
                    }
                        );
                    break;
                case "巨蟹":
                case "巨蟹座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：梦想】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUpspVmkanFZRu9P9AZukg5LsHfiaibuo22sbdMmZvBDcHUzkFZwiaa30qUw/0",
                        Description = "【我的星年签：梦想】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290614&idx=1&sn=59f015b814f5cd2b004919f25e1ceda6#rd"
                    }
                        );
                    break;
                case "狮子":
                case "狮子座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：浪漫】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUplXbofOB6sibv9xFhXwKshoo82ZuO12warT3rX8qh3fWScQvcpKcz9AQ/0",
                        Description = "【我的星年签：浪漫】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290708&idx=1&sn=dd512816f226b93ca22f3639d223795b#rd"
                    }
                        );
                    break;
                case "处女":
                case "处女座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：收获】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUp0xz657uPc5CetSqjBUhEnelHgAjhUiaxMJeBUBnR6RXo7zflcNCblNA/0",
                        Description = "【我的星年签：收获】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290742&idx=1&sn=a60ff017a87d4be6cd899706b78e5b83#rd"
                    }
                        );
                    break;
                case "天秤":
                case "天秤座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：幸福】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUppXEYI30NsuzOIib2TbYM5Lia79EiaTUKTNTgibPKfayXkdlXjib4qttJAow/0",
                        Description = "【我的星年签：幸福】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290817&idx=1&sn=f31f8dcccbbdd02afe20b89fe9041d0f#rd"
                    }
                        );
                    break;
                case "天蝎":
                case "天蝎座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：机遇】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUp4agUdALxOjHia5Mib4bJh80dEeIABeIZg1I0zITVO1tpDxpTiaGyDYagA/0",
                        Description = "【我的星年签：机遇】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290867&idx=1&sn=aff94d69e9cfe11f664f289c30c48407#rd"
                    }
                        );
                    break;
                case "射手":
                case "射手座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：改变】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUpymwPkhEVcvWJhRg0R04PJI9gr2PMxdBrRDdiasFbNyoFZ8ziaSpU0icuA/0",
                        Description = "【我的星年签：改变】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290930&idx=1&sn=84bc650285a89c13726d5a2be6330f75#rd"
                    }
                        );
                    break;
                case "摩羯":
                case "摩羯座":
                    list.Add(new WNewsEntity()
                    {
                        Title = "【我的星年签：惊喜】 ",
                        PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnXvM0L5WWicJpfRThC54pSUpryINkyLgTNJZQQk8r0icIcbYVCcUkDq5qcaEVNic4ic3EX2GKDJJ5NwkA/0",
                        Description = "【我的星年签：惊喜】 ",
                        OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202290966&idx=1&sn=1443832cc3f54344f6710a4543ddd912#rd"
                    }
                        );
                    break;
            }
            if (list.Count > 0)
                commonService.ResponseNewsMessage(requestParams.WeixinId, requestParams.OpenId, list, httpContext);
        }

        #endregion

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
                    ClickEvent(eventKey);

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
                var list = new List<WNewsEntity>();
                list.Add(new WNewsEntity()
                {
                    Title = "一“扫”倾心，开始Terrake法国天芮B.L.C.奇迹焕颜之旅",
                    PictureUrl = "http://tianrui.efic.com.cn/images/banner.jpg",
                    Description = "一“扫”倾心，开始Terrake法国天芮B.L.C.奇迹焕颜之旅",
                    OriginalUrl = "http://tianrui.efic.com.cn/WeiXin/AuthFeedBack.aspx?weixinId=gh_0846d6c7c2b8&goUrl=http://tianrui.efic.com.cn/index.html"
                }
                );
                commonService.ResponseNewsMessage(requestParams.WeixinId, requestParams.OpenId, list, httpContext);

                var appEntity = new WApplicationBLL(new BasicUserInfo()).GetAll()[0];
                CommonUtils.WriteLogWeixin("创建自定义菜单", appEntity.WeixinID);

                if (requestParams.OpenId.Trim() == "obbEAj1sXp06eR1GOrbb_VgIQ7yY")
                {
                    //获取access_token
                    var accessToken = commonService.GetAccessToken(appEntity.AppID, appEntity.AppSecret, appEntity.WeixinID);

                    if (accessToken.errcode == null || accessToken.errcode.Equals(string.Empty))
                    {
                        string content = string.Empty;
                        string uri = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accessToken.access_token);
                        string method = "POST";

                        content = "{"
        + "\"button\": [" +
            "{"
               + "\"type\": \"click\","
               + "\"name\": \"天芮官网\","
               + "\"key\": \"s1\","
               + "\"url\": \"\","
               + " \"sub_button\": ["
                  + "  {"
                      + "\"type\": \"view\","
                      + "\"name\": \"Terraké官网\","
                      + "\"key\": \"s1_1\","
                      + "\"url\": \"http://www.terrake.net\""
                    + "}"
               + " ]"
            + "},"
           + " {"
               + "\"type\": \"click\","
               + "\"name\": \"Vspa美肌\","
                + "\"key\": \"s2\","
               + "\"url\": \"\","
               + "\"sub_button\": ["
                   + "{"
                        + "\"type\": \"click\","
                        + "\"name\": \"挚爱臻选\","
                        + "\"key\": \"s2_1\","
                        + "\"url\": \"\""
                   + " },"
                   + "{"
                        + "\"type\": \"click\","
                        + "\"name\": \"臻致呵护\","
                        + "\"key\": \"s2_2\","
                        + "\"url\": \"\""
                   + " },"
                   + " {"
                    + "\"type\": \"click\","
                        + "\"name\": \"V-life时光\","
                        + "\"key\": \"s2_3\","
                        + "\"url\": \"\""
                    + "}"
               + " ]"
            + "},"
            + "{"
                + "\"type\": \"click\","
                + "\"name\": \"奢宠会\","
                + "\"key\": \"s3\","
                + "\"url\": \"\","
               + "\"sub_button\": ["
                   + " {"
                        + "\"type\": \"click\","
                        + "\"name\": \"达人力荐\","
                        + "\"key\": \"s3_1\","
                        + "\"url\": \"\""
                    + "},"
                    + "{"
                        + "\"type\": \"click\","
                        + "\"name\": \"芮粉分享\","
                        + "\"key\": \"s3_2\","
                        + "\"url\": \"\""
                   + " },"
                    + "{"
                        + "\"type\": \"click\","
                        + "\"name\": \"美肌Q&A\","
                        + "\"key\":\"s3_3\","
                        + "\"url\": \"\""
                   + " },"
                    + "{"
                        + "\"type\": \"view\","
                        + "\"name\": \"防伪查询\","
                        + "\"key\": \"s3_4\","
                        + "\"url\": \"http://terrake.mingyue315.com/m/fw.aspx\""
                   + " },"
                    + "{"
                        + "\"type\": \"view\","
                        + "\"name\": \"一扫即享千份礼遇\","
                        + "\"key\": \"s3_5\","
                        + "\"url\": \"http://tianrui.efic.com.cn/WeiXin/AuthFeedBack.aspx?weixinId=gh_0846d6c7c2b8&goUrl=http://tianrui.efic.com.cn/index.html\""
                    + "}"
               + " ]"
            + "}"
       + " ]"
    + "}";

                        CommonUtils.WriteLogWeixin("content：" + content, appEntity.WeixinID);

                        string data = CommonUtils.GetRemoteData(uri, method, content);
                        var result = data.DeserializeJSONTo<ResultEntity>();


                        CommonUtils.WriteLogWeixin("创建自定义菜单返回值： " + data, appEntity.WeixinID);
                    }
                }
                //weixinService.SaveOrUpdateCustomer(1);
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

        #region 点击菜单拉取消息时的事件推送

        public virtual void ClickEvent(string eventKey)
        {
            try
            {
                var list = new List<WNewsEntity>();
                switch (eventKey)
                {
                    case "s2_1":
                        list.Add(new WNewsEntity()
                        {
                            Title = "Terrake 3大美肤单品 消除岁末“危肌” ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnVjyOhWHznOQK1MsCaRcT6DrESldGk5tgsIJBKTPUIwHNo2Iuw7yxf74KWPuiaJOH2LteoUZhZuzhA/0",
                            Description = "Terrake 3大美肤单品 消除岁末“危肌” ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202162969&idx=1&sn=c1bd34e6e9f4b015c6473d955793ce66"
                        }
                        );
                        break;
                    case "s2_2":
                        list.Add(new WNewsEntity()
                        {
                            Title = "精华护理 肌肤比你更懂 ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnVjyOhWHznOQK1MsCaRcT6D48uuyibdozCZJNIKCknQnpJAXIAhjqdm1vmj4dJnibSw57tKtg8zgudw/0",
                            Description = "精华护理 肌肤比你更懂 ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202162855&idx=1&sn=c2369e3d2a90912eb9a23dd4c6f9140d"
                        }
                        );
                        break;
                    case "s2_3":
                        list.Add(new WNewsEntity()
                        {
                            Title = "午后时光中的甜蜜情缘 ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnVBiaPPYbEqsuXmjKRQD5QiaLbXNYI3cPURxWwmYTnSicYlLSfOGODrrJo6Z10BQ9DHlicyQ502RAZ51w/0",
                            Description = "午后时光中的甜蜜情缘 ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202065745&idx=1&sn=82d3e8bc3f7d9c9c8fab812655df0dee#rd"
                        }
                        );
                        break;
                    case "s3_1":
                        list.Add(new WNewsEntity()
                        {
                            Title = "达人力荐：圣诞夜Party前的底妆秘籍 ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnWnNmiaP14xCqQDZ8eFhuusY7038LqskUJxTMlfxPt8v385ULHHwIJNSSe2OqAsUnXLQgmLPE165Tg/0",
                            Description = "达人力荐：圣诞夜Party前的底妆秘籍 ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=202039639&idx=1&sn=3f0167ebb0b4c808deeec565ed2404da#rd"
                        }
                        );
                        break;
                    case "s3_2":
                        list.Add(new WNewsEntity()
                        {
                            Title = "芮粉分享：Terrake法国天芮森林能量雪妍花肌水——亮颜惊喜 顷刻即现 ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnVF7tONkvVdGoZkSq7UOZPpA86xly7N5AswtiaFyqsU3Gdpno0sJO7YM4kxwiat3XvJ91QHW3LJWaibg/0",
                            Description = "芮粉分享：Terrake法国天芮森林能量雪妍花肌水——亮颜惊喜 顷刻即现 ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=201906174&idx=1&sn=a9d44b43550e9728a055647199aaa747#rd"
                        }
                        );
                        break;
                    case "s3_3":
                        list.Add(new WNewsEntity()
                        {
                            Title = "美肌Q&A：最近感觉眼角出现了一些干纹，可是我刚刚二十出头，用眼霜会不会太早？ ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnWe3YrcGVJDvvqD7IJS6ibJsUpiaRHDwUibf9qW9V8y4Ts8pTbyd467gtbqCvRjMYl3YMQ9hsCOiaVG7w/0",
                            Description = "美肌Q&A：最近感觉眼角出现了一些干纹，可是我刚刚二十出头，用眼霜会不会太早？ ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=201837439&idx=1&sn=beeb5fb8835bdaa1622ca9df1f4cc1fb#rd"
                        }
                        );
                        list.Add(new WNewsEntity()
                        {
                            Title = "美肌Q&A：精华液每次的使用量多少最佳？什么时候用最好？ ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnWe3YrcGVJDvvqD7IJS6ibJsv8mGsfSCKicEkX5bJkLQHxLQNSnUAUicdaSLBibtOECeVpK0SROpqMWCg/0",
                            Description = "美肌Q&A：精华液每次的使用量多少最佳？什么时候用最好？ ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=201837439&idx=2&sn=4271e29d5e4708ce78bdb1ca77576831#rd"
                        }
                        );
                        list.Add(new WNewsEntity()
                        {
                            Title = "美肌Q&A：皮肤很干，晚上随便什么时候保湿效果都好么？ ",
                            PictureUrl = "http://mmbiz.qpic.cn/mmbiz/ibYUArGxZvnWe3YrcGVJDvvqD7IJS6ibJs4LIkomrhDRHX3ia9iaibjTjaYjcIu7rMiaAGweJTfx4b7Zofia6R030vc5g/0",
                            Description = "美肌Q&A：皮肤很干，晚上随便什么时候保湿效果都好么？ ",
                            OriginalUrl = "http://mp.weixin.qq.com/s?__biz=MjM5OTE4NzMwOQ==&mid=201837439&idx=3&sn=029891af9e99d5c94b7e492a793306dd#rd"
                        }
                        );
                        break;

                }
                commonService.ResponseNewsMessage(requestParams.WeixinId, requestParams.OpenId, list, httpContext);
            }
            catch (Exception e)
            {
                CommonUtils.WriteLogWeixin("点击菜单拉取消息时的事件推送！" + e.Message, requestParams.WeixinId);
            }
        }

        #endregion

        public class WNewsEntity
        {
            /// <summary>
            /// 标题
            /// </summary>
            public String Title { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public String Description { get; set; }

            /// <summary>
            /// 封面图片
            /// </summary>
            public String PictureUrl { get; set; }

            /// <summary>
            /// 原文链接
            /// </summary>
            public String OriginalUrl { get; set; }
        }
    }
}
