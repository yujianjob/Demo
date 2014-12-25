using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Common.Const
{
    /// <summary>
    /// 问卷选项类型
    /// </summary>
    public class C_QuesOptionType
    {
        /// <summary>
        /// 单行文本
        /// </summary>
        public const string TEXT = "1";
        /// <summary>
        /// 多行文本
        /// </summary>
        public const string TEXT_AREA = "2";
        /// <summary>
        /// 单选
        /// </summary>
        public const string RADIO_BOX = "3";
        /// <summary>
        /// 多选
        /// </summary>
        public const string CHECK_BOX = "4";
    }

    /// <summary>
    /// 性别
    /// </summary>
    public class C_Gender
    {
        /// <summary>
        /// 男
        /// </summary>
        public const string MEN = "1";
        /// <summary>
        /// 女
        /// </summary>
        public const string WOMEN = "2";
    }

    /// <summary>
    /// 是与否
    /// </summary>
    public class C_YesOrNo
    {
        /// <summary>
        /// 是
        /// </summary>
        public const string YES = "1";
        /// <summary>
        /// 否
        /// </summary>
        public const string NO = "0";
    }

    /// <summary>
    /// 属性类型
    /// </summary>
    public class C_PropertyType
    {
        /// <summary>
        /// 单行文本
        /// </summary>
        public const string TEXT = "1";
        /// <summary>
        /// 多行文本
        /// </summary>
        public const string TEXT_AREA = "2";
        /// <summary>
        /// 下拉列表
        /// </summary>
        public const string DROPDOWN_LIST = "3";
        /// <summary>
        /// 日期选择
        /// </summary>
        public const string DATE_TIME = "4";
    }

    /// <summary>
    /// 微信类型
    /// </summary>
    public class C_WeixinType
    {
        /// <summary>
        /// 订阅号(已认证)
        /// </summary>
        public const string SUBSCRIPTION = "1";
        /// <summary>
        /// 服务号(未认证)
        /// </summary>
        public const string SERVICE = "2";
        /// <summary>
        /// 服务号(已认证)
        /// </summary>
        public const string CERTIFICATION = "3";
    }

    /// <summary>
    /// 公司类型
    /// </summary>
    public class C_ClientType
    {
        /// <summary>
        /// 区域
        /// </summary>
        public const string REGIONAL = "1";
        /// <summary>
        /// 经销商
        /// </summary>
        public const string DEALER = "2";
    }

    /// <summary>
    /// 回复类型
    /// </summary>
    public class C_ReplyType
    {
        /// <summary>
        /// 回复文字
        /// </summary>
        public const string TEXT = "1";
        /// <summary>
        /// 回复图文
        /// </summary>
        public const string GRAPHIC = "2";
        /// <summary>
        /// 回复图片
        /// </summary>
        public const string PICTURE = "3";
        /// <summary>
        /// 回复语音
        /// </summary>
        public const string VOICE = "4";
        /// <summary>
        /// 回复视频
        /// </summary>
        public const string VIDEO = "5";
    }

    /// <summary>
    /// 经销商区域
    /// </summary>
    public class C_DealerRegion
    {
        /// <summary>
        /// 上海小区
        /// </summary>
        public const string SHANGHAI = "D01";
        /// <summary>
        /// 无锡小区
        /// </summary>
        public const string WUXI = "D02";
        /// <summary>
        /// 南京小区
        /// </summary>
        public const string NANJING = "D03";
        /// <summary>
        /// 苏州小区
        /// </summary>
        public const string SUZHOU = "D04";
        /// <summary>
        /// 南通小区
        /// </summary>
        public const string NANTONG = "D05";
        /// <summary>
        /// 杭州小区
        /// </summary>
        public const string HANGZHOU = "D06";
        /// <summary>
        /// 金华小区
        /// </summary>
        public const string JINHUA = "D07";
        /// <summary>
        /// 宁波小区
        /// </summary>
        public const string NINGBO = "D08";
        /// <summary>
        /// 温州小区
        /// </summary>
        public const string WENZHOU = "D09";
        /// <summary>
        /// 皖南小区
        /// </summary>
        public const string WANNAN = "D10";
        /// <summary>
        /// 皖北小区
        /// </summary>
        public const string WANBEI = "D11";
    }

    /// <summary>
    /// 区域划分
    /// </summary>
    public class C_RegionPartition
    {
        /// <summary>
        /// 华北地区
        /// </summary>
        public const string NORTHChINA = "1";
        /// <summary>
        /// 东北地区
        /// </summary>
        public const string NORTHEAST = "2";
        /// <summary>
        /// 华东地区
        /// </summary>
        public const string EASTCHINA = "3";
        /// <summary>
        /// 华中地区
        /// </summary>
        public const string CENTRALCHINA = "4";
        /// <summary>
        /// 华南地区
        /// </summary>
        public const string SOUTHCHINA = "5";
        /// <summary>
        /// 西南地区
        /// </summary>
        public const string SOUTHWEST = "6";
        /// <summary>
        /// 西北地区
        /// </summary>
        public const string NORTHWEST = "7";
        /// <summary>
        /// 港澳台地区
        /// </summary>
        public const string SPECIALAREA = "8";
    }

    /// <summary>
    /// 客户类型
    /// </summary>
    public class C_CustomerType
    {
        /// <summary>
        /// 粉丝
        /// </summary>
        public const string FANS = "1";
        /// <summary>
        /// 潜客
        /// </summary>
        public const string POTENTIAL = "2";
        /// <summary>
        /// 车主
        /// </summary>
        public const string OWNER = "3";
        /// <summary>
        /// 员工
        /// </summary>
        public const string EMPLOYEE = "4";
    }

    /// <summary>
    /// 内容应用范围
    /// </summary>
    public class C_ContentApplicationScope
    {
        /// <summary>
        /// 市场活动
        /// </summary>
        public const string MARKETACTIVITY = "1";
        /// <summary>
        /// 图文素材
        /// </summary>
        public const string MESSAGE = "2";
    }

    /// <summary>
    /// 内容展现范围
    /// </summary>
    public class C_ContentShowScope
    {
        /// <summary>
        /// 图文
        /// </summary>
        public const string GRAPHIC = "1";
        /// <summary>
        /// 报名
        /// </summary>
        public const string APPLY = "2";
        /// <summary>
        /// 投票
        /// </summary>
        public const string VOTE = "3";
        /// <summary>
        /// 视频
        /// </summary>
        public const string VIDEO = "4";
        /// <summary>
        /// 互动游戏
        /// </summary>
        public const string GAME = "5";
    }

    /// <summary>
    /// 计划购车时间
    /// </summary>
    public class C_PlanBuyTime
    {
        /// <summary>
        /// 三个月
        /// </summary>
        public const string THREEMONTHS = "1";
        /// <summary>
        /// 六个月
        /// </summary>
        public const string SIXMONTHS = "2";
        /// <summary>
        /// 一年
        /// </summary>
        public const string ONEYEAR = "3";
    }

    /// <summary>
    /// 图文素材类型
    /// </summary>
    public class C_NewsType
    {
        /// <summary>
        /// 微信菜单
        /// </summary>
        public const string MicroLetterMenu = "1";
        /// <summary>
        /// 被添加自动回复
        /// </summary>
        public const string AutomaticallyReply = "2";
        /// <summary>
        /// 消息自动回复
        /// </summary>
        public const string MessageAutoReply = "3";
        /// <summary>
        /// 关键词自动回复
        /// </summary>
        public const string KeywordsAutoReply = "4";
        /// <summary>
        /// 微信群发
        /// </summary>
        public const string WechatMassSend = "5";
    }

    /// <summary>
    /// 关注状态
    /// </summary>
    public class C_SubscribeStatus
    {
        /// <summary>
        /// 关注
        /// </summary>
        public const string Attention = "1";
        /// <summary>
        /// 取消关注
        /// </summary>
        public const string CacelAttention = "2";
    }

    /// <summary>
    /// 投票类型
    /// </summary>
    public class C_VoteType
    {
        /// <summary>
        /// 单选
        /// </summary>
        public const string Single = "1";
        /// <summary>
        /// 多选
        /// </summary>
        public const string Multiple = "2";
    }

    /// <summary>
    /// 业务类型
    /// </summary>
    public class Business_Type
    {
        /// <summary>
        /// 预约保养
        /// </summary>
        public const string MaintenanceOrder = "1";
        /// <summary>
        /// 预约维修
        /// </summary>
        public const string RepairOrder = "2";
        /// <summary>
        /// 预约试驾
        /// </summary>
        public const string TestDriveOrder = "3";
        /// <summary>
        /// 在线询价
        /// </summary>
        public const string AskPrice = "4";
        /// <summary>
        /// 在线咨询
        /// </summary>
        public const string Consult = "5";

    }

    /// <summary>
    /// 游戏类型
    /// </summary>
    public class C_GameType
    {
        /// <summary>
        /// 得分类
        /// </summary>
        public const string Classification = "1";
        /// <summary>
        /// 限时类
        /// </summary>
        public const string Timeclass = "2";
        /// <summary>
        /// 限时得分类
        /// </summary>
        public const string Timeclassification = "3";
    }

    /// <summary>
    /// 初始化系统消息提示
    /// </summary>
    public class C_MessageType
    {
        /// <summary>
        /// 预约保养
        /// </summary>
        public const string MaintenanceOrder = "1";
        /// <summary>
        /// 预约维修
        /// </summary>
        public const string RepairOrder = "2";
        /// <summary>
        /// 预约试驾
        /// </summary>
        public const string TestDriveOrder = "3";
        /// <summary>
        /// 在线咨询
        /// </summary>
        public const string Consult = "4";
    }

    /// <summary>
    /// 资讯
    /// </summary>
    public class C_Information
    {
        /// <summary>
        /// 售前资讯父类id
        /// </summary>
        public const string PresaleInformationId = "1";
        /// <summary>
        /// 售后资讯父类id
        /// </summary>
        public const string AftersalesInformationId = "2";
        /// <summary>
        /// 售前资讯id
        /// </summary>
        public const string PresaleId = "1";
        /// <summary>
        /// 售前资讯id
        /// </summary>
        public const string AftersalesId = "2";
    }
}
