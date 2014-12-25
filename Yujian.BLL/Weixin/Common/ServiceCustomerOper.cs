using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Yunchee.Volkswagen.Common.Const;
using Yunchee.Volkswagen.DataAccess;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Entity.Weixin;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.Log;

namespace Yunchee.Volkswagen.BLL.Weixin.Common
{
    public class ServiceCustomerOper
    {
        private BasicUserInfo loginInfo = null;
        private ServiceCustomerMappingBLL mappingService = null;
        private ServiceLastAcceptBLL acceptService = null;
        private CustomerBLL cusomerService = null;
        private CommonBLL commonBll = null;
        private ServiceWaitQueueBLL queueService = null;
        private WApplicationBLL applicationService = null;
        public  string TemplateMsg = @"客户信息：
昵称：{0}
姓名：{1}
性别：{2}
类型：{3}
联络方式：{4}
{5}";
        /// <summary>
        /// 发送客服消息模版
        /// </summary>
        public string ServiceMessage = @" 有新的{0}消息需要处理!
客户信息：
昵称：{1}
姓名：{2}
性别：{3}
客户类型：{4}
联络方式：{5}
{6}

请回复相关数字进行下一步操作：
1. 开始聊天
2. 订单详情 
3. 快捷回复消息指令 
4. 结束聊天
5. 确认订单
{7}
如无需聊天请回复4结束聊天再回复5确认订单。
";
        public ServiceCustomerOper()
        {
            loginInfo = new BasicUserInfo() { UserID = 1, ClientID = 1 };
            mappingService = new ServiceCustomerMappingBLL(loginInfo);
            acceptService = new ServiceLastAcceptBLL(loginInfo);
            cusomerService = new CustomerBLL(loginInfo);
            commonBll = new CommonBLL();
            queueService = new ServiceWaitQueueBLL(loginInfo);
            applicationService = new WApplicationBLL(loginInfo);
        }

        /// <summary>
        /// 建立关联
        /// </summary>
        /// <param name="firstEntity">队列实体</param>
        /// <param name="appEnity">公众号</param>
        /// <param name="User">客服</param>
        public void CreateMapping(ServiceWaitQueueEntity firstEntity, WApplicationEntity appEnity, UsersEntity User, string roleName, bool debug)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
            {
                #region 建立关联
                ServiceCustomerMappingEntity mapping = null;
                var mappingArr = mappingService.QueryByEntity(
                    new ServiceCustomerMappingEntity()
                    {
                        BusinessType = firstEntity.MessageType.ToString(),
                        BusinessID = firstEntity.OrderID,
                        ServiceOpenId = User.WxOpenId,
                        SessionStatus = C_YesOrNo.YES
                    }, null);
                if (mappingArr.Count() > 0)
                {
                    mapping = mappingArr[0];
                    if (mapping.SessionStatus == C_YesOrNo.NO)
                    {
                        mapping.SessionStatus = C_YesOrNo.YES;
                        mappingService.Update(mapping);
                    }
                }
                else
                {
                    mapping = new ServiceCustomerMappingEntity();
                    mapping.WxId = User.WxId;
                    mapping.CustomerOpenId = firstEntity.CustomerOpenid;
                    mapping.ServiceOpenId = User.WxOpenId;
                    mapping.BusinessID = firstEntity.OrderID;
                    mapping.BusinessType = firstEntity.MessageType.ToString();
                    mapping.AcceptTime = DateTime.Now;
                    mapping.SessionStatus = C_YesOrNo.YES;
                    mappingService.Create(mapping);
                }
                #endregion
                if (debug)
                {
                    Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "建立关联--" + User.WxOpenId + "--客户:" + firstEntity.CustomerOpenid });
                }
                else
                {
                    CommonUtils.WriteLogWeixin("建立关联--" + User.WxOpenId + "--客户:" + firstEntity.CustomerOpenid, appEnity.WeixinID);
                }
                #region 记录接入客户日志
                var lastAccpet = acceptService.QueryByEntity(new ServiceLastAcceptEntity() { ServiceOpenid = User.WxOpenId, ApplicationID = firstEntity.ApplicationID }, null);
                if (lastAccpet.Count() > 0)
                {
                    var record = lastAccpet.FirstOrDefault();
                    record.LastAccpetTime = DateTime.Now;
                    acceptService.Update(record);
                }
                else
                {
                    var record = new ServiceLastAcceptEntity();
                    record.MessageType = firstEntity.MessageType;
                    record.OrderID = firstEntity.OrderID;
                    record.CustomerOpenid = firstEntity.CustomerOpenid;
                    record.ServiceOpenid = User.WxOpenId;
                    record.ApplicationID = firstEntity.ApplicationID;
                    record.LastAccpetTime = DateTime.Now;
                    acceptService.Create(record);
                }
                #endregion

                if (debug)
                {
                    Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "记录接入客户日志--客服：" + User.WxOpenId + "客户:" + firstEntity.CustomerOpenid });
                }
                else
                {
                    CommonUtils.WriteLogWeixin("记录接入客户日志--客服：" + User.WxOpenId + "客户:" + firstEntity.CustomerOpenid, appEnity.WeixinID);
                }
                #region 推送单据消息到客服
                //客户
                var customer = cusomerService.QueryByEntity(new CustomerEntity() { WxId = User.WxId, WxOpenId = firstEntity.CustomerOpenid }, null);
                if (customer.Count() == 0) return;
                SendMessageEntity message = new SendMessageEntity();
                message.touser = User.WxOpenId;
                message.msgtype = "text";
                string SpecialContent;//单据信息
                string orderName;//订单姓名
                string orderSex;//订单性别
                string orderPhone;//订单联系方式
                GetOrderInfo(firstEntity.MessageType.Value,firstEntity.OrderID.Value, out SpecialContent, out orderName, out orderSex, out orderPhone);
                message.content = string.Format(ServiceMessage
                    , GetMsgType(firstEntity.MessageType)
                    , customer[0].WxNickName
                    , GetCustomerName(firstEntity.MessageType, orderName, customer[0].Name)
                    , GetSex(firstEntity.MessageType, orderSex, customer[0].WxSex)
                    , GetCusType(customer[0].Type)
                    , GetPhone(firstEntity.MessageType, orderPhone, customer[0].Phone)
                    , SpecialContent
                    , roleName == "售后客服" ? "6. 转给售后顾问" : "");
                Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "发送消息内容--" + message.content });
                var result = commonBll.SendCustomerMessage(message, appEnity.AppID, appEnity.AppSecret, appEnity.WeixinID);
                #endregion
                Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "推送单据消息到客服--" + User.WxOpenId });

                if (roleName == "售后客服")//如为售后客服，向PC端推送消息
                {
                    WriteServiceText(mapping, message.content);
                    if (debug)
                    {
                        Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "推送单据消息到PC客服--" + User.WxOpenId });
                    }
                    else
                    {
                        CommonUtils.WriteLogWeixin("推送单据消息到PC客服--" + User.WxOpenId, appEnity.WeixinID);
                    }
                }

                queueService.DeletePhysical(firstEntity.ID.HasValue ? firstEntity.ID.Value : 0);
                if (debug)
                {
                    Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "删除处理成功的队列实体--ID:" + firstEntity.ID });
                }
                else
                {
                    CommonUtils.WriteLogWeixin("删除已处理成功队列实体--ID:" + firstEntity.ID, appEnity.WeixinID);
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// 向关联的客服发送本地会话消息
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="content"></param>
        public void WriteServiceText(ServiceCustomerMappingEntity mapping, string content)
        {
            ServiceSessionInfoBLL sessionService = new ServiceSessionInfoBLL(loginInfo);
            ServiceTextBLL textService = new ServiceTextBLL(loginInfo);
            var session = new ServiceSessionInfoEntity();
            session.WxId = mapping.WxId;
            session.FromOpenId = mapping.WxId;
            session.ToOpenId = mapping.ServiceOpenId;
            session.MessageType = "1";
            session.SendTime = DateTime.Now;
            session.BusinessID = mapping.BusinessID;
            session.BusinessType = mapping.BusinessType;
            session.SessionFlag = "3";//公众号发给客服
            session.IsRead = "0";
            session.SessionType = "0";
            sessionService.Create(session);

            var text = new ServiceTextEntity();
            text.SessionID = session.ID;
            text.Text = content;
            textService.Create(text);
        }

        /// <summary>
        /// 订单详情，如为保养，维修，试驾，返回订单的用户名，性别，联系方式
        /// </summary>
        /// <param name="MessageType"></param>
        /// <param name="OrderID"></param>
        /// <param name="SpecialContent"></param>
        /// <param name="orderName"></param>
        /// <param name="orderSex"></param>
        /// <param name="orderPhone"></param>
        public void GetOrderInfo(int MessageType, int OrderID, out string SpecialContent, out string orderName, out string orderSex, out string orderPhone)
        {
            SpecialContent = string.Empty;
            orderName = string.Empty;//单据客户
            orderSex = string.Empty;//单据性别
            orderPhone = string.Empty;//单据联系方式
            if (MessageType == 1 || MessageType == 2
                || MessageType == 3 || MessageType == 4)//订单详情
            {
                if (MessageType == 1)
                {
                    MaintenanceOrderDAO dao = new MaintenanceOrderDAO(new BasicUserInfo() { ClientID = 1, UserID = 1 });
                    var obj = dao.GetMaintenanceOrderById(OrderID);
                    var project = new MaintenanceOrderDetailDAO(new BasicUserInfo() { ClientID = 1, UserID = 1 }).GetMaintenanceProject(obj.ID.ToString());
                    StringBuilder sbProject = new StringBuilder();
                    foreach (DataRow dr in project.Rows)
                    {
                        if (dr["isChecked"].ToString() == "1")
                        {
                            sbProject.AppendLine(dr["Name"].ToString());
                        }
                    }
                    SpecialContent = string.Format(@"车型：{0}
车牌号：{1}
上次保养时间：{2}
上次保养公里数：{3}
预约保养时间：{4}
保养项目：{5}", obj.CarTypeName, obj.LicensePlateNumber, obj.LastMaintenanceTime
       , obj.LastMileage, Convert.ToDateTime(obj.TargetTime).ToString("yyyy-MM-dd HH:mm"), sbProject.ToString());
                    orderName = obj.CustomerName;
                    orderSex = obj.GenderName;
                    orderPhone = obj.Phone;
                }
                if (MessageType == 2)
                {
                    RepairOrderDAO dao = new RepairOrderDAO(new BasicUserInfo() { ClientID = 1, UserID = 1 });
                    var obj = dao.GetRepairOrderById(OrderID);
                    SpecialContent = string.Format(@"车型：{0}
车牌号：{1}
维修原因：{2}", obj.CarTypeName, obj.LicensePlateNumber, obj.RepairReason);
                    orderName = obj.CustomerName;
                    orderSex = obj.GenderName;
                    orderPhone = obj.Phone;
                }
                if (MessageType == 3)
                {
                    TestDriveOrderDAO dao = new TestDriveOrderDAO(new BasicUserInfo() { ClientID = 1, UserID = 1 });
                    var obj = dao.GetTestDriveOrderById(OrderID);
                    SpecialContent = string.Format(@"试驾车型：{0}
计划购车时间：{1}
预约试驾时间：{2}", obj.CarTypeName, obj.PlanBuyTimeName, Convert.ToDateTime(obj.TargetTime).ToString("yyyy-MM-dd HH:mm"));
                    orderName = obj.CustomerName;
                    orderSex = obj.GenderName;
                    orderPhone = obj.Phone;
                }
                if (MessageType == 4)
                {
                    ServiceAskPriceDAO dao = new ServiceAskPriceDAO(new BasicUserInfo() { ClientID = 1, UserID = 1 });
                    var obj = dao.GetRAskPriceOrderById(OrderID);
                    SpecialContent = "询价车型：" + obj.CarTypeName;
                }
            }
        }

        /// <summary>
        /// 返回联系方式
        /// </summary>
        /// <param name="nullable"></param>
        /// <param name="orderPhone"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetPhone(int? MessageType, string orderPhone, string customerPhone)
        {
            if (MessageType == 1 || MessageType == 2 || MessageType == 3)
            {
                return orderPhone;
            }
            return customerPhone;
        }

        /// <summary>
        /// 返回客户姓名
        /// </summary>
        /// <param name="MessageType"></param>
        /// <param name="orderName"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public string GetCustomerName(int? MessageType, string orderName, string customerName)
        {
            if (MessageType == 1 || MessageType == 2 || MessageType == 3)
            {
                return orderName;
            }
            return customerName;
        }

        /// <summary>
        /// 返回客户类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCusType(string type)
        {
            if (type == "0")
                return "粉丝";
            if (type == "1")
                return "潜客";
            if (type == "2")
                return "车主";
            if (type == "3")
                return "用户";
            return "未知";
        }

        /// <summary>
        /// 返回性别
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public string GetSex(int? MessageType, string orderSex, string sex)
        {
            if (MessageType == 1 || MessageType == 2 || MessageType == 3)
            {
                return orderSex;
            }
            if (sex == "0")
                return "女";
            if (sex == "1")
                return "男";
            return "未知";
        }

        /// <summary>
        /// 返回单据名称
        /// </summary>
        /// <param name="MessageType"></param>
        /// <returns></returns>
        public string GetMsgType(int? MessageType)
        {
            string msgType = string.Empty;
            switch (MessageType.ToString())
            {
                case Business_Type.MaintenanceOrder:
                    msgType = "保养订单";
                    break;
                case Business_Type.RepairOrder:
                    msgType = "维修订单";
                    break;
                case Business_Type.TestDriveOrder:
                    msgType = "试驾订单";
                    break;
                case Business_Type.AskPrice:
                    msgType = "在线询价";
                    break;
                case Business_Type.Consult:
                    msgType = "在线咨询";
                    break;
            }
            return msgType;
        }

        /// <summary>
        /// 预约保养，预约维修返回售后；试驾，询价，返回售前；咨询区分售前售后
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetService(ServiceWaitQueueEntity entity, string RoleName)
        {
            string roleName = string.Empty;
            switch (entity.MessageType.ToString())
            {
                case Business_Type.MaintenanceOrder:
                case Business_Type.RepairOrder:
                    roleName = RoleName.Split('|')[1];
                    break;
                case Business_Type.TestDriveOrder:
                case Business_Type.AskPrice:
                    roleName = RoleName.Split('|')[0];
                    break;
                case Business_Type.Consult:
                    var consult = new ServiceConsultBLL(loginInfo).GetByID(entity.OrderID);
                    if (consult != null && consult.ID > 0)
                    {
                        roleName = consult.Consult.ToString() == C_YesOrNo.NO ? RoleName.Split('|')[0] : RoleName.Split('|')[1];
                    }
                    break;
                default:
                    break;
            }
            return roleName;
        }

        /// <summary>
        /// 返回术语Code
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetTypeCode(ServiceCustomerMappingEntity mapping)
        {
            switch (mapping.BusinessType)
            {
                case "1":
                    return "Maintance";
                case "2":
                    return "Repair";
                case "3":
                    return "TestDrive";
                case "4":
                    return "AskPrice";
                case "5":
                    var consult = new ServiceConsultBLL(loginInfo).GetByID(mapping.BusinessID);
                    if (consult != null && consult.ID > 0)
                    {
                        return consult.Consult.ToString() == C_YesOrNo.NO ? "ConsultBeforSale" : "ConsultAfterSale";
                    }
                    return "";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 是否客服抢单指令
        /// </summary>
        /// <returns></returns>
        public bool GrabOrder(RequestParams requestParams)
        {
            var User = new UsersBLL(loginInfo).QueryByEntity(new UsersEntity() { WxId = requestParams.WeixinId, WxOpenId = requestParams.OpenId }, null);
            if (User.Count() == 0)
                return false;
            var service = mappingService.QueryByEntity(
                new ServiceCustomerMappingEntity() { ServiceOpenId = requestParams.OpenId, SessionStatus = C_YesOrNo.YES }, null);
            if (service.Count() > 0)//有正在处理的单据
                return false;

            var massOrder = queueService.QueryByEntity(new ServiceWaitQueueEntity() { Status = 4 }, null);
            RolesBLL roleService = new RolesBLL(loginInfo);
            RoleUsersBLL roleUserService = new RoleUsersBLL(loginInfo);
            if (massOrder.Count() > 0)
            {
                #region 群发单据
                string roleName = string.Empty;
                foreach (var tmp in massOrder)
                {
                    if (tmp.MessageType == 1 || tmp.MessageType == 2)
                    {
                        roleName = "销售顾问";
                    }
                    if (tmp.MessageType == 3 || tmp.MessageType == 4)
                    {
                        roleName = "售后客服";
                    }
                    if (tmp.MessageType == 5)
                    {
                        var consult = new ServiceConsultBLL(loginInfo).GetByID(tmp.OrderID);
                        roleName = consult.Consult == 0 ? "销售顾问" : "售后客服";
                    }
                    var Sales = roleService.QueryByEntity(new RolesEntity() { Name = roleName }, null);
                    if (Sales.Count() > 0)
                    {
                        var roleUser = roleUserService.QueryByEntity(new RoleUsersEntity() { RoleID = Sales[0].ID, UserID = User[0].ID }, null);
                        if (roleUser.Count() > 0)
                        {
                            CreateMapping(tmp, applicationService.GetByID(tmp.ApplicationID), User[0], roleName, false);

                            queueService.DeletePhysical(tmp.ID.HasValue ? tmp.ID.Value : 0);
                            return true;
                        }
                    }
                }
                return false;
                #endregion
            }
            else
            {
                //没有群发状态的单据，返回失败消息
                GrapFailed(requestParams,User[0], "销售顾问");
                GrapFailed(requestParams,User[0], "售后客服");
            }
            return false;
        }

        /// <summary>
        /// 抢单失败
        /// </summary>
        /// <param name="User"></param>
        /// <param name="roleName"></param>
        private void GrapFailed(RequestParams requestParams,UsersEntity User, string roleName)
        {
            RolesBLL roleService = new RolesBLL(loginInfo);
            RoleUsersBLL roleUserService = new RoleUsersBLL(loginInfo);
            var Sales = roleService.QueryByEntity(new RolesEntity() { Name = roleName }, null);
            var appEntity = applicationService.QueryByEntity(new WApplicationEntity() { WeixinID = requestParams.WeixinId }, null);
            if (Sales.Count() > 0)
            {
                var roleUser = roleUserService.QueryByEntity(new RoleUsersEntity() { RoleID = Sales[0].ID, UserID = User.ID }, null);
                if (roleUser.Count() > 0)
                {
                    SendMessageEntity message = new SendMessageEntity();
                    message.touser = User.WxOpenId;
                    message.msgtype = "text";
                    message.content = "该订单已有其他工作人员在处理！";
                    var result = commonBll.SendCustomerMessage(message, appEntity[0].AppID, appEntity[0].AppSecret, appEntity[0].WeixinID);
                }
            }
        }
    }
}
